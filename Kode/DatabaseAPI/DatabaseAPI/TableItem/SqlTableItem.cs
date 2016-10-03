using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItem
{
    public class SqlTableItem : ITableItem
    {
        private SqlConnection _conn;
        private SqlDataReader _reader;
        private SqlCommand _cmd;

        public SqlTableItem()
        {
            _conn = new SqlConnection("Server=tcp:storedatabase.database.windows.net,1433;Initial Catalog=StoreDatabase;Persist Security Info=False;User ID={Rieder};Password={Poelse$69};MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;");
        }

        public void CreateItem(string name, int itemGroup)
        {
            try
            {
                _conn.Open();

                string sqlInsertCommand = $"INSERT INTO Item (Name, ItemGroupID)" +
                                          $"VALUES  ('" + name + "', '" + itemGroup + "')";

                _cmd = new SqlCommand(sqlInsertCommand) {Connection = _conn};

                _cmd.ExecuteNonQuery();
            }
            finally
            {
                if (_conn != null) _conn.Close();
            }

        }

        public void DeleteItem(int ID)
        {
            try
            {
                _conn.Open();
                string sqlDeleteCommand = $"DELETE FROM Item" +
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

                string cmdText = $"SELECT * FROM Item" +
                                 $"WHERE Name LIKE '" + itemName + "'";

                _cmd = new SqlCommand(cmdText, _conn);

                _reader = _cmd.ExecuteReader();

                while (_reader.Read())
                {
                    var localItem = new Item((int)_reader["ItemID"], (string)_reader["Name"], (int)_reader["ItemGroupID"]);
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