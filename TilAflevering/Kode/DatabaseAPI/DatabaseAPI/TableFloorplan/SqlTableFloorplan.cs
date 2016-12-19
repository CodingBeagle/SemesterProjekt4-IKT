using System;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableFloorplan
{
    public class SqlTableFloorplan : ITableFloorplan
    {
        private SqlConnection _conn;
        private SqlCommand _cmd;
        private SqlDataReader _reader;
        private string _connString;
        private string _sqlQueryString;

        public SqlTableFloorplan(string connectionString)
        {
            _connString = connectionString;
        }

        public void UploadFloorplan(string name, int width, int height, string filePath)
        {
            try
            {
                byte[] imageContent = new byte[0];

                if (File.Exists(filePath))
                {
                    imageContent = File.ReadAllBytes(filePath);
                }

                bool floorplanExist = DoesFloorplanExist();

                if (!floorplanExist)
                {
                    InsertFloorplan(name, width, height, imageContent);
                }
                else
                {
                    UpdateFloorplan(name, width, height, imageContent);
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("An error occoured while uploading a new floorplan image: " + e.Message);
            }
        }

        public void DownloadFloorplan(string filepath)
        {
            _sqlQueryString = @"SELECT Image FROM Floorplan WHERE FloorPlanID = 1";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);

                try
                {
                    _conn.Open();
                    _reader = _cmd.ExecuteReader();

                    while (_reader.Read())
                    {
                        DownloadImageDataToImageFile("floorplan", filepath, _reader);
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("Something went wrong in DownloadFloorplan: " + e.Message);
                }
            }
        }

        private void InsertFloorplan(string name, int width, int height, byte[] imageContent)
        {
            _sqlQueryString = @"INSERT INTO Floorplan
                                (FloorPlanID, Name, Image, imageWidth, imageHeight)
                                VALUES (@id, @name, @image, @width, @height)";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@id", 1);
                _cmd.Parameters.AddWithValue("@name", name);
                _cmd.Parameters.AddWithValue("@width", width);
                _cmd.Parameters.AddWithValue("@height", height);
                _cmd.Parameters.Add("@image", SqlDbType.Image, imageContent.Length).Value = imageContent;

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("An error occoured while inserting floorplan to database: " + e.Message);
                }
            }
        }

        public Floorplan GetFloorplan()
        {
            _sqlQueryString = @"SELECT * FROM Floorplan WHERE Floorplan.FloorPlanID=1";
            Floorplan floorplan = null;

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);

                try
                {
                    _conn.Open();
                    _reader = _cmd.ExecuteReader();

                    long floorplanID = 0;
                    string floorplanName = "";
                    long width = 0;
                    long height = 0;

                    while (_reader.Read())
                    {
                        floorplanID = (long)_reader["FloorPlanID"];
                        floorplanName = (string)_reader["Name"];
                        width = (long)_reader["imageWidth"];
                        height = (long)_reader["imageHeight"];
                    }

                    floorplan = new Floorplan(floorplanID, floorplanName, width, height);
                }
                catch (Exception e)
                {
                    Debug.WriteLine("An error occoured while getting floorplan from database: " + e.Message);
                }
            }

            return floorplan;
        }

        private void UpdateFloorplan(string name, int width, int height, byte[] imageContent)
        {
            _sqlQueryString = @"UPDATE Floorplan SET Name=@name, Image=@image, imageWidth=@width, imageHeight=@height
                                WHERE Floorplan.FloorPlanID = 1";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);
                _cmd.Parameters.AddWithValue("@name", name);
                _cmd.Parameters.AddWithValue("@width", width);
                _cmd.Parameters.AddWithValue("@height", height);
                _cmd.Parameters.Add("@image", SqlDbType.Image, imageContent.Length).Value = imageContent;

                try
                {
                    _conn.Open();
                    _cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    Debug.WriteLine("An error occoured while updating floorplan to database: " + e.Message);
                }
            }
        }

        private bool DoesFloorplanExist()
        {
            bool floorplanExist = false;
            _sqlQueryString = "SELECT FloorPlanID FROM Floorplan";

            using (_conn = new SqlConnection(_connString))
            {
                _cmd = new SqlCommand(_sqlQueryString, _conn);

                try
                {
                    _conn.Open();
                    _reader = _cmd.ExecuteReader();

                    if (_reader.HasRows)
                    {
                        floorplanExist = true;
                    }
                }
                catch (Exception e)
                {
                    Debug.WriteLine("An error occoured while checking if floorplan exists: " + e.Message);
                }
            }
            return floorplanExist;
        }

        private Byte[] DownloadImageDataToImageFile(string floorplanName, string filepath, SqlDataReader reader)
        {
            FileStream stream;
            BinaryWriter br;
            int bufferSize = 100;
            long startIndex = 0;
            long retval = 0;
            byte[] outbyte = new byte[bufferSize];

            // Create a filestream used to save the floorplan as a local image @"../../images/"
            stream = new FileStream(filepath + floorplanName + ".jpg", FileMode.OpenOrCreate, FileAccess.Write);

            // The binary writer writes primitive types in binary to a stream
            br = new BinaryWriter(stream);

            startIndex = 0;

            // GetBytes reads a stream of bytes from the specified column into the buffer
            retval = reader.GetBytes(0, startIndex, outbyte, 0, bufferSize);

            // As long as we can download in chunks of bufferSize, we do that
            while (retval == bufferSize)
            {
                // Write the retrieved chunk of bytes to the image file
                br.Write(outbyte);

                // Clear all buffers for the writer, so that we are ready for the next chunk of bytes
                br.Flush();

                startIndex += bufferSize;
                retval = reader.GetBytes(0, startIndex, outbyte, 0, bufferSize);
            }

            // Write the remaining bytes to the image
            br.Write(outbyte, 0, (int)retval);
            br.Flush();

            br.Close();
            stream.Close();

            return outbyte;
        }
    }
}