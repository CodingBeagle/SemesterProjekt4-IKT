using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

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

        public void CreateItem(string name, long itemGroupId)
        {
            try
            {
                _conn.Open();

                string sqlInsertCommand = $"INSERT INTO Item (Name, ItemGroupID)" +
                                          $"VALUES ('"+ name +"', "+ itemGroupId+")";

                _cmd = new SqlCommand(sqlInsertCommand) {Connection = _conn};

                _cmd.ExecuteNonQuery();
            }
            finally
            {
                if (_conn != null) _conn.Close();
            }

        }

        public void DeleteItem(long ID)
        {
            try
            {
                _conn.Open();
                string sqlDeleteCommand = $"DELETE FROM Item " +
                                          $"WHERE ItemId = '" + ID + "'";

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

                long itemID = 0;
                string theName = "";
                long itemGroupID = 0;

                while (_reader.Read())
                {
                    if (!_reader.IsDBNull(_reader.GetOrdinal("ItemID")))
                        itemID = (long) _reader["ItemID"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                        theName = (string) _reader["Name"];

                    if (_reader.IsDBNull(_reader.GetOrdinal("itemGroupID")))
                        itemGroupID = (long) _reader["ItemGroupID"];

                    var localItem = new Item(itemID, theName, itemGroupID);
                    listOfItems.Add(localItem);
                }
                return listOfItems;
            }
            finally
            {
                _reader?.Close();
                _conn?.Close();
            }
        }
    }
}