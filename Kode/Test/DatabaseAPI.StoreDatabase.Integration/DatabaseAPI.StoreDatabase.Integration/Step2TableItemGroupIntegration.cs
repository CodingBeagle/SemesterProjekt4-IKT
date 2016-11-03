using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using NUnit.Framework;

namespace DatabaseAPI.StoreDatabase.Integration
{

    [TestFixture]
    class Step2TableItemGroupIntegration
    {
        private IDatabaseService _db;

        [SetUp]
        public void SetUp()
        {
            _db = new DatabaseService(new SqlStoreDatabaseFactory());

            CleanUp();
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }

        [Test]
        public void TableItemGroup_CreateItemGroup_ItemGroupExistInDatabase()
        {
           long testItemGroup1 = _db.TableItemGroup.CreateItemGroup("Step2IntegrationTest_TestItemGroup1");


        }

        private void CleanUp()
        {
            List<ItemGroup> itemGroupsToCleanUp = _db.TableItemGroup.SearchItemGroups("Step2IntegrationTest");

            foreach (var itemGroup in itemGroupsToCleanUp)
            {
                _db.TableItemGroup.DeleteItemGroup(itemGroup.ItemGroupID);
            }
        }

    }
}
