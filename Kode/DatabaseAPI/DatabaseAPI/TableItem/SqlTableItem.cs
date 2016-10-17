using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using System.Linq;

namespace DatabaseAPI.TableItem
{
    public class SqlTableItem : ITableItem
    {
        private SqlConnection _conn;
        private SqlDataReader _reader;
        private SqlCommand _cmd;

        public SqlTableItem(string connectionString)
        {
            _conn = new SqlConnection(connectionString);
        }

        public long CreateItem(string name, long itemGroupId)
        {
            long createdID;
            try
            {
                _conn.Open();

                string sqlInsertCommand = $"INSERT INTO Item (Name, ItemGroupID)" +
                                          $"VALUES ('" + name + "', " + itemGroupId + ");" +
                                          "SELECT CAST(scope_identity() AS BIGINT)";

                _cmd = new SqlCommand(sqlInsertCommand) {Connection = _conn};

                createdID = (long) _cmd.ExecuteScalar();
            }
            finally
            {
                if (_conn != null) _conn.Close();
            }
            return createdID;

        }

        public void DeleteItem(long ID)
        {
            try
            {
                _conn.Open();
                string sqlDeleteCommand = $"DELETE FROM Item " +
                                          $"WHERE ItemId = " + ID;

                _cmd = new SqlCommand(sqlDeleteCommand) {Connection = _conn};

                _cmd.ExecuteNonQuery();
            }
            finally
            {
                if (_conn != null) _conn.Close();
            }

        }

        public List<Item> SearchItems(string itemName)
        {
            List<Item> listOfItems = new List<Item>();
            try
            {
                _conn.Open();

                string cmdText = $"SELECT * FROM Item " +
                                 $"WHERE Name LIKE '%{itemName}%'";

                Debug.WriteLine(cmdText);

                _cmd = new SqlCommand(cmdText, _conn);

                _reader = _cmd.ExecuteReader();
                
                while (_reader.Read())
                {
                    Item localItem = new Item(0, "", 0);

                    if (!_reader.IsDBNull(_reader.GetOrdinal("ItemID")))
                        localItem.ItemID = (long)_reader["ItemID"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                        localItem.Name = (string)_reader["Name"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("itemGroupID")))
                        localItem.ItemGroupID = (long)_reader["ItemGroupID"];

                    listOfItems.Add(localItem);
                }               
            }
            finally
            {
                _reader?.Close();
                _conn?.Close();
            }
            return listOfItems;
        }

        public Item GetItem(long itemID)
        {
            Item localItem = null;
            try
            {
                _conn.Open();
                
                string cmdText = $"SELECT * FROM Item " +
                                 $"WHERE ItemID = {itemID}";
                _cmd = new SqlCommand(cmdText, _conn);

                _reader = _cmd.ExecuteReader();

                while (_reader.Read())
                {
                    localItem = new Item(0,"",0);
                    if (!_reader.IsDBNull(_reader.GetOrdinal("ItemID")))
                        localItem.ItemID = (long)_reader["ItemID"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                        localItem.Name = (string)_reader["Name"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("itemGroupID")))
                        localItem.ItemGroupID = (long)_reader["ItemGroupID"];
                }               
            }
            finally
            {
                _reader?.Close();
                _conn?.Close();
            }
            return localItem;
        }
    }
}