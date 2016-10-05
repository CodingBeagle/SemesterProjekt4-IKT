using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace DatabaseAPI.TableItemGroup
{
    public class SqlTableItemGroup : ITableItemGroup
    {
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlDataReader _dataReader;


        public SqlTableItemGroup(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public void CreateItemGroup(string itemGroupName, long itemGroupParentID)
        {
            try
            {
                _connection.Open();

                _command =
                    new SqlCommand(
                        $"INSERT INTO ItemGroup (Name, rItemGroupID) VALUES ('" + itemGroupName + "', '" +
                        itemGroupParentID + "')",
                        _connection);

                _command.ExecuteNonQuery();
            }
            finally
            {
                _connection?.Close();
            }
        }

        public void CreateItemGroup(string itemGroupName)
        {
            try
            {
                _connection.Open();

                _command =
                    new SqlCommand(
                        $"INSERT INTO ItemGroup (Name) VALUES ('" + itemGroupName + "')",
                        _connection);

                _command.ExecuteNonQuery();
            }
            finally
            {
                _connection?.Close();
            }
        }

        public void DeleteItemGroup(long itemGroupID)
        {
            try
            {
                _connection.Open();

                _command = new SqlCommand($"DELETE FROM ItemGroup WHERE ItemGroupID = '" + itemGroupID + "'",
                    _connection);

                _command.ExecuteNonQuery();

            }
            finally
            {
                _connection?.Close();

            }
        }


        public ItemGroup GetItemGroup(long itemGroupID)
        {
            ItemGroup ItemGroupResult = null;
            string itemGroupName;
            long itemGroupParentID = 0;

            try
            {
                _connection.Open();

                _command = new SqlCommand($"SELECT  * FROM ItemGroup WHERE ItemGroupID = {itemGroupID}", _connection);

                _dataReader = _command.ExecuteReader();

                if (_dataReader.HasRows)
                {
                    while (_dataReader.Read())
                    {
                        itemGroupName = (string) _dataReader["Name"];

                        if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("rItemGroupID")))
                        {
                            itemGroupParentID = (long) _dataReader["rItemGroupID"];
                        }
                        ItemGroupResult = new ItemGroup(itemGroupName, itemGroupParentID, itemGroupID);
                    }
                }

            }
            finally
            {
                _dataReader?.Close();
                _connection?.Close();

            }

            return ItemGroupResult;
        }

        public List<ItemGroup> GetAllItemGroups()
        {
            List<ItemGroup> itemGroupQuery = new List<ItemGroup>();
            try
            {
                _connection.Open();
                _command = new SqlCommand("SELECT * FROM ItemGroup", _connection);

                _dataReader = _command.ExecuteReader();


                while (_dataReader.Read())
                {
                    string itemGroupName = "";
                    long itemGroupID = 0;
                    long parentItemGroupId = 0;

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("Name")))
                        itemGroupName = (string) _dataReader["Name"];

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("ItemGroupID")))
                        itemGroupID = (long) _dataReader["ItemGroupID"];

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("rItemGroupID")))
                    {
                        var rItemGroupID = _dataReader["rItemGroupID"];
                        if (rItemGroupID != null)
                            parentItemGroupId = (long)rItemGroupID;
                    }
                    var newItem = new ItemGroup(itemGroupName, parentItemGroupId, itemGroupID);
                    itemGroupQuery.Add(newItem);
                }
            }
            finally
            {
                _connection?.Close();
                _dataReader?.Close();
            }
            return itemGroupQuery;
        }
    }
}