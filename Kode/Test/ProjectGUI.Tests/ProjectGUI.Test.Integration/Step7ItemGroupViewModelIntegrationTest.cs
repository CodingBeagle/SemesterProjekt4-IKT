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
            _mb = Substitute.For<IMessageBox>();
            _db = new DatabaseService(new SqlStoreDatabaseFactory());
        }
    }
}
