using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace DatabaseAPI.TableStoreSection
{
    public class SqlTableStoreSection : ITableStoreSection
    {
        private SqlConnection _conn;
        private SqlCommand _cmd;
        private SqlDataReader _reader;
        private string _connString;
        private string _sqlQueryString;


        public SqlTableStoreSection(string connectionString)
        {
            _connString = connectionString;
        }

        public long CreateStoreSection(string storeSectionName, long coordinateX, long coordinateY, long floorPlanID)
        {
            long createdID = 0;
            _sqlQueryString = @"INSERT INTO StoreSection (Name, CoordinateX, CoordinateY, FloorPlanID) " +
                              @"VALUES (@storeSectionName, @coordinateX, @coordinateY, @floorPlanID)" +
                              @"SELECT CAST(scope_identity() AS BIGINT)";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@storeSectionName", storeSectionName);
                _cmd.Parameters.AddWithValue("@coordinateX", coordinateX);
                _cmd.Parameters.AddWithValue("@coordinateY", coordinateY);
                _cmd.Parameters.AddWithValue("@floorPlanID", floorPlanID);

                try
                {
                    _conn.Open();
                    createdID = (long) _cmd.ExecuteScalar();

                }
                catch (Exception e)
                {

                    Debug.WriteLine("Something went wrong in CreateStoreSection: " + e.Message);
                }
            }
            return createdID;
        }

        public void DeleteStoreSection(long storeSectionID)
        {
            _sqlQueryString = @"DELETE FROM StoreSection WHERE StoreSectionID = @storeSectionID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@storeSectionID", storeSectionID);

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in DeleteStoreSection: " + e.Message);
                }
            }

        }

        public void DeleteAllStoreSections(long floorPlanID)
        {
            _sqlQueryString = @"DELETE FROM StoreSection WHERE FloorPlanID = @floorPlanID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@floorPlanID", floorPlanID);

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in DeleteAllStoreSections: " + e.Message);
                }
            }

        }

        public List<StoreSection> GetAllStoreSections(long floorPlanID)
        {
            List<StoreSection> allStoreSections = new List<StoreSection>();

            _sqlQueryString = @"SELECT* FROM StoreSection WHERE FloorPlanID = @floorPlanID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@floorPlanID", floorPlanID);

                try
                {
                    _conn.Open();
                    _reader = _cmd.ExecuteReader();

                    while (_reader.Read())
                    {
                        StoreSection newSection = new StoreSection(0, "", 0, 0, 0);

                        if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                            newSection.Name = (string) _reader["Name"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("StoreSectionID")))
                            newSection.StoreSectionID = (long) _reader["StoreSectionID"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("CoordinateX")))
                            newSection.CoordinateX = (long) _reader["CoordinateX"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("CoordinateY")))
                            newSection.CoordinateY = (long) _reader["CoordinateY"];

                        allStoreSections.Add(newSection);
                    }
                }
                catch (Exception e)
                {

                    Debug.WriteLine("Something went wrong in GetAllStoreSections: " + e.Message);
                }
            }
            return allStoreSections;
        }


        public StoreSection GetStoreSection(long storeSectionID)
        {
            StoreSection storeSectionReturnValue = null;

            _sqlQueryString = @"SELECT * FROM StoreSection WHERE StoreSectionID = @storeSectionID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@storeSectionID", storeSectionID);

                try
                {
                    _conn.Open();
                    _reader = _cmd.ExecuteReader();

                    while (_reader.Read())
                    {
                        long floorPlanID = 0;
                        string storeSectionName = "";
                        long coordinateX = 0;
                        long coordinateY = 0;

                        if (!_reader.IsDBNull(_reader.GetOrdinal("Name")))
                            storeSectionName = (string) _reader["Name"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("CoordinateX")))
                            coordinateX = (long) _reader["CoordinateX"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("CoordinateY")))
                            coordinateY = (long) _reader["CoordinateY"];

                        if (!_reader.IsDBNull(_reader.GetOrdinal("FloorPlanID")))
                            floorPlanID = (long) _reader["FloorPlanID"];

                        storeSectionReturnValue = new StoreSection(storeSectionID, storeSectionName, coordinateX,
                            coordinateY, floorPlanID);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in GetStoreSection: " + e.Message);
                }

            }
            return storeSectionReturnValue;
        }

        public void UpdateStoreSectionCoordinate(long storeSectionID, long coordinateX, long coordinateY)
        {
            _sqlQueryString = @"UPDATE StoreSection SET  CoordinateX = @coordinateX, CoordinateY = @coordinateY 
                              WHERE StoreSectionID = @storeSectionID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@coordinateX", coordinateX);
                _cmd.Parameters.AddWithValue("@coordinateY", coordinateY);
                _cmd.Parameters.AddWithValue("@storeSectionID", storeSectionID);

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in UpdateStoreSectionCoordinate: " + e.Message);
                }
            }

        }

        public void UpdateStoreSectionName(long storeSectionID, string storeSectionName)
        {
            _sqlQueryString = @"UPDATE StoreSection SET Name = @newSectionName 
                              WHERE StoreSectionID = @storeSectionID";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@newSectionName", storeSectionName);
                _cmd.Parameters.AddWithValue("@storeSectionID", storeSectionID);

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();

                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in UpdateStoreSectionName: " + e.Message);
                }
            }
        }

    }

}

