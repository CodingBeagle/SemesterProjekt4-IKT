using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableItem
{
    public class SqlTableItem : ITableItem
    {
        private SqlConnection conn;
        public static string sqlconnectionString { get; private set; }

        public void CreateItem(string name, int itemGroup)
        {
            sqlconnectionString = "herpkasdker/localDB/stuff";
            conn = new SqlConnection(sqlconnectionString);
            try
            {
                conn.Open();

                string sqlInsertCommand = $"INSERT INTO Item (Name, ItemGroupID)" +
                                          $"VALUES  ({name}, {itemGroup})";

                SqlCommand cmd = new SqlCommand(sqlInsertCommand) {Connection = conn};

                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null) conn.Close();
            }

        }

        public void DeleteItem(int ID)
        {
            sqlconnectionString = "herpkasdker/localDB/stuff";
            conn = new SqlConnection(sqlconnectionString);
            try
            {
                conn.Open();
                string sqlDeleteCommand = $"DELETE FROM Item" +
                                          $"WHERE ItemId = '{ID}";

                SqlCommand cmd = new SqlCommand(sqlDeleteCommand) {Connection = conn};

                cmd.ExecuteNonQuery();
            }
            finally
            {
                if (conn != null) conn.Close();
            }

        }

        public List<Item> SearchItems(string itemName)
        {
            sqlconnectionString = "herpkasdker/localDB/stuff";
            conn = new SqlConnection(sqlconnectionString);
            List<Item> listOfItems = new List<Item>();
            SqlDataReader reader = null;
            try
            {
                conn.Open();

                string cmdText = $"SELECT * FROM Item" +
                                 $"WHERE Name LIKE '{itemName}'";

                SqlCommand cmd = new SqlCommand(cmdText, conn);

                reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    var localItem = new Item((int)reader["ItemID"], (string)reader["Name"], (int)reader["ItemGroupID"]);
                    listOfItems.Add(localItem);
                }
                return listOfItems;
            }
            finally
            {
                reader?.Close();
                conn?.Close();
            }
        }
    }
}