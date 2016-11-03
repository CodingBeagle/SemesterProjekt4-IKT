using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace DatabaseAPI.TableItemSectionPlacement
{
    public class SqlTableItemSectionPlacement : ITableItemSectionPlacement
    {
        private SqlConnection _conn;
        private SqlDataReader _reader;
        private SqlCommand _cmd;
        private string _connString;
        private string _sqlQueryString;

        public SqlTableItemSectionPlacement(string connectionString)
        {
            _connString = connectionString;
        }

        //Creates new touple in itemSectionplacement.
        //Returns the ItemSectionPlacementID for that touple
        public void PlaceItem(long itemID, long sectionID)
        {
            _sqlQueryString = @"INSERT INTO ItemSectionPlacement (ItemID, StoreSectionID) " +
                                    @"VALUES (@itemID, @sectionID);";
            using (_conn = new SqlConnection(_connString))
            {

                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@itemID", itemID);
                _cmd.Parameters.AddWithValue("@sectionID", sectionID);

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in PlaceItem: " + e.Message);
                }
            }
        }

        //Finds all ItemIDs connected to the sectionID
        //Uses said ItemIDs to get the Item objects from DB
        //Returns list of said Item objects
        public List<Item> ListItemsInSection(long sectionID)
        {
            List<Item> itemsInSection = new List<Item>();

            Item newItem;

            _sqlQueryString = @"SELECT * FROM ItemSectionPlacement 
                                    INNER JOIN Item 
                                    ON Item.ItemID = ItemSectionPlacement.ItemID 
                                    WHERE StoreSectionID= @sectionID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@sectionID", sectionID);

                try
                {
                    _conn.Open();
                    _reader = _cmd.ExecuteReader();

                    while (_reader.Read())
                    {
                        long _itemID = (long) _reader["ItemID"];
                        string _itemName = (string) _reader["Name"];
                        long _itemGroupID = (long) _reader["ItemGroupID"];

                        newItem = new Item(_itemID, _itemName, _itemGroupID);
                        itemsInSection.Add(newItem);
                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in ListItemsInSection: " + e.Message);
                }
            }
            return itemsInSection;
        }

        public List<StoreSection> FindPlacementsByItem(long itemID)
        {
            List<StoreSection> sectionList = new List<StoreSection>();

            _sqlQueryString = @"SELECT * FROM ItemSectionPlacement " +
                                    @"WHERE ItemID = @itemID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@itemID", itemID);

                try
                {
                    _conn.Open();
                    _reader = _cmd.ExecuteReader();
                    while (_reader.Read())
                    {
                        if (!_reader.IsDBNull(_reader.GetOrdinal("StoreSectionID")))
                        {
                            long storeSectionID = (long) _reader["StoreSectionID"];

                            DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());
                            StoreSection tempStoreSec = db.TableStoreSection.GetStoreSection(storeSectionID);
                            sectionList.Add(tempStoreSec);
                        }
                    }
                }
                catch (Exception e)
                {

                    Debug.WriteLine("Something went wrong in FindPlacementsByItem: " + e.Message);
                }
            }
            return sectionList;
        }

        public void DeleteAllPlacementsInSection(long sectionID)
        {

            _sqlQueryString = @"DELETE FROM ItemSectionPlacement " +
                                    @"WHERE StoreSectionID = @sectionID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@sectionID", sectionID);

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                    Debug.WriteLine("Something went wrong in DeleteAllPlacementsInSection: " + e.Message);
                }
            }

        }

        public void DeletePlacementsByItem(long itemID)
        {
            _sqlQueryString = @"DELETE FROM ItemSectionPlacement " +
                                @"WHERE itemId = @itemID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@itemID", itemID);

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {

                    Debug.WriteLine("Something went wrong in DeletePlacementsByItem: " + e.Message);
                }
            }

        }

        public void DeletePlacement(long itemID, long sectionID)
        {
            _sqlQueryString = @"DELETE FROM ItemSectionPlacement " +
                                    @"WHERE ItemID = @itemID And StoreSectionID = @sectionID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@itemID", itemID);
                _cmd.Parameters.AddWithValue("@sectionID", sectionID);

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in DeletePlacement: " + e.Message);
                }
            }

           
        }
        
    }
}