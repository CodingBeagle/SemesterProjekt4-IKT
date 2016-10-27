using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItemGroup
{
    public class SqlTableItemGroup : ITableItemGroup
    {
        private SqlConnection _conn;
        private SqlCommand _cmd;
        private SqlDataReader _reader;


        public SqlTableItemGroup(string connectionString)
        {
            _conn = new SqlConnection(connectionString);
        }

        public long CreateItemGroup(string itemGroupName, long itemGroupParentID)
        {
            long _createdID;
            try
            {
                _conn.Open();

                _cmd =
                    new SqlCommand(
                        $"INSERT INTO ItemGroup (Name, ParentItemGroupID) VALUES ('" + itemGroupName + "', '" +
                        itemGroupParentID + "'); SELECT CAST(scope_identity() AS BIGINT)",
                        _conn);

                _createdID = (long) _cmd.ExecuteScalar();
            }
            finally
            {
                _conn?.Close();
            }
            return _createdID;
        }

        public long CreateItemGroup(string itemGroupName)
        {
            long _createdID;
            try
            {
                _conn.Open();

                _cmd =
                    new SqlCommand(
                        $"INSERT INTO ItemGroup (Name) VALUES ('" + itemGroupName + "');" +
                        "SELECT CAST(scope_identity() AS BIGINT)",
                        _conn);

                _createdID = (long) _cmd.ExecuteScalar();
            }
            finally
            {
                _conn?.Close();
            }
            return _createdID;
        }

        public List<ItemGroup> SearchItemGroups(string itemGroupName)
        {
            List<ItemGroup> searchResults = new List<ItemGroup>();
            string name = null;
            long itemGroupParentID = 0;
            long itemGroupID = 0;
            try
            {
                _conn.Open();

                string sqlCommand = $"SELECT * FROM ItemGroup WHERE Name LIKE '%{itemGroupName}%'";
                _cmd = new SqlCommand(sqlCommand, _conn);

                _reader = _cmd.ExecuteReader();

                    while (_reader.Read())
                    {
                        if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                        {
                            name = (string) _reader["Name"];
                        }
                        if (!_reader.IsDBNull(_reader.GetOrdinal("ParentItemGroupID")))
                        {
                            itemGroupParentID = (long) _reader["ParentItemGroupID"];
                        }
                        if (!_reader.IsDBNull(_reader.GetOrdinal("ItemGroupID")))
                        {
                            itemGroupID = (long) _reader["ItemGroupID"];
                        }
                    searchResults.Add(new ItemGroup(name, itemGroupParentID, itemGroupID));
                    name = null;
                    itemGroupParentID = 0;
                    itemGroupID = 0;
                }
                    
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                _reader?.Close();
                _conn?.Close();
            }
            return searchResults;
        }

        public void UpdateItemGroup(string oldItemGroupName, string itemGroupName)
        {
            try
            {
                _conn.Open();
                string sqlCommand = $"UPDATE ItemGroup SET Name='{itemGroupName}' WHERE Name='{oldItemGroupName}'";
                _cmd = new SqlCommand(sqlCommand, _conn);

                _cmd.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                _conn?.Close();
            }
        }

        public void DeleteItemGroup(long itemGroupID)
        {
            try
            {
                _conn.Open();

                _cmd = new SqlCommand($"DELETE FROM ItemGroup WHERE ItemGroupID = " + itemGroupID,
                    _conn);

                _cmd.ExecuteNonQuery();

            }
            finally
            {
                _conn?.Close();

            }
        }


        public ItemGroup GetItemGroup(long itemGroupID)
        {
            ItemGroup ItemGroupResult = null;
            string itemGroupName;
            long itemGroupParentID = 0;

            try
            {
                _conn.Open();

                _cmd = new SqlCommand($"SELECT  * FROM ItemGroup WHERE ItemGroupID = {itemGroupID}", _conn);

                _reader = _cmd.ExecuteReader();

                if (_reader.HasRows)
                {
                    while (_reader.Read())
                    {
                        itemGroupName = (string) _reader["Name"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("ParentItemGroupID")))
                        {
                            itemGroupParentID = (long) _reader["ParentItemGroupID"];
                        }
                        ItemGroupResult = new ItemGroup(itemGroupName, itemGroupParentID, itemGroupID);
                    }
                }

            }
            finally
            {
                _reader?.Close();
                _conn?.Close();

            }

            return ItemGroupResult;
        }

        public List<ItemGroup> GetAllItemGroups()
        {
            List<ItemGroup> itemGroupQuery = new List<ItemGroup>();
            try
            {
                _conn.Open();
                _cmd = new SqlCommand("SELECT * FROM ItemGroup", _conn);

                _reader = _cmd.ExecuteReader();


                while (_reader.Read())
                {
                    string itemGroupName = "";
                    long itemGroupID = 0;
                    long parentItemGroupId = 0;

                    if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                        itemGroupName = (string) _reader["Name"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("ItemGroupID")))
                        itemGroupID = (long) _reader["ItemGroupID"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("ParentItemGroupID")))
                    {
                        var rItemGroupID = _reader["ParentItemGroupID"];
                        if (rItemGroupID != null)
                            parentItemGroupId = (long)rItemGroupID;
                    }
                    var newItem = new ItemGroup(itemGroupName, parentItemGroupId, itemGroupID);
                    itemGroupQuery.Add(newItem);
                }
            }
            finally
            {
                _conn?.Close();
                _reader?.Close();
            }
            return itemGroupQuery;
        }
    }
}