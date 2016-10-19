using System.Collections.Generic;
using System.Data.SqlClient;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace DatabaseAPI.TableStoreSection
{
    public class SqlTableStoreSection : ITableStoreSection 
    {
        private SqlConnection _conn;
        private SqlCommand _cmd;
        private SqlDataReader _reader;


        public SqlTableStoreSection(string connectionString)
        {
            _conn = new SqlConnection(connectionString);
        }

        public long CreateStoreSection(string storeSectionName, long coordinateX, long coordinateY, long floorPlanID)
        {
            long createdID;
            try
            {
                _conn.Open();

                _cmd =
                    new SqlCommand(
                        $"INSERT INTO StoreSection (Name, CoordinateX, CoordinateY, FloorPlanID) " +
                        $"VALUES ('" + storeSectionName + "', '" + coordinateX + "', '" + coordinateY + "', '" + floorPlanID + "');" +
                        "SELECT CAST(scope_identity() AS BIGINT)",
                        _conn);

                createdID = (long)_cmd.ExecuteScalar();
            }
            finally
            {
                _conn?.Close();
            }
            return createdID;
        }

        public void DeleteStoreSection(long storeSectionID)
        {
            try
            {
                _conn.Open();

                _cmd = new SqlCommand($"DELETE FROM StoreSection WHERE StoreSectionID = '" + storeSectionID + "'",
                    _conn);

                _cmd.ExecuteNonQuery();

            }
            finally
            {
                _conn?.Close();

            }
        }

        public void DeleteAllStoreSections(long floorPlanID)
        {
            try
            {
                _conn.Open();

                _cmd = new SqlCommand($"DELETE FROM StoreSection WHERE FloorPlanID = '" + floorPlanID + "'",
                    _conn);

                _cmd.ExecuteNonQuery();

            }
            finally
            {
                _conn?.Close();

            }
        }

        public List<StoreSection> GetAllStoreSections(long floorPlanID)
        {
            List<StoreSection> allStoreSections = new List<StoreSection>();
            try
            {
                _conn.Open();
                _cmd = new SqlCommand("SELECT * FROM StoreSection WHERE FloorPlanID = '" + floorPlanID + "'", _conn);

                _reader = _cmd.ExecuteReader();


                while (_reader.Read())
                {
                    StoreSection newSection = new StoreSection(0,"",0,0,0);

                    if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                        newSection.Name = (string)_reader["Name"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("StoreSectionID")))
                        newSection.StoreSectionID = (long)_reader["StoreSectionID"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("CoordinateX")))
                        newSection.CoordinateX = (long)_reader["CoordinateX"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("CoordinateY")))
                        newSection.CoordinateY = (long)_reader["CoordinateY"];

                    allStoreSections.Add(newSection);
                }
            }
            finally
            {
                _conn?.Close();
                _reader?.Close();
            }
            return allStoreSections;
        }


        public StoreSection GetStoreSection(long storeSectionID)
        {
            StoreSection storeSectionReturnValue = null;
            try
            {
                _conn.Open();
                _cmd = new SqlCommand("SELECT * FROM StoreSection WHERE StoreSectionID = '" + storeSectionID + "'", _conn);

                _reader = _cmd.ExecuteReader();


                while (_reader.Read())
                {
                    long floorPlanID = 0;
                    string storeSectionName = "";
                    long coordinateX = 0;
                    long coordinateY = 0;

                    if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                        storeSectionName = (string)_reader["Name"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("CoordinateX")))
                        coordinateX = (long)_reader["CoordinateX"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("CoordinateY")))
                        coordinateY = (long)_reader["CoordinateY"];

                    if (!_reader.IsDBNull(_reader.GetOrdinal("FloorPlanID")))
                        floorPlanID = (long)_reader["FloorPlanID"];

                    storeSectionReturnValue = new StoreSection(storeSectionID, storeSectionName, coordinateX, coordinateY, floorPlanID);
                    
                }
            }
            finally
            {
                _conn?.Close();
                _reader?.Close();
            }
            return storeSectionReturnValue;
        }

        public void UpdateStoreSectionCoordinate(long storeSectionID, long coordinateX, long coordinateY)
        {
            try
            {
                _conn.Open();

                _cmd =
                    new SqlCommand(
                        $"UPDATE StoreSection SET  CoordinateX = '"+ coordinateX + "', CoordinateY = '" + coordinateY + "' WHERE StoreSectionID = '"+ storeSectionID +"'",
                        _conn);

                _cmd.ExecuteNonQuery();
            }
            finally
            {
                _conn?.Close();
            }
        }

        public void UpdateStoreSectionName(long storeSectionID, string storeSectionName)
        {
            string query = @"UPDATE StoreSection SET Name = @newSectionName WHERE StoreSectionID = @storeSectionID";

            using (_cmd =  new SqlCommand(query,_conn))
            {
                _conn.Open();
                _cmd.Parameters.AddWithValue("@newSectionName", storeSectionName);
                _cmd.Parameters.AddWithValue("@storeSectionID", storeSectionID);

                _cmd.ExecuteNonQuery();
                _conn.Close();
            }
        }

    }

}

