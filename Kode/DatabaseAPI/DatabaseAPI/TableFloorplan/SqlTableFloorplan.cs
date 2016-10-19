using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI.DatabaseModel;

namespace DatabaseAPI.TableFloorplan
{
    public class SqlTableFloorplan : ITableFloorplan
    {
        private readonly SqlConnection _conn;

        public SqlTableFloorplan(string connectionString)
        {
            _conn = new SqlConnection(connectionString);
        }

        public void UploadFloorplan(string name, int width, int height, string filePath)
        {
            try
            {
                byte[] imageContent = File.ReadAllBytes(filePath);

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

        public void DownloadFloorplan()
        {
            try
            {
                string sqlStatement = @"SELECT Image FROM Floorplan WHERE FloorPlanID = 1";

                _conn.Open();
                using (SqlCommand cmd = new SqlCommand(sqlStatement, _conn))
                {
                    SqlDataReader rd = cmd.ExecuteReader();

                    while (rd.Read())
                    {
                        DownloadImageDataToImageFile("floorplan", rd);
                    }

                    rd.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("Something went horrible wrong when downloading the floorplan: " + e.Message);
            }
            finally
            {
                _conn.Close();
            }
        }

        private void InsertFloorplan(string name, int width, int height, byte[] imageContent)
        {
            try
            {
                string sqlStatement = @"INSERT INTO Floorplan
                                        (FloorPlanID, Name, Image, imageWidth, imageHeight)
                                        VALUES (@id, @name, @image, @width, @height);";

                using (SqlCommand sqlCmd = new SqlCommand(sqlStatement, _conn))
                {
                    sqlCmd.Parameters.AddWithValue("@id", 1);
                    sqlCmd.Parameters.AddWithValue("@name", name);
                    sqlCmd.Parameters.AddWithValue("@width", width);
                    sqlCmd.Parameters.AddWithValue("@height", height);
                    sqlCmd.Parameters.Add("@image", SqlDbType.Image, imageContent.Length).Value = imageContent;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("An error occoured while inserting floorplan to database: " + e.Message);
            }
            finally
            {
                _conn.Close();
            }
        }

        private void UpdateFloorplan(string name, int width, int height, byte[] imageContent)
        {
            try
            {
                string sqlStatement = @"UPDATE Floorplan SET Name=@name, Image=@image, imageWidth=@width, imageHeight=@height
                                     WHERE Floorplan.FloorPlanID = 1";

                _conn.Open();
                using (SqlCommand sqlCmd = new SqlCommand(sqlStatement, _conn))
                {
                    sqlCmd.Parameters.AddWithValue("@name", name);
                    sqlCmd.Parameters.AddWithValue("@width", width);
                    sqlCmd.Parameters.AddWithValue("@height", height);
                    sqlCmd.Parameters.Add("@image", SqlDbType.Image, imageContent.Length).Value = imageContent;

                    sqlCmd.ExecuteNonQuery();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("An error occoured while updating floorplan to database: " + e.Message);
            }
            finally
            {
                _conn.Close();
            }
        }

        private bool DoesFloorplanExist()
        {
            bool floorplanExist = false;

            try
            {
                string checkFloorplanExistanceSql = "SELECT FloorPlanID FROM Floorplan";

                _conn.Open();
                using (SqlCommand sqlCmd = new SqlCommand(checkFloorplanExistanceSql, _conn))
                {
                    SqlDataReader reader = sqlCmd.ExecuteReader();

                    if (reader.HasRows)
                    {
                        floorplanExist = true;
                    }

                    reader.Close();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine("");
            }
            finally
            {
                _conn.Close();
            }

            return floorplanExist;
        }

        private Byte[] DownloadImageDataToImageFile(string floorplanName, SqlDataReader reader)
        {
            FileStream stream;
            BinaryWriter br;
            int bufferSize = 100;
            long startIndex = 0;
            long retval = 0;
            byte[] outbyte = new byte[bufferSize];

            // Create a filestream used to save the floorplan as a local image
            stream = new FileStream(@"../../images/" + floorplanName + ".jpg", FileMode.OpenOrCreate, FileAccess.Write);

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