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

        public long CreateStoreSection(string storeSectionName, long coordinateX, long coordinateY, long floorPlanID)
        {
            long createdID;
            try
            {
                _connection.Open();

                _command =
                    new SqlCommand(
                        $"INSERT INTO StoreSection (Name, CoordinateX, CoordinateY, FloorPlanID) " +
                        $"VALUES ('" + storeSectionName + "', '" + coordinateX + "', '" + coordinateY + "', '" + floorPlanID + "');" +
                        "SELECT CAST(scope_identity() AS BIGINT)",
                        _connection);

                createdID = (long)_command.ExecuteScalar();
            }
            finally
            {
                _connection?.Close();
            }
            return createdID;
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

        public List<StoreSection> GetAllStoreSections(long floorPlanID)
        {
            List<StoreSection> allStoreSections = new List<StoreSection>();
            try
            {
                _connection.Open();
                _command = new SqlCommand("SELECT * FROM StoreSection WHERE FloorPlanID = '" + floorPlanID + "'", _connection);

                _dataReader = _command.ExecuteReader();


                while (_dataReader.Read())
                {
                    long storeSectionID = 0;
                    string storeSectionName = "";
                    long coordinateX = 0;
                    long coordinateY = 0;

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("Name")))
                        storeSectionName = (string)_dataReader["Name"];

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("StoreSectionID")))
                        storeSectionID = (long)_dataReader["StoreSectionID"];

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("CoordinateX")))
                        coordinateX = (long)_dataReader["CoordinateX"];

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("CoordinateY")))
                        coordinateY = (long)_dataReader["CoordinateY"];

                    var newSection = new StoreSection(storeSectionID, storeSectionName, coordinateX, coordinateY, floorPlanID);
                    allStoreSections.Add(newSection);
                }
            }
            finally
            {
                _connection?.Close();
                _dataReader?.Close();
            }
            return allStoreSections;
        }


        public StoreSection GetStoreSection(long storeSectionID)
        {
            StoreSection storeSectionReturnValue = null;
            try
            {
                _connection.Open();
                _command = new SqlCommand("SELECT * FROM StoreSection WHERE StoreSectionID = '" + storeSectionID + "'", _connection);

                _dataReader = _command.ExecuteReader();


                while (_dataReader.Read())
                {
                    long floorPlanID = 0;
                    string storeSectionName = "";
                    long coordinateX = 0;
                    long coordinateY = 0;

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("Name")))
                        storeSectionName = (string)_dataReader["Name"];

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("CoordinateX")))
                        coordinateX = (long)_dataReader["CoordinateX"];

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("CoordinateY")))
                        coordinateY = (long)_dataReader["CoordinateY"];

                    if (!_dataReader.IsDBNull(_dataReader.GetOrdinal("FloorPlanID")))
                        floorPlanID = (long)_dataReader["FloorPlanID"];

                    storeSectionReturnValue = new StoreSection(storeSectionID, storeSectionName, coordinateX, coordinateY, floorPlanID);
                    
                }
            }
            finally
            {
                _connection?.Close();
                _dataReader?.Close();
            }
            return storeSectionReturnValue;
        }

        public void UpdateStoreSection(long storeSectionID, string storeSectionName, long coordinateX, long coordinateY, long floorPlanID)
        {
            try
            {
                _connection.Open();

                _command =
                    new SqlCommand(
                        $"UPDATE StoreSection SET Name = '"+ storeSectionName +"', CoordinateX = '"+ coordinateX + "', CoordinateY = '" + coordinateY + "', FloorPlanID = '" + floorPlanID + "' WHERE StoreSectionID = '"+ storeSectionID +"'",
                        _connection);

                _command.ExecuteNonQuery();
            }
            finally
            {
                _connection?.Close();
            }
        }

    }

}

