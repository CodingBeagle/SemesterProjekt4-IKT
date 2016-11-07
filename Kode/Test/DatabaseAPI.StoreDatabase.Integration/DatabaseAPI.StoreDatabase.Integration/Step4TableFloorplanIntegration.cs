using System.Diagnostics;
using System.IO;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace DatabaseAPI.StoreDatabase.Integration
{
    [TestFixture]
    public class Step4TableFloorplanIntegration
    {
        private IDatabaseService _db;

        [SetUp]
        public void SetUp()
        {
            _db = new DatabaseService(new SqlStoreDatabaseFactory());
        }

        [TearDown]
        public void TearDown()
        {
            if (File.Exists("floorplan.jpg"))
            {
                File.Delete("floorplan.jpg");
            }
        }

        [Test]
        public void DownloadFloorplan_FileExist()
        {
            _db.TableFloorplan.DownloadFloorplan("");

            Assert.That(File.Exists("floorplan.jpg"), Is.True);
        }

        [Test]
        public void UploadFloorplan_GetFloorplan_FloorplanDataCorrect()
        {
            _db.TableFloorplan.UploadFloorplan("Step4TableFloorplan_Floorplan", 15, 15, "");

            Floorplan testFloorplan = _db.TableFloorplan.GetFloorplan();

            Assert.That(testFloorplan.Name, Is.EqualTo("Step4TableFloorplan_Floorplan"));
            Assert.That(testFloorplan.Width, Is.EqualTo(15));
            Assert.That(testFloorplan.Height, Is.EqualTo(15));
            Assert.That(testFloorplan.FloorplanID, Is.EqualTo(1));
        }
    }
}
