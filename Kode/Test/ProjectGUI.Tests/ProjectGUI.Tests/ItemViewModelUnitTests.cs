using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI;
using DatabaseAPI.Factories;
using mainMenu.ViewModels;
using NSubstitute;
using NUnit.Framework;


namespace ProjectGUI.Tests
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
            
            _uut = new ItemViewModel(_db, _mb);
        }

        [Test]
        public void ItemViewModel_DeleteItemCommand_DeleteItemIsCalled()
        {
            _uut.ItemName = "test";
            _uut.DeleteItemCommand
        }
    }
}
