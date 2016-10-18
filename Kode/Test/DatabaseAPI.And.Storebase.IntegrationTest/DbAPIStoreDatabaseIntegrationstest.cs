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
        private ITableStoreSection storeSection;
        private Item locItem;
        private ItemGroup locItemGroup;
        private StoreSection locStoreSection;
        private List<StoreSection> locAllStoreSections;

        [SetUp]
        public void SetUp()
        {
            floorplan = _storedatabasefactory.CreateTableFloorplan();
            item = _storedatabasefactory.CreateTableItem();
            itemGroup = _storedatabasefactory.CreateTableItemGroup();
            itemSectionPlacement = _storedatabasefactory.CreateTableItemSectionPlacement();
            storeSection = _storedatabasefactory.CreateTableStoreSection();
            locItemGroup = itemGroup.GetItemGroup(itemGroup.CreateItemGroup("DbItemGroup")); //opretter ItemGroup på database samt local kopi
            locStoreSection = storeSection.GetStoreSection(storeSection.CreateStoreSection("TestStoreSection", 1, 2, 1));

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
            if(locStoreSection!=null)
                storeSection.DeleteStoreSection(locStoreSection.StoreSectionID);
            locStoreSection = null;
        }

//Table - Item Group
        [Test] // CreateItemGroup() without parentID and GetItemGroup()
        public void CreateItemGroupGetItemGroup_CreateItemGroupAndGetItemGroupCalled_GetItemGroupReturnsCreatedItemGroup()
        {
            ItemGroup tItemGroup = new ItemGroup("ItemGroupTest", 0, itemGroup.CreateItemGroup("ItemGroupTest")); 
            ItemGroup retItemGroup = itemGroup.GetItemGroup(tItemGroup.ItemGroupID);

            Assert.That(tItemGroup.ItemGroupID==retItemGroup.ItemGroupID && tItemGroup.ItemGroupName==retItemGroup.ItemGroupName);

            itemGroup.DeleteItemGroup(tItemGroup.ItemGroupID);
        }

        [Test] // CreateItemGroup with parentID
        public void CreateItemGroupWithParentIDGetItemGroup_CreateItemGroupAndGetItemGroupCalled_GetItemGroupReturnsCreatedItemGroup()
        {
            ItemGroup tItemGroup = new ItemGroup("ItemGroupTest", locItemGroup.ItemGroupID, itemGroup.CreateItemGroup("ItemGroupTest", locItemGroup.ItemGroupID));
            ItemGroup retItemGroup = itemGroup.GetItemGroup(tItemGroup.ItemGroupID);

            Assert.That(tItemGroup.ItemGroupID == retItemGroup.ItemGroupID 
                && tItemGroup.ItemGroupName == retItemGroup.ItemGroupName
                && tItemGroup.ItemGroupParentID == retItemGroup.ItemGroupParentID);

            itemGroup.DeleteItemGroup(tItemGroup.ItemGroupID);
        }

        [Test] // DeleteItemGroup
        public void DeleteItemGroup_DeleteItemGroupCalled_GetReturnsNull()
        {
            itemGroup.DeleteItemGroup(locItem.ItemGroupID);
            Assert.That(itemGroup.GetItemGroup(locItemGroup.ItemGroupID)==null);
        }

        [Test] // DeleteItemGroup with referenced Item (not possible - throws exception)
        public void DeleteItemGroup_DeleteReferencedItemGroupCalled_DeleteItemGroupThrowsException()
        {
            ItemGroup tItemGroup = new ItemGroup("ItemTest",locItemGroup.ItemGroupID,itemGroup.CreateItemGroup("ItemGroupTest", locItemGroup.ItemGroupID));
            Item tItem = new Item(0,"",0);
            tItem.ItemID = item.CreateItem(tItemGroup.ItemGroupName, tItemGroup.ItemGroupID);

            Assert.Throws<SystemException>(() => itemGroup.DeleteItemGroup(tItemGroup.ItemGroupID));

            item.DeleteItem(tItem.ItemID);
            itemGroup.DeleteItemGroup(tItemGroup.ItemGroupID);
        }

        [Test] // GetAllItemGroups
        public void GetAllItemGroups_GetAllItemGroupsCalled_GetAllItemGroupsReturnsListOfItemGroups()
        {
            List<long> itemGroupIDs = new List<long>();
            itemGroupIDs.Add(itemGroup.CreateItemGroup("ItemGroupTest1"));
            itemGroupIDs.Add(itemGroup.CreateItemGroup("ItemGroupTest2"));
            itemGroupIDs.Add(itemGroup.CreateItemGroup("ItemGroupTest3"));
            List<ItemGroup> retItemGroups = itemGroup.GetAllItemGroups();

            int count = 0;
            foreach (var iGroup in retItemGroups)
            {
                if (itemGroupIDs.Contains(iGroup.ItemGroupID)) //if item not removed
                {
                    itemGroup.DeleteItemGroup(iGroup.ItemGroupID);
                    count++;
                    if (count == 3)
                        break;
                }
            }
            Assert.That(count==3);
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
            storeSection.DeleteStoreSection(locStoreSection.StoreSectionID);
            string testString = "TestStoreSectionCreate";
            long coordinateX = 1;
            long coordinateY = 2;
            long floodPlanID = 1; // skal erstates med værdi korrekt værdi fra floorplan.
            long createID = storeSection.CreateStoreSection(testString, coordinateX, coordinateY, floodPlanID);
            locStoreSection = storeSection.GetStoreSection(createID);
            Assert.That(locStoreSection.Name == testString && locStoreSection.CoordinateX == coordinateX && locStoreSection.CoordinateY == coordinateY);
        }

        [Test] // DeleteStoreSection() Test
        public void DeleteStoreSection_DeleteStoreSectionCalled_GetReturnsNull()
        {
            storeSection.DeleteStoreSection(locStoreSection.StoreSectionID);
            locStoreSection = storeSection.GetStoreSection(locStoreSection.StoreSectionID);

            Assert.That(locStoreSection == null);
        }

        //[Test] // DeleteAllStoreSection() and GetAllStoreSection Test
        //public void DeleteAllStoreSection_DeleteAllStoreSectionCalled_GetAllStoreSectionsReturnsEmptyList()
        //{
        //    string testString1 = "TestStoreSection1";
        //    long coordinateX1 = 1;
        //    long coordinateY1 = 2;
        //    string testString2 = "TestStoreSection2";
        //    long coordinateX2 = 2;
        //    long coordinateY2 = 3;
        //    long floodPlanID = 1; // skal erstates med værdi korrekt værdi fra floorplan.
        //    storeSection.CreateStoreSection(testString1, coordinateX1, coordinateY1, floodPlanID);
        //    storeSection.CreateStoreSection(testString2, coordinateX2, coordinateY2, floodPlanID);
        //    storeSection.DeleteStoreSection(floodPlanID);
        //    locAllStoreSections = storeSection.GetAllStoreSections(floodPlanID);
        //    storeSection.DeleteAllStoreSections(floodPlanID);
        //    Assert.That(locAllStoreSections == null);

        //}

        [Test] // UpdateStoreSectionName()
        public void UpdateStoreSectionName_UpdateStoreSectionNameCalled_GetStoreSectionsReturnsWithNewName()
        {
            string updatedName = "NewSectionname";
            storeSection.UpdateStoreSectionName(locStoreSection.StoreSectionID,updatedName);
            locStoreSection = storeSection.GetStoreSection(locStoreSection.StoreSectionID);

            Assert.That(locStoreSection.Name == updatedName);         
        }

        [Test] // UpdateStoreSectionCoordinate()
        public void UpdateStoreSectionCoordinate_UpdateStoreSectionCoordinateCalled_GetStoreSectionsReturnsWithNewCoordinate()
        {

            long newcoordinateX = 5;
            long newcoordinateY = 6;
            storeSection.UpdateStoreSectionCoordinate(locStoreSection.StoreSectionID, newcoordinateX, newcoordinateY);
            locStoreSection = storeSection.GetStoreSection(locStoreSection.StoreSectionID);

            Assert.That(locStoreSection.CoordinateX == newcoordinateX && locStoreSection.CoordinateY == newcoordinateY);
        }
//Table - ItemSectionPlacement
        [Test]
        public void PlaceItem_PlaceItemCalled_ListItemsInSectionReturnsListofItems()
        {   

            // Creating and placing 3 items in TestStoreSection
            Item item1 = item.GetItem(item.CreateItem("TestPlaceItem1", locItemGroup.ItemGroupID));
            Item item2 = item.GetItem(item.CreateItem("TestPlaceItem2", locItemGroup.ItemGroupID));
            Item item3 = item.GetItem(item.CreateItem("TestPlaceItem3", locItemGroup.ItemGroupID));
            itemSectionPlacement.PlaceItem(item1.ItemID, locStoreSection.StoreSectionID);
            itemSectionPlacement.PlaceItem(item2.ItemID, locStoreSection.StoreSectionID);
            itemSectionPlacement.PlaceItem(item3.ItemID, locStoreSection.StoreSectionID);
            List<Item> insertedItems = new List<Item>();
            insertedItems.Add(item1);
            insertedItems.Add(item2);
            insertedItems.Add(item3);
            List<Item> itemsInSection = itemSectionPlacement.ListItemsInSection(locStoreSection.StoreSectionID);
           
            Assert.That(insertedItems.Count == itemsInSection.Count);
        }

    }
}

