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


        public SqlTableItemSectionPlacement(string connectionString)
        {
            _conn = new SqlConnection(connectionString);
        }

        //Creates new touple in itemSectionplacement.
        //Returns the ItemSectionPlacementID for that touple
        public void PlaceItem(long itemID, long sectionID)
        {
            try
            {
                _conn.Open();

                string sqlInsertCommand = $"INSERT INTO ItemSectionPlacement (ItemID, StoreSectionID) " +
                                          $"VALUES ({itemID}, {sectionID});";

                _cmd = new SqlCommand(sqlInsertCommand, _conn);

                _cmd.ExecuteNonQuery();
            }
            finally
            {
                _conn?.Close();
            }
        }

        //Finds all ItemIDs connected to the sectionID
        //Uses said ItemIDs to get the Item objects from DB
        //Returns list of said Item objects
        public List<Item> ListItemsInSection(long sectionID)
        {
            List<Item> itemsInSection = new List<Item>();
          
            Item newItem;

            try
            {
                _conn.Open();

                string sqlCommandString = $"SELECT * FROM ItemSectionPlacement" +
                                          $" INNER JOIN Item " +
                                          $" ON Item.ItemID = ItemSectionPlacement.ItemID" +
                                          $" WHERE StoreSectionID='" + sectionID + "'";

                _cmd = new SqlCommand(sqlCommandString, _conn);
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
                Debug.WriteLine("Something went wrong: " + e.Message);
            }
            finally
            {
                _conn?.Close();
                _reader?.Close();
            }
            return itemsInSection;
        }

        public List<StoreSection> FindPlacementsByItem(long ItemID)
        {
            List<StoreSection> secList = new List<StoreSection>();
            try
            {
                _conn.Open();

                string sqlCommandString = $"SELECT * FROM ItemSectionPlacement " +
                                          $"WHERE ItemID = {ItemID};";

                _cmd = new SqlCommand(sqlCommandString, _conn);

                _reader = _cmd.ExecuteReader();
                while (_reader.Read())
                {
                    if (!_reader.IsDBNull(_reader.GetOrdinal("StoreSectionID")))
                    {                                                
                        long sSecID = (long) _reader["StoreSectionID"];

                        DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());
                        StoreSection tStoreSec = db.TableStoreSection.GetStoreSection(sSecID);
                        secList.Add(tStoreSec);
                    }
                }
                
                return secList;
            }
            finally
            {
                _conn?.Close();
                _reader?.Close();
            }
        }

        public void DeleteAllPlacementsInSection(long sectionId)
        {
            try
            {
                _conn.Open();

                string sqlCommand = $"DELETE FROM ItemSectionPlacement " +
                                    $"WHERE StoreSectionID = {sectionId};";

                _cmd = new SqlCommand(sqlCommand, _conn);
                _cmd.ExecuteNonQuery();
            }
            finally
            {
                _conn?.Close();
            }
        }

        public void DeletePlacementsByItem(long itemId)
        {
            try
            {
                _conn.Open();

                string sqlCommand = $"DELETE FROM ItemSectionPlacement " +
                                    $"WHERE itemId = {itemId};";

                _cmd = new SqlCommand(sqlCommand, _conn);
                _cmd.ExecuteNonQuery();
            }
            finally
            {
                _conn?.Close();
            }
        }

        public void DeletePlacement(long itemId, long sectionId)
        {
            try
            {
                _conn.Open();

                string sqlCommand = $"DELETE FROM ItemSectionPlacement " +
                                    $"WHERE ItemID = {itemId} And StoreSectionID = {sectionId};";

                _cmd = new SqlCommand(sqlCommand, _conn);
                _cmd.ExecuteNonQuery();
            }
            finally
            {
                _conn?.Close();
            }
        }
    }
}