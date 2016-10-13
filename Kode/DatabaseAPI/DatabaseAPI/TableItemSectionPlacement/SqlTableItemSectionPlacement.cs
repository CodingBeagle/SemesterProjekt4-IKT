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
        DatabaseService db = new DatabaseService(new SqlStoreDatabaseFactory());


        public SqlTableItemSectionPlacement(string connectionString)
        {
            _conn = new SqlConnection(connectionString);
        }

        //Creates new touple in itemSectionplacement.
        //Returns the ItemSectionPlacementID for that touple
        public long PlaceItem(long itemID, long sectionID)
        {
            long itemSectionPlacementID;
            try
            {
                _conn.Open();

                string sqlInsertCommand = $"INSERT INTO ItemSectionPlacement (ItemID, StoreSectionID)" +
                                          $"VALUES ({itemID}, {sectionID});" +
                                          $"SELECT CAST(scope_identity() AS BIGINT)";

                _cmd = new SqlCommand(sqlInsertCommand, _conn);

                itemSectionPlacementID = (long) _cmd.ExecuteScalar();
            }
            finally
            {
                _conn?.Close();
            }
            return itemSectionPlacementID;
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

                string sqlCommandString = $"SELECT * FROM ItemSectionPlacement" +
                                          $"WHERE StoreSectionID = {sectionID};";

                _cmd = new SqlCommand(sqlCommandString, _conn);
                _reader = _cmd.ExecuteReader();

                while (_reader.Read())
                {
                    if(!_reader.IsDBNull(_reader.GetOrdinal("ItemID")))
                    {
                        itemIDList.Add((long)_reader["ItemID"]);
                    }
                }

                //Get items in the database, using the IDs from the ItemSectionPlacement
                foreach (var ID in itemIDList)
                {
                    var localItem = db.TableItem.GetItem(ID);
                    itemsInSection.Add(localItem);
                }
            }
            finally 
            {
                _conn?.Close();
                _reader?.Close();
            }
            return itemsInSection;
        }

        public StoreSection FindPlacementByItem(long ItemID)
        {
            throw new System.NotImplementedException();
        }

        public void DeletePlacement(long itemSectionPlacementID)
        {
            throw new NotImplementedException();
        }
    }
}