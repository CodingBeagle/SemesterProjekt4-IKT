using System.Collections.Generic;
using DatabaseAPI.Factories;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.TableItem;
using NUnit.Framework;

namespace DatabaseAPI.StoreDatabase.Integration
{
    [TestFixture]
    public class Step1TableItemIntegration
    {
        private IDatabaseService _db;

        [SetUp]
        public void Setup()
        {   
            _db = new DatabaseService(new SqlStoreDatabaseFactory());

            CleanUp();
        }

        [TearDown]
        public void Teardown()
        {
            CleanUp();
        }

        [Test]
        public void TableItemGroup_CreateItemGroup_ItemGroupExistInDatabase()
        {
            // Creation
            long createdItemGroupID = _db.TableItemGroup.CreateItemGroup("");
        }

        /*
        [Test]
        public void TableItem_CreateItem_ItemExistInDatabase()
        {
            // Creation
            long createdItemID = _db.TableItem.CreateItem("Step1IntegrationTest_Item1", 0);

            // Testing
            Item fetchedItem = _db.TableItem.GetItem(createdItemID);

            Assert.That(createdItemID, Is.EqualTo(fetchedItem.ItemID));
        }
        */

        private void CleanUp()
        {
            // Cleanup item groups


            // Cleanup items
            List<Item> itemsToCleanUp = _db.TableItem.SearchItems("Step1IntegrationTest");
            foreach (var item in itemsToCleanUp)
            {
                _db.TableItem.DeleteItem(item.ItemID);
            }
        }
    }
}
