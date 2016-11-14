using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using NUnit.Framework;
using NUnit.Framework.Internal;

namespace Locatiles.Test.Integration
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
        public void TableItemGroup_CreateItemGroupGetItemGroup_ItemGroupExistInDatabase()
        {
           long createdItemGroupID = _db.TableItemGroup.CreateItemGroup("Step2IntegrationTest_TestItemGroup1");

            ItemGroup fetchedItemGroup = _db.TableItemGroup.GetItemGroup(createdItemGroupID);

           Assert.That(fetchedItemGroup.ItemGroupID, Is.EqualTo(createdItemGroupID));
            

        }

        [Test]
        public void TableItemGroup_CreateItemGroupWParentID_ItemGroupExistsInDatabase()
        {
            long parentItemGroupID = _db.TableItemGroup.CreateItemGroup("Step2IntegrationTest_ParentItemGroup");
            long childItemgroupID = _db.TableItemGroup.CreateItemGroup("Step2IntegrationTest_ChildItemGroup",
                parentItemGroupID);

            ItemGroup fetchedItemGroup = _db.TableItemGroup.GetItemGroup(childItemgroupID);

            Assert.That(fetchedItemGroup.ItemGroupParentID, Is.EqualTo(parentItemGroupID));
        }

        [Test]
        public void TableItemGroup_SearchItemGroups_ReturnsRightCount()
        {
            long itemGroupID1 = _db.TableItemGroup.CreateItemGroup("Step2IntegrationTest_TestItemGroup1");
            long itemGroupID2 = _db.TableItemGroup.CreateItemGroup("Step2IntegrationTest_TestItemGroup2");
            long itemGroupID3 = _db.TableItemGroup.CreateItemGroup("Step2IntegrationTest_TestItemGroup3");
            long itemGroupID4 = _db.TableItemGroup.CreateItemGroup("Step2IntegrationTest_Test4");

            List<ItemGroup> fetchedItemGroups = _db.TableItemGroup.SearchItemGroups("ItemGroup");

            Assert.That(fetchedItemGroups.Count, Is.EqualTo(4));
        }

        [Test]
        public void TableItemGroup_UpdateItemGroup_ItemGroupNameIsChanged()
        {
            string newItemGroupName = "Step2IntegrationTest_NewItemGroupName";
            string oldItemGroupName = "Step2IntegrationTest_OldItemGroupName";
            long itemGroupID = _db.TableItemGroup.CreateItemGroup(oldItemGroupName);

            _db.TableItemGroup.UpdateItemGroup(oldItemGroupName,newItemGroupName);

            ItemGroup fetchedItemGroup = _db.TableItemGroup.GetItemGroup(itemGroupID);

            Assert.That(fetchedItemGroup.ItemGroupName, Is.EqualTo(newItemGroupName));

        }

        [Test]
        public void TableItemGroup_DeleteItemGroup_ItemGroupDoesNotExistInDatabase()
        {
            long itemGroupID = _db.TableItemGroup.CreateItemGroup("Step2IntegrationTest_TestItemGroup");

            _db.TableItemGroup.DeleteItemGroup(itemGroupID);

            ItemGroup fetchedItemGroup = _db.TableItemGroup.GetItemGroup(itemGroupID);

            Assert.That(fetchedItemGroup, Is.Null);
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
