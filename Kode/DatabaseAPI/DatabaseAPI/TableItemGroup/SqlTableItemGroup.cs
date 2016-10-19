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
        private SqlDataReader _dataReader;


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

                _dataReader = _cmd.ExecuteReader();

                    while (_dataReader.Read())
                    {
                        if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("Name")))
                        {
                            name = (string) _dataReader["Name"];
                        }
                        if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("ParentItemGroupID")))
                        {
                            itemGroupParentID = (long) _dataReader["ParentItemGroupID"];
                        }
                        if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("ItemGroupID")))
                        {
                            itemGroupID = (long) _dataReader["ItemGroupID"];
                        }
                    searchResults.Add(new ItemGroup(name, itemGroupParentID, itemGroupID));
                }
                    
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            finally
            {
                _dataReader?.Close();
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

                _dataReader = _cmd.ExecuteReader();

                if (_dataReader.HasRows)
                {
                    while (_dataReader.Read())
                    {
                        itemGroupName = (string) _dataReader["Name"];

                        if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("ParentItemGroupID")))
                        {
                            itemGroupParentID = (long) _dataReader["ParentItemGroupID"];
                        }
                        ItemGroupResult = new ItemGroup(itemGroupName, itemGroupParentID, itemGroupID);
                    }
                }

            }
            finally
            {
                _dataReader?.Close();
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

                _dataReader = _cmd.ExecuteReader();


                while (_dataReader.Read())
                {
                    string itemGroupName = "";
                    long itemGroupID = 0;
                    long parentItemGroupId = 0;

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("Name")))
                        itemGroupName = (string) _dataReader["Name"];

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("ItemGroupID")))
                        itemGroupID = (long) _dataReader["ItemGroupID"];

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("ParentItemGroupID")))
                    {
                        var rItemGroupID = _dataReader["ParentItemGroupID"];
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
                _dataReader?.Close();
            }
            return itemGroupQuery;
        }
    }
}