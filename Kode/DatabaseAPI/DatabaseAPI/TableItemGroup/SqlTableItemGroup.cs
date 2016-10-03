using System;
using System.Data.SqlClient;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItemGroup
{
    public class SqlTableItemGroup : ITableItemGroup
    {
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlDataReader _dataReader;


        public SqlTableItemGroup()
        {
            _connection = new SqlConnection("Server=tcp:storedatabase.database.windows.net,1433;Initial Catalog=StoreDatabase;Persist Security Info=False;User ID=Rieder;Password=Poelse$69;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
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