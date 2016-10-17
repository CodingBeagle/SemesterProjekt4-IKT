using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
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
                                          $"VALUES ({itemID}, {sectionID}); " +
                                          $"SELECT CAST(scope_identity() AS BIGINT)";

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
            List<long> itemIDList = new List<long>();

            try
            {
                _conn.Open();

                string sqlCommandString = $"SELECT * FROM ItemSectionPlacement " +
                                          $"WHERE StoreSectionID = {sectionID}; ";

                _cmd = new SqlCommand(sqlCommandString, _conn);
                _reader = _cmd.ExecuteReader();

                while (_reader.Read())
                {
                    if (!_reader.IsDBNull(_reader.GetOrdinal("ItemID")))
                    {
                        itemIDList.Add((long) _reader["ItemID"]);
                    }
                }
                _conn?.Close();
                _reader?.Close();
                //Get items in the database, using the IDs from the ItemSectionPlacement
                DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());
                foreach (var ID in itemIDList)
                {
                    var localItem = db.TableItem.GetItem(ID);
                    itemsInSection.Add(localItem);
                }
                return itemsInSection;
            }
            catch(Exception e)
            {
                
            }
            return null;
        }

        public StoreSectionModel FindPlacementByItem(long ItemID)
        {
            long sectionID=0;
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
                        sectionID = (long)_reader["StoreSectionID"];
                    }
                }
                DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());
                var itemPlacement = db.TableStoreSection.GetStoreSection(sectionID);
                return itemPlacement;
            }
            finally 
            {
                _conn?.Close();
                _reader?.Close();
            }
        }

        public void DeleteAllPlacementsInSection(long sectionId)
        {
            throw new NotImplementedException();
        }

        public void DeletePlacementByItem(long itemId)
        {
            throw new NotImplementedException();
        }
    }
}