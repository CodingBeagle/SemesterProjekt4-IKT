using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using mainMenu.Models;
using mainMenu.ViewModels;
using NSubstitute;
using NSubstitute.Core.Arguments;

namespace ProjectGUI.Tests
{
    [TestFixture]
    public class ItemGroupViewModelUnitTests
    {
        private IDatabaseService _db;
        private IStoreDatabaseFactory _dbFactory;
        private IMessageBox _mb;
        private ItemGroupViewModel _uut;

        [SetUp]
        public void SetUp()
        {
            _dbFactory = Substitute.For<IStoreDatabaseFactory>();
            _db = Substitute.For<IDatabaseService>();
            _mb = Substitute.For<IMessageBox>();
            List<ItemGroup> testList = new List<ItemGroup>();
            testList.Add(new ItemGroup("test", 0, 0));
            _db.TableItemGroup.SearchItemGroups(Arg.Any<string>())
                .Returns(testList);
            _uut = new ItemGroupViewModel(_db, _mb);
        }

        //[Test]
        //public void ItemGroupViewModel_Constructor_ComboBoxOptionsIsPopulated()
        //{
            
        //}

        [Test]
        public void ItemGroupViewModel_EditItemGroupCommand_UpdateItemGroupIsCalled()
        {
            _uut.EditItemGroupCommand.Execute(null);
            _uut.PreviousItemGroupName = "Test1";
            _uut.ItemGroupName = "Test";
            _db.Received().TableItemGroup.UpdateItemGroup(_uut.PreviousItemGroupName, _uut.ItemGroupName);
        }


        [Test]
        public void ItemGroupViewModel_EditItemGroupCommand_UpdateItemGroupName_MessageBoxShowsNewItemGroupName()
        {
            _uut.PreviousItemGroupName = "Test1";
            _uut.ItemGroupName = "Test";
            _uut.ListOfItemGroups.Add(new ItemGroup("test1", (long)0, (long)0));
            _uut.ListOfItemGroups.Add(new ItemGroup("test2", (long)0, (long)1));
            _uut.ListOfItemGroups.Add(new ItemGroup("test3", (long)0, (long)2));
            _uut.ListOfItemGroups.CurrentIndex = 1;
            _uut.ItemGroupName = "Test";
            _uut.EditItemGroupCommand.Execute(null);
            _mb.Received(1).OpenMessageBox("Varegruppens navn er blevet opdateret til Test");
        }

        [Test]
        public void ItemGroupViewModel_EditItemGroupCommand_UpdateItemGroupNameWrongly_MessageBoxShowsErrorOccured()
        {
            _uut.PreviousItemGroupName = "Test1";
            _uut.ItemGroupName = "Test";
            _uut.EditItemGroupCommand.Execute(null);
            _mb.Received(1).OpenMessageBox("Noget gik galt! Check debug for fejlmeddelelse");
        }

        [Test]
        public void ItemGroupViewModel_CreateItemGroupCommand_CreateNewItemGroupWithWrongName_MessageBoxShowsNamingError()
        {
            _uut.ItemGroupName = "!";
            _uut.CreateItemGroupCommand.Execute(null);
            _mb.Received(1).OpenMessageBox("Navnet på en vare kan kun indeholde bogstaver og tal");
        }

        [Test]
        public void
            ItemGroupViewModel_CreateItemGroupCommand_CreateNewItemGroupWithAcceptedNameAndNoParent_ItemGroupIsCreated()
        {
            _uut.ItemGroupName = "Test1";
            _uut.CreateItemGroupCommand.Execute(null);
            _db.Received(1).TableItemGroup.CreateItemGroup(_uut.ItemGroupName);
        }

        [Test]
        public void
            ItemGroupViewModel_CreateItemGroupCommand_CreateNewItemGroupWithAcceptedNameAndParent_ItemGroupIsCreated()
        {
            _uut.ItemGroupName = "Test1";
            _uut.ListOfItemGroups.Add(new ItemGroup("test1", (long)0, (long)0));
            _uut.ListOfItemGroups.Add(new ItemGroup("test2", (long)0, (long)1));
            _uut.ListOfItemGroups.Add(new ItemGroup("test3", (long)0, (long)2));
            _uut.ListOfItemGroups.CurrentIndex = 1;
            _uut.CreateItemGroupCommand.Execute(null);
            _db.Received(1)
                .TableItemGroup.CreateItemGroup(_uut.ItemGroupName,
                    _uut.ListOfItemGroups[_uut.ListOfItemGroups.CurrentIndex].ItemGroupID);
        }
        [Test]
        public void ItemGroupViewModel_CreateItemGroupCommand_CreateNewItemWithAcceptedName_MessageBoxShowsConfirmationMsg()
        {
            _uut.ItemGroupName = "Test1";
            _uut.CreateItemGroupCommand.Execute(null);
            _mb.Received(1).OpenMessageBox("Test1 er blevet tilføjet til databasen");
        }

        [Test]
        public void ItemGroupViewModel_CreateItemGroupCommand_CreateNewItemWithGoneWrong_MessageBoxShowsError()
        {
            _uut.ListOfItemGroups = null;
            _uut.ItemGroupName = "Test1";
            _uut.CreateItemGroupCommand.Execute(null);
            _mb.Received(1).OpenMessageBox("Noget gik galt! Check debug for fejlmeddelelse");
        }

        [Test]
        public void ItemGroupViewModel_SearchCommand_SearchPopulatesList()
        {
            _uut.SearchString = "Test";
            _uut.SearchCommand.Execute(null);
            _db.Received(1).TableItemGroup.SearchItemGroups("Test");
        }
    }

    
}
