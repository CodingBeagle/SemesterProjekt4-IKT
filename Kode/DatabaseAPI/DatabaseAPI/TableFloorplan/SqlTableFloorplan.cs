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

                string query = @"SELECT Image FROM Floorplan WHERE FloorPlanID = @id";
                
                SqlCommand sqlCmd = new SqlCommand(query, _conn);
                sqlCmd.Parameters.AddWithValue("@id", id);

                SqlDataReader reader = sqlCmd.ExecuteReader(CommandBehavior.SequentialAccess);

                // Setup
                FileStream stream;
                BinaryWriter br;
                long startIndex = 0;
                long retval = 0;
                byte[] outbyte = new byte[100];

                while (reader.Read())
                {
                    stream = new FileStream("DownloadTests/Test.jpg", FileMode.OpenOrCreate, FileAccess.Write);
                    br = new BinaryWriter(stream);

                    startIndex = 0;

                    retval = reader.GetBytes(1, startIndex, outbyte, 0, 100);

                    while (retval == 100)
                    {
                        
                    }
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

            return null;
        }
    }
}
