using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using DatabaseAPI.DatabaseModel;
using System.Threading.Tasks;
using DatabaseAPI.Factories;

namespace DatabaseAPI.TableStoreSection
{
    public class SqlTableStoreSection : ITableStoreSection 
    {
        private SqlConnection _connection;
        private SqlCommand _command;
        private SqlDataReader _dataReader;


        public SqlTableStoreSection(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public void CreateStoreSection(string storeSectionName, long coordinateX, long coordinateY, long floorPlanID)
        {
            try
            {
                _connection.Open();

                _command =
                    new SqlCommand(
                        $"INSERT INTO StoreSection (Name, CoordinateX, CoordinateY, FloorPlanID) VALUES ('" + storeSectionName + "', '" + coordinateX + "', '" + coordinateY + "', '" + floorPlanID + "')",
                        _connection);

                _command.ExecuteNonQuery();
            }
            finally
            {
                _connection?.Close();
            }
        }

        public void DeleteStoreSection(long storeSectionID)
        {
            try
            {
                _connection.Open();

                _command = new SqlCommand($"DELETE FROM StoreSection WHERE StoreSectionID = '" + storeSectionID + "'",
                    _connection);

                _command.ExecuteNonQuery();

            }
            finally
            {
                _connection?.Close();

            }
        }

        public void DeleteAllStoreSections(long floorPlanID)
        {
            try
            {
                _connection.Open();

                _command = new SqlCommand($"DELETE FROM StoreSection WHERE FloorPlanID = '" + floorPlanID + "'",
                    _connection);

                _command.ExecuteNonQuery();

            }
            finally
            {
                _connection?.Close();

            }
        }

        public StoreSection GetStoreSection(long storeSectionID)
        {
        }

    }
}
