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
        public void TableItem_CreateItem_ItemExistInDatabase()
        {
            // Creation setup
            long testItemGroup = _db.TableItemGroup.CreateItemGroup("Step1IntegrationTest_ItemGroup");
            long createdItemID = _db.TableItem.CreateItem("Step1IntegrationTest_Item1", testItemGroup);

            // Testing
            Item fetchedItem = _db.TableItem.GetItem(createdItemID);

            Assert.That(createdItemID, Is.EqualTo(fetchedItem.ItemID));
        }

        [Test]
        public void TableItem_GetItem_ItemExist()
        {
            // Creation setup
            long testItemGroupID = _db.TableItemGroup.CreateItemGroup("Step1IntegrationTest_ItemGroup");
            long testItemID = _db.TableItem.CreateItem("Step1IntegrationTest_Item1", testItemGroupID);

            // Testing
            Item fetchItem = _db.TableItem.GetItem(testItemID);

            Assert.That(fetchItem, Is.Not.Null);
        }

        [Test]
        public void TableItem_GetItem_ItemDoesNotExist()
        {
            // Creation setup
            long testItemGroupID = _db.TableItemGroup.CreateItemGroup("Step1IntegrationTest_ItemGroup");
            long testItemID = _db.TableItem.CreateItem("Step1IntegrationTest_Item1", testItemGroupID);
            _db.TableItem.DeleteItem(testItemID);

            // Testing
            Item fetchItem = _db.TableItem.GetItem(testItemID);

            Assert.That(fetchItem, Is.Null);
        }

        [Test]
        public void TableItem_DeleteItem_ItemDoesNotExist()
        {
            // Creation setup
            long testItemGroupID = _db.TableItemGroup.CreateItemGroup("Step1IntegrationTest_ItemGroup");
            long testItemID = _db.TableItem.CreateItem("Step1IntegrationTest_Item1", testItemGroupID);

            // Testing
            _db.TableItem.DeleteItem(testItemID);
            Item fetchedItem = _db.TableItem.GetItem(testItemID);

            Assert.That(fetchedItem, Is.Null);
        }

        [Test]
        public void TableItem_SearchItems_Returns5Items()
        {
            // Creation setup
            long testItemGroupID = _db.TableItemGroup.CreateItemGroup("Step1IntegrationTest_ItemGroup");
            long testItem1 = _db.TableItem.CreateItem("Step1IntegrationTest_Item1", testItemGroupID);
            long testItem2 = _db.TableItem.CreateItem("Step1IntegrationTest_Item2", testItemGroupID);
            long testItem3 = _db.TableItem.CreateItem("Step1IntegrationTest_Item3", testItemGroupID);
            long testItem4 = _db.TableItem.CreateItem("Step1IntegrationTest_Item4", testItemGroupID);
            long testItem5 = _db.TableItem.CreateItem("Step1IntegrationTest_Item5", testItemGroupID);

            // Testing
            List<Item> searchedItems = _db.TableItem.SearchItems("Step1IntegrationTest");
           
            Assert.That(searchedItems.Count, Is.EqualTo(5));
        }

        [Test]
        public void TableItem_SearchItems_Returns3Items()
        {
            // Creation setup
            long testItemGroupID = _db.TableItemGroup.CreateItemGroup("Step1IntegrationTest_ItemGroup");
            long testItem1 = _db.TableItem.CreateItem("Step1IntegrationTest_Item1", testItemGroupID);
            long testItem2 = _db.TableItem.CreateItem("Step1IntegrationTest_Item2", testItemGroupID);
            long testItem3 = _db.TableItem.CreateItem("Step1IntegrationTest_Item3", testItemGroupID);

            // Testing
            List<Item> searchedItems = _db.TableItem.SearchItems("Step1IntegrationTest");

            Assert.That(searchedItems.Count, Is.EqualTo(3));
        }

        private void CleanUp()
        {
            // Cleanup items
            List<Item> itemsToCleanUp = _db.TableItem.SearchItems("Step1IntegrationTest");
            foreach (var item in itemsToCleanUp)
            {
                _db.TableItem.DeleteItem(item.ItemID);
            }

            // Cleanup item groups
            List<ItemGroup> itemGroupsToCleanup = _db.TableItemGroup.SearchItemGroups("Step1IntegrationTest");
            foreach (var itemGroup in itemGroupsToCleanup)
            {
                _db.TableItemGroup.DeleteItemGroup(itemGroup.ItemGroupID);
            }
        }
    }
}
