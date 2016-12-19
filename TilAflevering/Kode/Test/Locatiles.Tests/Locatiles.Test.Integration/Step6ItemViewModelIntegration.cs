
using System.Collections.Generic;
using System.Linq;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using mainMenu.Models;
using mainMenu.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace Locatiles.Test.Integration
{
    [TestFixture]
    public class Step6ItemViewModelIntegration
    {
        private ItemViewModel _iut;
        private IDatabaseService _db;
        private IMessageBox _messageBox;

        private long testItemID1;
        private long testItemGroup;

        [SetUp]
        public void SetUp()
        {
            _db = new DatabaseService(new SqlStoreDatabaseFactory());

            CleanUp();
            InitialSetup();

            _messageBox = Substitute.For<IMessageBox>();
            _iut = new ItemViewModel(_db, _messageBox);
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }

        [Test]
        public void ItemViewModel_CreateItemCommand_CorrectName_ExpectItemInDatabase()
        {
            // Setup
            _iut.ItemName = "Step6IntegrationTestItem1";
            List<ItemGroup> itemGroupsInDatabase = _iut.ItemGroupComboBoxList;
            int index = itemGroupsInDatabase.FindIndex(o => o.ItemGroupName == "Step6IntegrationTest_ItemGroup");
            _iut.ComboBoxIndex = index;
             _iut.CreateItemCommand.Execute(null);

            // Test
            List<Item> fetchedItems = _db.TableItem.SearchItems("Step6IntegrationTestItem1");

            Assert.That(fetchedItems.Count, Is.EqualTo(1));
        }

        [Test]
        public void ItemViewModel_CreateItemCommand_InvalidName_ExpectItemNotInDatabase()
        {
            // Setup
            _iut.ItemName = "Step6IntegrationTest_Item2";
            List<ItemGroup> itemGroupsInDatabase = _iut.ItemGroupComboBoxList;
            int index = itemGroupsInDatabase.FindIndex(o => o.ItemGroupName == "Step6IntegrationTest_ItemGroup");
            _iut.ComboBoxIndex = index;
            _iut.CreateItemCommand.Execute(null);

            // Test
            List<Item> fetchedItems = _db.TableItem.SearchItems("Step6IntegrationTest_Item2");

            Assert.That(fetchedItems.Count, Is.EqualTo(0));
        }

        [Test]
        public void ItemViewModel_DeleteItemCommand_ExpectItemNotInDatabase()
        {
            List<DisplayItem> itemsInDatabase = _iut.ListOfItems.ToList();
            int testItemIndex = itemsInDatabase.FindIndex(o => o.ID == testItemID1);
            _iut.ListOfItems.CurrentIndex = testItemIndex;

            _iut.DeleteItemCommand.Execute(null);

            // Test
            List<Item> fetchedItems = _db.TableItem.SearchItems("Step6IntegrationTest_Item3");

            Assert.That(fetchedItems, Is.Empty);
        }

        [Test]
        public void ItemViewModel_SearchItemCommand_ExpectItemInDatabase()
        {
            string searchString = "Step6IntegrationTest_Item3";
            _iut.SearchString = searchString;

            _iut.SearchItemCommand.Execute(null);

            List<DisplayItem> searchedItems = _iut.ListOfItems.ToList();

            Assert.That(searchedItems.Count, Is.EqualTo(1));
        }

        [Test]
        public void ItemViewModel_SearchItemCommand_ExpectItemNotInDatabase()
        {
            string searchString = "Step6IntegrationTest_NonExistentItem";
            _iut.SearchString = searchString;

            _iut.SearchItemCommand.Execute(null);

            List<DisplayItem> searchedItems = _iut.ListOfItems.ToList();

            Assert.That(searchedItems, Is.Empty);
        }

        private void CleanUp()
        {
            List<Item> itemsToDelete = _db.TableItem.SearchItems("Step6IntegrationTest");

            foreach (Item item in itemsToDelete)
            {
                _db.TableItem.DeleteItem(item.ItemID);
            }


            List<ItemGroup> itemGroupsToDelete = _db.TableItemGroup.SearchItemGroups("Step6IntegrationTest_ItemGroup");

            foreach (ItemGroup itemGroup in itemGroupsToDelete)
            {
                _db.TableItemGroup.DeleteItemGroup(itemGroup.ItemGroupID);
            }


        }

        private void InitialSetup()
        {
            testItemGroup = _db.TableItemGroup.CreateItemGroup("Step6IntegrationTest_ItemGroup");

            testItemID1 = _db.TableItem.CreateItem("Step6IntegrationTest_Item3", testItemGroup);
        }
    }
}
