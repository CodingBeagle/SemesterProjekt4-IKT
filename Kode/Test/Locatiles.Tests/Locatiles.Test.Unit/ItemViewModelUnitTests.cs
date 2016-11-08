using System.Collections.Generic;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using mainMenu.Models;
using mainMenu.ViewModels;
using NSubstitute;
using NUnit.Framework;


namespace Locatiles.Test.Unit
{
    [TestFixture]
    public class ItemViewModelUnitTests
    {
        private IStoreDatabaseFactory _databaseFactory;
        private IDatabaseService _db;
        private IMessageBox _mb;
        private ItemViewModel _uut;
        

        [SetUp]
        public void SetUp()
        {
            _db = Substitute.For<IDatabaseService>();
            _databaseFactory = Substitute.For<IStoreDatabaseFactory>();
            _mb = Substitute.For<IMessageBox>();

            _db.TableItem.SearchItems(Arg.Any<string>()).Returns(new List<Item>());
            
            _uut = new ItemViewModel(_db, _mb);
        }

        [Test]
        public void ItemViewModel_CreateItemCommand_CreateNewItemWithAcceptedName_ItemIsCreated()
        {
            _uut.ItemName = "test";
            _db.TableItemGroup.GetAllItemGroups().Returns(new List<ItemGroup>() {new ItemGroup("test", 0 , 1)});
            _uut.ComboBoxIndex = 0;
            _uut.CreateItemCommand.Execute(null);
            _db.TableItem.Received(1).CreateItem("test", Arg.Any<long>());
        }

        
        [Test]
        public void ItemViewModel_CreateItemCommand_CreateNewItemWithAcceptedName_MessageBoxShowsItemCreated()
        {
            _uut.ItemName = "test";
            _uut.CreateItemCommand.Execute(null);
            _mb.Received(1).OpenMessageBox(Arg.Any<string>());
        }

        [Test]
        public void ItemViewModel_CreateItemCommand_CreateNewItemWithWrongName_MessageBoxShowsWrongName()
        {
            _uut.ItemName = "!";
            _uut.CreateItemCommand.Execute(null);
            _mb.Received(1).OpenMessageBox("Navnet på en vare kan kun indeholde bogstaver og tal") ;
        }

        [Test]
        public void ItemViewModel_DeleteItemCommand_DeleteItemIsCalled_ItemDeletedFromDB()
        {
            _uut.ListOfItems = new DisplayItems();
            _uut.ListOfItems.CurrentIndex = 0;
            _uut.ListOfItems.Add(new DisplayItem(new Item(1,"test",0)));
            _uut.DeleteItemCommand.Execute(null);
            _db.TableItem.Received(1).DeleteItem(Arg.Any<long>());
        }

        [Test]
        public void ItemViewModel_DeleteItemCommand_DeleteItemWithItem_MessageBoxShowsItemDeleted()
        {
            _uut.ListOfItems = new DisplayItems();
            _uut.ListOfItems.CurrentIndex = 0;
            _uut.ListOfItems.Add(new DisplayItem(new Item(1, "test", 0)));
            _uut.DeleteItemCommand.Execute(null);
            _mb.Received(1).OpenMessageBox(Arg.Any<string>());
        }

        [Test]
        public void ItemViewModel_DeleteItemCommand_DeleteItemWithoutItem_MessageBoxShowsError()
        {
            _uut.DeleteItemCommand.Execute(null);
            _mb.Received(1).OpenMessageBox(Arg.Any<string>());
        }
       
    }
}
