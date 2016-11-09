using System.Collections.Generic;
using System.Linq;
using DatabaseAPI;
using DatabaseAPI.Factories;
using mainMenu.Models;
using mainMenu.ViewModels;
using NSubstitute;
using NUnit.Framework;

namespace ProjectGUI.Test.Integration
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
        public void ItemGroupViewModel_EditItemGroupCommand_ItemIsEdited()
        {
            _db.TableItemGroup.CreateItemGroup("Step7IntegrationTest", 0);
            _iut = new ItemGroupViewModel(_db, _mb);
            var shitlist = _iut.ListOfItemGroups.ToList();
            _iut.ListOfItemGroups.CurrentIndex = shitlist.FindIndex(x => x.ItemGroupName == "Step7IntegrationTest");
            _iut.ItemGroupName = "EditedStep7IntegrationTest";
            _iut.EditItemGroupCommand.Execute(null);
            Assert.That(_db.TableItemGroup.SearchItemGroups("EditedStep7IntegrationTest").Count, Is.EqualTo(1));
        }
    }
}
