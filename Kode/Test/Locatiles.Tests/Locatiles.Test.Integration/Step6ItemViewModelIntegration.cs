
using System.Collections.Generic;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using mainMenu.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace Locatiles.Test.Integration
{
    [TestFixture]
    public class Step6ItemViewModelIntegration
    {
        private ItemViewModel _uut;
        private IDatabaseService _db;
        private IMessageBox _messageBox;

        [SetUp]
        public void SetUp()
        {
            _db = new DatabaseService(new SqlStoreDatabaseFactory());

            CleanUp();
            InitialSetup();

            _messageBox = Substitute.For<IMessageBox>();
            _uut = new ItemViewModel(_db, _messageBox);
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
            _uut.ItemName = "Step6IntegrationTestItem";
            List<ItemGroup> itemGroupsInDatabase = _uut.ItemGroupComboBoxList;
            int index = itemGroupsInDatabase.FindIndex(o => o.ItemGroupName == "Step6IntegrationTest_ItemGroup");
            _uut.ComboBoxIndex = index;
             _uut.CreateItemCommand.Execute(null);

            // Test
            List<Item> fetchedItems = _db.TableItem.SearchItems("Step6IntegrationTestItem");

            Assert.That(fetchedItems.Count, Is.EqualTo(1));
        }

        [Test]
        public void ItemViewModel_CreateItemCommand_InvalidName_ExpectItemNotInDatabase()
        {
            // Setup
            _uut.ItemName = "Step6IntegrationTest_Item";
            List<ItemGroup> itemGroupsInDatabase = _uut.ItemGroupComboBoxList;
            int index = itemGroupsInDatabase.FindIndex(o => o.ItemGroupName == "Step6IntegrationTest_ItemGroup");
            _uut.ComboBoxIndex = index;
            _uut.CreateItemCommand.Execute(null);

            // Test
            List<Item> fetchedItems = _db.TableItem.SearchItems("Step6IntegrationTest_Item");

            Assert.That(fetchedItems.Count, Is.EqualTo(0));
        }

        private void CleanUp()
        {
            List<ItemGroup> itemGroupsToDelete = _db.TableItemGroup.SearchItemGroups("Step6IntegrationTest_ItemGroup");

            foreach (ItemGroup itemGroup in itemGroupsToDelete)
            {
                _db.TableItemGroup.DeleteItemGroup(itemGroup.ItemGroupID);
            }
        }

        private void InitialSetup()
        {
            _db.TableItemGroup.CreateItemGroup("Step6IntegrationTest_ItemGroup");
        }
    }
}
