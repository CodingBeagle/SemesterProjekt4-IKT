using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.TableStoreSection;
using NUnit.Framework;
using DatabaseAPI.Factories;
using DatabaseAPI.TableFloorplan;
using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;
using DatabaseAPI.TableItemSectionPlacement;

namespace DatabaseAPI.And.Storebase.IntegrationTest
{
    [TestFixture]
    public class DbAPIStoreDatabaseIntegrationstest
    {


        SqlStoreDatabaseFactory _storedatabasefactory = new SqlStoreDatabaseFactory();
        private ITableFloorplan floorplan;
        private ITableItem item;
        private ITableItemGroup itemGroup;
        private ITableItemSectionPlacement itemSectionPlacement;
        private ITableStoreSection sqlTableStore;
        private Item locItem;
        private ItemGroup locItemGroup;

        [SetUp]
        public void SetUp()
        {
            floorplan = _storedatabasefactory.CreateTableFloorplan();
            item = _storedatabasefactory.CreateTableItem();
            itemGroup = _storedatabasefactory.CreateTableItemGroup();
            itemSectionPlacement = _storedatabasefactory.CreateTableItemSectionPlacement();
            sqlTableStore = _storedatabasefactory.CreateTableStoreSection();
            locItemGroup = itemGroup.GetItemGroup(itemGroup.CreateItemGroup("TestItemGroup4")); //opretter ItemGroup på database samt local kopi
        }

        [TearDown]
        public void CleanUp()
        {
            if(locItem != null)
                item.DeleteItem(locItem.ItemID);
            locItem = null;
            if(locItemGroup != null)
                itemGroup.DeleteItemGroup(locItemGroup.ItemGroupID);
            locItemGroup = null;
        }

//Table - Item Group
        [Test] // CreateItemGroup() without parentID and GetItemGroup()
        public void CreateItemGroupGetItemGroup_CreateItemGroupAndGetItemGroupCalled_GetItemGroupReturnsCreatedItemGroup()
        {
            
        }

        [Test] // CreateItemGroup with parentID
        public void CreateItemGroupWithParentIDGetItemGroup_CreateItemGroupAndGetItemGroupCalled_GetItemGroupReturnsCreatedItemGroup()
        {
            
        }

        [Test] // DeleteItemGroup
        public void DeleteItemGroup_DeleteItemGroupCalled_GetReturnsNull()
        {

        }

        [Test] // DeleteItemGroup with referenced Item (not possible - throws exception)
        public void DeleteItemGroup_DeleteReferencedItemGroupCalled_DeleteItemGroupThrowsException()
        {

        }

        [Test] // GetAllItemGroups
        public void GetAllItemGroups_GetAllItemGroupsCalled_GetAllItemGroupsReturnsListOfItemGroups()
        {

        }



//Table - Item
        [Test] //CreateItem() and GetItem() tested
        public void CreateItemGetItem_CreateItemAndGetItemCalled_GetItemReturnsCreatedItem()
        {
            string testString = "TestItem4";
            long CreateID = item.CreateItem(testString, locItemGroup.ItemGroupID);
            locItem = item.GetItem(CreateID);
            Assert.That(locItem.Name == testString && locItem.ItemGroupID == locItemGroup.ItemGroupID);
        }


        [Test] // DeleteItem()
        public void DeleteItem_DeleteItemCalled_GetReturnsNull()
        {
            string testString = "TestItem4";
            long CreateID = item.CreateItem(testString, locItemGroup.ItemGroupID);
            item.DeleteItem(CreateID);
            locItem = item.GetItem(CreateID);

            Assert.That(locItem == null);
        }

        [Test] // SearchItems()
        public void SearchItems_SearchItemSForInsertedItems_ReturnsListWithMatchingItems()
        {
               
        }
//Table - StoreSection
        [Test] // CreateStoreSection() and GetStoreSection() Test
        public void CreateStoreSectionGetStoreSection_CreateStoreSectionAndGetStoreSectionCalled_GetStoreSectionReturnsCreatedStoreSection()
        {
            
        }

        [Test] // DeleteStoreSection() Test
        public void DeleteStoreSection_DeleteStoreSectionCalled_GetReturnsNull()
        {
            
        }

        [Test] // DeleteAllStoreSection() and GetAllStoreSection Test
        public void DeleteAllStoreSection_DeleteAllStoreSectionCalled_GetAllStoreSectionsReturnsEmptyList()
        {
            
        }

        [Test] // UpdateStoreSectionName()
        public void UpdateStoreSectionName_UpdateStoreSectionNameCalled_GetStoreSectionsReturnsWithNewName()
        {
            
        }

        [Test] // UpdateStoreSectionNCoordinate()
        public void UpdateStoreSectionCoordinate_UpdateStoreSectionCoordinateCalled_GetStoreSectionsReturnsWithNewCoordinate()
        {

        }
//Table - ItemSectionPlacement


    }
}

