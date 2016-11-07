using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using NUnit.Framework;

namespace DatabaseAPI.StoreDatabase.Integration
{
    [TestFixture]
    class Step3TableStoreSectionIntegration
    {
        private IDatabaseService _db;
        private long _createdStoreSectionID1 = 0;
        private long _createdStoreSectionID2 = 0;
        private long _createdStoreSectionID3 = 0;
        private int _floorplanID = 1;



        [SetUp]
        public void SetUp()
        {
            _db = new DatabaseService(new SqlStoreDatabaseFactory());

            CleanUp();
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }

        [Test]
        public void TableStoreSection_CreateStoreSectionGetStoreSection_StoreSectionExistsInDatabase()
        {
            _createdStoreSectionID1 = _db.TableStoreSection.CreateStoreSection("Step3Integration_TestSection1", 20, 20, _floorplanID);

            StoreSection fetchedStoreSection = _db.TableStoreSection.GetStoreSection(_createdStoreSectionID1);

            Assert.That(fetchedStoreSection.StoreSectionID, Is.EqualTo(_createdStoreSectionID1));
        }

        [Test]
        public void TableStoreSection_DeleteStoreSection_StoreSectionDoesNotExist()
        {
            _createdStoreSectionID1 = _db.TableStoreSection.CreateStoreSection("Step3Integration_TestSection1", 20, 20, _floorplanID);

            _db.TableStoreSection.DeleteStoreSection(_createdStoreSectionID1);

            StoreSection fetchedStoreSection = _db.TableStoreSection.GetStoreSection(_createdStoreSectionID1);

            Assert.That(fetchedStoreSection, Is.Null);
        }

        [Test]
        public void TableStoreSection_UpdateStoreSectionName_StoreSectionNameIsChanged()
        {
            string newSectionName = "Step3Integration_NewSectionName";
            _createdStoreSectionID1 = _db.TableStoreSection.CreateStoreSection("Step3Integration_OldSectionName", 20, 20, _floorplanID);

            _db.TableStoreSection.UpdateStoreSectionName(_createdStoreSectionID1, newSectionName);

            StoreSection fetchedStoreSection = _db.TableStoreSection.GetStoreSection(_createdStoreSectionID1);

            Assert.That(fetchedStoreSection.Name, Is.EqualTo(newSectionName));
        }

        [Test]
        public void TableStoreSection_DeleteAllStoreSections_NoStoreSectionOnDatabase()
        {
            _db.TableStoreSection.DeleteAllStoreSections(1);

            List<StoreSection> fetchedStoreSections = _db.TableStoreSection.GetAllStoreSections(_floorplanID);

            Assert.That(fetchedStoreSections.Count,Is.EqualTo(0));
        }

        [Test]
        public void TableStoreSection_GetAllStoreSections_ReturnsCorrectCount()
        {
            _db.TableStoreSection.DeleteAllStoreSections(_floorplanID);

            _createdStoreSectionID1 = _db.TableStoreSection.CreateStoreSection("Step3Integration_TestSection1", 20, 20, _floorplanID);
            _createdStoreSectionID2 = _db.TableStoreSection.CreateStoreSection("Step3Integration_TestSection2", 20, 20, _floorplanID);
            _createdStoreSectionID3 = _db.TableStoreSection.CreateStoreSection("Step3Integration_TestSection3", 20, 20, _floorplanID);

            List<StoreSection> fetchedStoreSections = _db.TableStoreSection.GetAllStoreSections(_floorplanID);

            Assert.That(fetchedStoreSections.Count,Is.EqualTo(3));

        }



        private void CleanUp()
        {
            if (_createdStoreSectionID1 != 0)
            {
                _db.TableStoreSection.DeleteStoreSection(_createdStoreSectionID1);
            }

            if (_createdStoreSectionID2 != 0)
            {
                _db.TableStoreSection.DeleteStoreSection(_createdStoreSectionID2);
            }

            if (_createdStoreSectionID3 != 0)
            {
                _db.TableStoreSection.DeleteStoreSection(_createdStoreSectionID3);
            }

        }
    }
}
