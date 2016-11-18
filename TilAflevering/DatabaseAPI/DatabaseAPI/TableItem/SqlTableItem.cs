using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItem
{
    public class SqlTableItem : ITableItem
    {
        private SqlConnection _conn;
        private SqlDataReader _reader;
        private SqlCommand _cmd;
        private string _connString;
        private string _sqlQueryString;

        public SqlTableItem(string connectionString)
        {
            _connString = connectionString;
        }

        public long CreateItem(string itemName, long itemGroupID)
        {
            long createdID = 0;

            _sqlQueryString = @"INSERT INTO Item (Name, ItemGroupID) 
                              VALUES ( @itemName, @itemGroupID); 
                              SELECT CAST(scope_identity() AS BIGINT)";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@itemName", itemName);
                _cmd.Parameters.AddWithValue("@itemGroupID", itemGroupID);

                try
                {
                    _conn.Open();
                    createdID = (long) _cmd.ExecuteScalar();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in CreateItem: " + e.Message);
                }
            }
            return createdID;
        }

        public void DeleteItem(long itemID)
        {
            _sqlQueryString = @"DELETE FROM Item 
                                WHERE ItemID = @itemID";

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
                    Debug.WriteLine("Something went wrong in DeleteItem: " + e.Message);
                }
            }
        }

        public List<Item> SearchItems(string itemName)
        {
            string itemNameWithWildCards = string.Format("%{0}%", itemName);
            List<Item> listOfItems = new List<Item>();

            _sqlQueryString = @"SELECT * FROM Item WHERE Name LIKE @itemName";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@itemName", itemNameWithWildCards);

                try
                {
                    _conn.Open();
                    _reader = _cmd.ExecuteReader();

                    while (_reader.Read())
                    {
                        Item localItem = new Item(0, "", 0);

                        if (!_reader.IsDBNull(_reader.GetOrdinal("ItemID")))
                            localItem.ItemID = (long) _reader["ItemID"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                            localItem.Name = (string) _reader["Name"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("itemGroupID")))
                            localItem.ItemGroupID = (long) _reader["ItemGroupID"];

                        listOfItems.Add(localItem);

                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in SearchItem: " + e.Message);
                }
            }
            return listOfItems;
        }

        public Item GetItem(long itemID)
        {
            Item localItem = null;

            _sqlQueryString = @"SELECT * FROM Item WHERE ItemID = @itemID";

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
                        localItem = new Item(0, "", 0);
                        if (!_reader.IsDBNull(_reader.GetOrdinal("ItemID")))
                            localItem.ItemID = (long)_reader["ItemID"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                            localItem.Name = (string)_reader["Name"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("itemGroupID")))
                            localItem.ItemGroupID = (long)_reader["ItemGroupID"];
                    }

                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in GetItem: " + e.Message);
                }
            }
            return localItem;
        }
    }
}