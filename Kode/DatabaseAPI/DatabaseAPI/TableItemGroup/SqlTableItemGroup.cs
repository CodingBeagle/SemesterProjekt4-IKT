using System;
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


        public SqlTableItemGroup(IConnectionStringFactory factory)
        {
            _connection = new SqlConnection(factory.CreateConnectionString());
        }

        public void CreateItemGroup(string itemGroupName, int itemGroupParentID)
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

        public void DeleteItemGroup(int itemGroupID)
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

       
        public ItemGroup GetItemGroup(int itemGroupID)
        { 
            ItemGroup ItemGroupResult = null;
            string itemGroupName;
            long itemGroupParentID = 0;

            try
            {
                _connection.Open();

                _command = new SqlCommand($"SELECT  * FROM ItemGroup WHERE ItemGroupID = {itemGroupID}",_connection);

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
    
    }
}