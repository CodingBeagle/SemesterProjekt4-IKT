using System.Collections.Generic;
using System.Linq;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using mainMenu.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace Locatiles.Test.Integration
{
    [TestFixture]
    class Step7ItemGroupViewModelIntegrationTest
    {
        private ItemGroupViewModel _iut;
        private IDatabaseService _db;
        private IMessageBox _mb;


        
        [SetUp]
        public void Setup()
        {
            Cleanup();
            _mb = Substitute.For<IMessageBox>();
            _db = new DatabaseService(new SqlStoreDatabaseFactory());
            
        }

        [TearDown]
        public void Teardown()
        {
            Cleanup();
        }

        private void Cleanup()
        {
            _db = new DatabaseService( new SqlStoreDatabaseFactory());
            var shitlist = _db.TableItemGroup.SearchItemGroups("Step7");
            foreach (var itemGroup in shitlist)
            {
                _db.TableItemGroup.DeleteItemGroup(itemGroup.ItemGroupID);
            }
        }

        [Test]
        public void ItemGroupViewModel_CreateItemGroupCommand_ValidNameAndNoParent_ItemGroupIsCreated()
        {
            _iut = new ItemGroupViewModel(_db, _mb);
            _iut.ItemGroupName = "Step7NewItemGroup";
            _iut.CreateItemGroupCommand.Execute(null);
           Assert.That(_db.TableItemGroup.SearchItemGroups("Step7NewItemGroup").Count, Is.EqualTo(1));
        }

        [Test]
        public void ItemGroupViewModel_CreateItemGroupCommand_ValidNameAndParent_ItemGroupIsCreated()
        {
            //Setup itemgroups required for the test
            _db.TableItemGroup.CreateItemGroup("Step7TestParent");

            //Prepare the test
            _iut = new ItemGroupViewModel(_db, _mb);
            _iut.ItemGroupName = "Step7TestChild";
            List<ItemGroup> temp = _iut.ComboBoxOptions.ToList();
            _iut.ComboBoxIndex = temp.FindIndex(x => x.ItemGroupName == "Step7TestParent");

            //Execute test
            _iut.CreateItemGroupCommand.Execute(null);
            Assert.That(_db.TableItemGroup.SearchItemGroups("Step7TestChild").Count, Is.EqualTo(1));
        }

        [Test]
        public void ItemGroupViewModel_CreateItemGroupCommand_InvalidNameGiven_NoItemGroupCreated()
        {
            _iut = new ItemGroupViewModel(_db, _mb) {ItemGroupName = "!Step7!"};

            _iut.CreateItemGroupCommand.Execute(null);
            Assert.That(_db.TableItemGroup.SearchItemGroups("!Step7!").Count, Is.EqualTo(0));
        }

        [Test]
        public void ItemGroupViewModel_DeleteItemGroupCommand_ValidItemGroupSelected_ItemGroupIsDeleted()
        {
            _db.TableItemGroup.CreateItemGroup("Step7DeleteThisItemGroup");
            _iut = new ItemGroupViewModel(_db,_mb);

            List<ItemGroup> temp = _iut.ListOfItemGroups.ToList();
            _iut.ListOfItemGroups.CurrentIndex = temp.FindIndex(x => x.ItemGroupName == "Step7DeleteThisItemGroup");

            _iut.DeleteItemGroupCommand.Execute(null);
            Assert.That(_db.TableItemGroup.SearchItemGroups("Step7DeleteThisItemGroup").Count, Is.EqualTo(0));
        }

        [Test]
        public void ItemGroupViewModel_DeleteItemGroupCommand_InvalidItemGroupSelected_ItemGroupIsNotDeleted()
        {
            _db.TableItemGroup.CreateItemGroup("Step7DeleteThisItemGroup");
            _iut = new ItemGroupViewModel(_db, _mb);

            _iut.DeleteItemGroupCommand.Execute(null);
            Assert.That(_db.TableItemGroup.SearchItemGroups("Step7DeleteThisItemGroup").Count, Is.EqualTo(1));
        }

        [Test]
        public void ItemGroupViewModel_EditItemGroupCommand_ItemIsEdited()
        {
            _db.TableItemGroup.CreateItemGroup("Step7IntegrationTest");
            _iut = new ItemGroupViewModel(_db, _mb);
            List<ItemGroup> temp = _iut.ListOfItemGroups.ToList();
            _iut.ListOfItemGroups.CurrentIndex = temp.FindIndex(x => x.ItemGroupName == "Step7IntegrationTest");
            _iut.ItemGroupName = "EditedStep7IntegrationTest";
            _iut.UpdateItemGroupName();
            _iut.EditItemGroupCommand.Execute(null);
            Assert.That(_db.TableItemGroup.SearchItemGroups("EditedStep7IntegrationTest").Count, Is.EqualTo(1));
        }

        [Test]
        public void ItemGroupViewModel_SearchCommand_SearchValidItemGroup_ItemGroupFound()
        {
            _db.TableItemGroup.CreateItemGroup("Step7SearchTest");

            _iut = new ItemGroupViewModel(_db, _mb);

            _iut.SearchString = "Step7SearchTest";

            _iut.SearchCommand.Execute(null);

            Assert.That(_iut.ListOfItemGroups.Count, Is.EqualTo(1));
        }

        [Test]
        public void ItemGroupViewModel_SearchCommand_SearchInvalidItemGroup_ItemGroupNotFound()
        {
            _iut = new ItemGroupViewModel(_db, _mb);

            _iut.SearchString = "Step7SearchTest";

            _iut.SearchCommand.Execute(null);

            Assert.That(_iut.ListOfItemGroups.Count, Is.EqualTo(0));
        }
    }

}
