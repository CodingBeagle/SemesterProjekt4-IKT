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

                _conn.Open();

                string sqlStatement = @"INSERT INTO Floorplan
                                        (Name, Image, imageWidth, imageHeight)
                                        VALUES (@name, @image, @width, @height)";

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
                Debug.WriteLine("Something went wrong: " + e.Message);
            }
            finally
            {
                _conn.Close();
            }
        }

        public Floorplan GetFloorplan(int id)
        {
            try
            {
                _conn.Open();

                string query = @"SELECT Name, Image FROM Floorplan WHERE FloorPlanID = @id";
                
                SqlCommand sqlCmd = new SqlCommand(query, _conn);
                sqlCmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = sqlCmd.ExecuteReader(CommandBehavior.SequentialAccess);

                // Setup
                FileStream stream;
                BinaryWriter br;
                int bufferSize = 100;
                long startIndex = 0;
                long retval = 0;
                byte[] outbyte = new byte[bufferSize];
                string floorplanName = "";

                while (reader.Read())
                {
                    floorplanName = reader.GetString(0);

                    // Create a filestream used to save the floorplan as a local image
                    stream = new FileStream("Floorplan/" + floorplanName + ".jpg", FileMode.OpenOrCreate, FileAccess.Write);

                    // The binary writer writes primitive types in binary to a stream
                    br = new BinaryWriter(stream);

                    startIndex = 0;

                    // GetBytes reads a stream of bytes from the specified column into the buffer
                    retval = reader.GetBytes(1, startIndex, outbyte, 0, bufferSize);

                    // As long as we can download in chunks of bufferSize, we do that
                    while (retval == bufferSize)
                    {
                        // Write the retrieved chunk of bytes to the image file
                        br.Write(outbyte);

                        // Clear all buffers for the writer, so that we are ready for the next chunk of bytes
                        br.Flush();

                        startIndex += bufferSize;
                        retval = reader.GetBytes(1, startIndex, outbyte, 0, bufferSize);
                    }

                    // Write the remaining bytes to the image
                    br.Write(outbyte, 0, (int)retval);
                    br.Flush();

                    br.Close();
                    stream.Close();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Something went wrong: " + e.Message);
            }
            finally
            {
                _conn.Close();
            }

            return null;
        }
    }
}
