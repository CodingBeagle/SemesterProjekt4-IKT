using System;
using System.CodeDom;
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
        private string _connString;
        private string _sqlQueryString;


        public SqlTableItemGroup(string connectionString)
        {
            _connString = connectionString;
        }

        public long CreateItemGroup(string itemGroupName, long itemGroupParentID)
        {
            long createdID = 0;

            _sqlQueryString = @"INSERT INTO ItemGroup (Name, ParentItemGroupID) 
                              VALUES (@itemGroupName, @itemGroupParentID)" +
                              @"SELECT CAST(scope_identity() AS BIGINT)";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@itemGroupName", itemGroupName);
                _cmd.Parameters.AddWithValue("@itemGroupParentID", itemGroupParentID);

                try
                {
                    _conn.Open();
                    createdID = (long) _cmd.ExecuteScalar();

                }
                catch (Exception e)
                {

                    Debug.WriteLine("Something went wrong in CreateItemGroup: " + e.Message);
                }
            }
            return createdID;
        }

        public long CreateItemGroup(string itemGroupName)
        {
            long createdID = 0;

            _sqlQueryString = @"INSERT INTO ItemGroup (Name) " +
                              @"VALUES (@itemGroupName); " +
                              @"SELECT CAST(scope_identity() AS BIGINT)";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@itemGroupName", itemGroupName);

                try
                {
                    _conn.Open();
                    createdID = (long)_cmd.ExecuteScalar();

                }
                catch (Exception e)
                {

                    Debug.WriteLine("Something went wrong in CreateItemGroup: " + e.Message);
                }
            }
            return createdID;
        }

        public List<ItemGroup> SearchItemGroups(string itemGroupName)
        {
            string itemGroupNameWithWildCards = string.Format("%{0}%",itemGroupName);
            List<ItemGroup> searchResults = new List<ItemGroup>();
            string name = null;
            long itemGroupParentID = 0;
            long itemGroupID = 0;

            _sqlQueryString = @"SELECT * FROM ItemGroup WHERE Name LIKE @itemGroupName";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@itemGroupName", itemGroupNameWithWildCards);

                try
                {
                    _conn.Open();
                    _reader = _cmd.ExecuteReader();

                    if (_reader.HasRows)
                    {
                        while (_reader.Read())
                        {
                            if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                            {
                                name = (string)_reader["Name"];
                            }
                            if (!_reader.IsDBNull(_reader.GetOrdinal("ParentItemGroupID")))
                            {
                                itemGroupParentID = (long)_reader["ParentItemGroupID"];
                            }
                            if (!_reader.IsDBNull(_reader.GetOrdinal("ItemGroupID")))
                            {
                                itemGroupID = (long)_reader["ItemGroupID"];
                            }
                            searchResults.Add(new ItemGroup(name, itemGroupParentID, itemGroupID));

                            name = null;
                            itemGroupParentID = 0;
                            itemGroupID = 0;
                        }
                    }
                   

                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in SearchItemGroups: " + e.Message);
                }
            }
            return searchResults;
        }

        public void UpdateItemGroup(string oldItemGroupName, string itemGroupName)
        {
            _sqlQueryString = @"UPDATE ItemGroup SET Name= @itemGroupName 
                                WHERE Name= @oldItemGroupName";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue(@"itemGroupName", itemGroupName);
                _cmd.Parameters.AddWithValue(@"oldItemGroupName", oldItemGroupName);

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in UpdateItemGroup: " + e.Message);
                }
            }

        }

        public void DeleteItemGroup(long itemGroupID)
        {
            _sqlQueryString = @"DELETE FROM ItemGroup WHERE ItemGroupID = @itemGroupID";
            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue(@"itemGroupID", itemGroupID);

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in DeleteItemGroup: " + e.Message);
                  
                }
            }
        }


        public ItemGroup GetItemGroup(long itemGroupID)
        {
            ItemGroup ItemGroupResult = null;
            string itemGroupName;
            long itemGroupParentID = 0;

            _sqlQueryString = @"SELECT * FROM ItemGroup WHERE ItemGroupID = @itemGroupID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@itemGroupID", itemGroupID);

                try
                {
                    _conn.Open();
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
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in GetItemGroup: " + e.Message);
                }

                
            }
            return ItemGroupResult;
        }

        public List<ItemGroup> GetAllItemGroups()
        {
            List<ItemGroup> itemGroupList = new List<ItemGroup>();
            string itemGroupName = "";
            long itemGroupID = 0;
            long parentItemGroupId = 0;

            _sqlQueryString = "SELECT * FROM ItemGroup";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);

                try
                {
                    _conn.Open();
                    _reader = _cmd.ExecuteReader();

                    while (_reader.Read())
                    {

                        if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                            itemGroupName = (string) _reader["Name"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("ItemGroupID")))
                            itemGroupID = (long) _reader["ItemGroupID"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("ParentItemGroupID")))
                        {
                            var rItemGroupID = _reader["ParentItemGroupID"];
                            if (rItemGroupID != null)
                                parentItemGroupId = (long) rItemGroupID;
                        }
                        var newItem = new ItemGroup(itemGroupName, parentItemGroupId, itemGroupID);
                        itemGroupList.Add(newItem);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in GetAllItemGroups: " + e.Message);
                }
            }
            return itemGroupList;
        }
    }
}