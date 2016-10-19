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
        //test push for git

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
            itemGroup.DeleteItemGroup(locItemGroup.ItemGroupID);
            Assert.That(itemGroup.GetItemGroup(locItemGroup.ItemGroupID)==null);
        }

        [Test] // DeleteItemGroup - Parent group (not possible)
        public void DeleteItemGroup_DeleteItemGroupParentGroup_DeleteItemGroupThrowsException()
        {
            long createID = itemGroup.CreateItemGroup("ChildItemGroup", locItemGroup.ItemGroupID);
            Assert.Throws<SqlException>(() => itemGroup.DeleteItemGroup(locItemGroup.ItemGroupID));
            itemGroup.DeleteItemGroup(createID); 
        }

        [Test] // DeleteItemGroup with referenced Item (not possible - throws exception)
        public void DeleteItemGroup_DeleteReferencedItemGroupCalled_DeleteItemGroupThrowsException()
        {
            ItemGroup tItemGroup = itemGroup.GetItemGroup(itemGroup.CreateItemGroup("ItemSubGroupTest", locItemGroup.ItemGroupID));
            Item tItem = item.GetItem(item.CreateItem("TestItemWithSubGroup", tItemGroup.ItemGroupID));

            Assert.Throws<SqlException>(() => itemGroup.DeleteItemGroup(tItemGroup.ItemGroupID));

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

        [Test]
        public void UpdateItemGroup_ItemGroupInsertedAndEdited_GetItemGroupReturnsInserted()
        {
            string updatedName = "NewItemGroupname";          
            itemGroup.UpdateItemGroup(locItemGroup.ItemGroupName, updatedName);
            locItemGroup = itemGroup.GetItemGroup(locItemGroup.ItemGroupID);

            Assert.That(locItemGroup.ItemGroupName == updatedName);
        }

        [Test]
        public void SearchItemGroup_InsertItemGroupsAndSearch_ReturnsListWithMatchingItemGroups()
        {
            ItemGroup itemGroup1 = itemGroup.GetItemGroup(itemGroup.CreateItemGroup("FindMeblabla"));
            ItemGroup itemGroup2 = itemGroup.GetItemGroup(itemGroup.CreateItemGroup("blablaFindMeblabla"));
            ItemGroup itemGroup3 = itemGroup.GetItemGroup(itemGroup.CreateItemGroup("blablaFindMe"));

            List<long> insertedItemGroups = new List<long>();
            insertedItemGroups.Add(itemGroup1.ItemGroupID);
            insertedItemGroups.Add(itemGroup2.ItemGroupID);
            insertedItemGroups.Add(itemGroup3.ItemGroupID);
            List<ItemGroup> searchResultItemGroups = itemGroup.SearchItemGroups("FindMe");

            int matchingSearch = 0;
            foreach (var searchResult in searchResultItemGroups)
            {
                if (insertedItemGroups.Contains(searchResult.ItemGroupID))
                {
                    matchingSearch++;
                }
            }

            Assert.That(matchingSearch == insertedItemGroups.Count);

            itemGroup.DeleteItemGroup(itemGroup1.ItemGroupID);
            itemGroup.DeleteItemGroup(itemGroup2.ItemGroupID);
            itemGroup.DeleteItemGroup(itemGroup3.ItemGroupID);

        }

        [Test]
        public void SearchItemGroup_InsertItemGroupsAndSearch_ReturnsEmptyList()
        {
            ItemGroup itemGroup1 = itemGroup.GetItemGroup(itemGroup.CreateItemGroup("blabla"));
            ItemGroup itemGroup2 = itemGroup.GetItemGroup(itemGroup.CreateItemGroup("blablaasdzxcblabla"));
            ItemGroup itemGroup3 = itemGroup.GetItemGroup(itemGroup.CreateItemGroup("blablaliqwje"));

            List<long> insertedItemGroups = new List<long>();
            insertedItemGroups.Add(itemGroup1.ItemGroupID);
            insertedItemGroups.Add(itemGroup2.ItemGroupID);
            insertedItemGroups.Add(itemGroup3.ItemGroupID);
            List<ItemGroup> searchResultItemGroups = itemGroup.SearchItemGroups("FindMe");

            int matchingSearch = 0;
            foreach (var searchResult in searchResultItemGroups)
            {
                if (insertedItemGroups.Contains(searchResult.ItemGroupID))
                {
                    matchingSearch++;
                }
            }

            Assert.That(matchingSearch == 0);

            itemGroup.DeleteItemGroup(itemGroup1.ItemGroupID);
            itemGroup.DeleteItemGroup(itemGroup2.ItemGroupID);
            itemGroup.DeleteItemGroup(itemGroup3.ItemGroupID);

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
            Item item1 = item.GetItem(item.CreateItem("blablaFindMeblabla", locItemGroup.ItemGroupID));
            Item item2 = item.GetItem(item.CreateItem("blaFindMe", locItemGroup.ItemGroupID));
            Item item3 = item.GetItem(item.CreateItem("FindMeblabla", locItemGroup.ItemGroupID));

            List<long> insertedItems = new List<long>();
            insertedItems.Add(item1.ItemID);
            insertedItems.Add(item2.ItemID);
            insertedItems.Add(item3.ItemID);
            List<Item> searchResultItems = item.SearchItems("FindMe");

            int matchingSearch = 0;
            foreach (var searchResult in searchResultItems)
            {
                if (insertedItems.Contains(searchResult.ItemID))
                {
                    matchingSearch++;
                }
            }

            Assert.That(matchingSearch == insertedItems.Count);

            item.DeleteItem(item1.ItemID);
            item.DeleteItem(item2.ItemID);
            item.DeleteItem(item3.ItemID);
    
        }

        [Test] // SearchItems() no items found
        public void SearchItems_SearchItemSForInsertedItems_ReturnsEmptyList()
        {
            Item item1 = item.GetItem(item.CreateItem("blablablabla", locItemGroup.ItemGroupID));
            Item item2 = item.GetItem(item.CreateItem("bla", locItemGroup.ItemGroupID));
            Item item3 = item.GetItem(item.CreateItem("blabla", locItemGroup.ItemGroupID));

            List<long> insertedItems = new List<long>();
            insertedItems.Add(item1.ItemID);
            insertedItems.Add(item2.ItemID);
            insertedItems.Add(item3.ItemID);
            List<Item> searchResultItems = item.SearchItems("FindMe");

            int matchingSearch = 0;
            foreach (var searchResult in searchResultItems)
            {
                if (insertedItems.Contains(searchResult.ItemID))
                {
                    matchingSearch++;
                }
            }

            Assert.That(matchingSearch == 0);

            item.DeleteItem(item1.ItemID);
            item.DeleteItem(item2.ItemID);
            item.DeleteItem(item3.ItemID);

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

            item.DeleteItem(item1.ItemID);
            item.DeleteItem(item2.ItemID);
            item.DeleteItem(item3.ItemID);
        }

        [Test]
        public void FindPlacementsByItem_ItemPlacedInTwoSections_ReturnsSectionsOfItem()
        {
            StoreSection tStoreSection = storeSection.GetStoreSection(storeSection.CreateStoreSection("A38", 4, 2, 1));
            Item item1 = item.GetItem(item.CreateItem("TestPlaceItem1", locItemGroup.ItemGroupID));
            itemSectionPlacement.PlaceItem(item1.ItemID, locStoreSection.StoreSectionID);            
            itemSectionPlacement.PlaceItem(item1.ItemID, tStoreSection.StoreSectionID);
            List<StoreSection> retSecs = itemSectionPlacement.FindPlacementsByItem(item1.ItemID);
            List<long> retSecsIDs = new List<long>();
            foreach(var sec in retSecs)
                retSecsIDs.Add(sec.StoreSectionID);
            Assert.That(retSecsIDs.Contains(locStoreSection.StoreSectionID) && retSecsIDs.Contains(tStoreSection.StoreSectionID));

            item.DeleteItem(item1.ItemID);
            storeSection.DeleteStoreSection(tStoreSection.StoreSectionID);
        }

        [Test]
        public void DeleteAllPlacementsInSection_DeleteAllPlacementsInSectionCalled_ListItemsInSectionReturnsEmptyList()
        {
            Item item1 = item.GetItem(item.CreateItem("TestPlaceItem1", locItemGroup.ItemGroupID));
            Item item2 = item.GetItem(item.CreateItem("TestPlaceItem2", locItemGroup.ItemGroupID));
            itemSectionPlacement.PlaceItem(item1.ItemID, locStoreSection.StoreSectionID);
            itemSectionPlacement.PlaceItem(item2.ItemID, locStoreSection.StoreSectionID);

            itemSectionPlacement.DeleteAllPlacementsInSection(locStoreSection.StoreSectionID);

            List<Item> retItems = itemSectionPlacement.ListItemsInSection(locStoreSection.StoreSectionID);
            Assert.That(retItems.Count==0);

            item.DeleteItem(item1.ItemID);
            item.DeleteItem(item2.ItemID);
        }

        [Test]
        public void DeletePlacementsByItem_DeletePlacementByItemCalled_FindPlacementByItemReturnsEmptyList()
        {
            Item item1 = item.GetItem(item.CreateItem("TestPlaceItem1", locItemGroup.ItemGroupID));
            itemSectionPlacement.PlaceItem(item1.ItemID, locStoreSection.StoreSectionID);
            itemSectionPlacement.DeletePlacementsByItem(item1.ItemID);

            Assert.That(itemSectionPlacement.FindPlacementsByItem(item1.ItemID).Count==0);

            item.DeleteItem(item1.ItemID);
        }

        [Test]
        public void DeletePlacement_ItemPlacedAndDeleted_FindPlacementByItemReturnsEmptyList()
        {
            Item item1 = item.GetItem(item.CreateItem("TestPlaceItem1", locItemGroup.ItemGroupID));
            itemSectionPlacement.PlaceItem(item1.ItemID, locStoreSection.StoreSectionID);
            itemSectionPlacement.DeletePlacement(item1.ItemID,locStoreSection.StoreSectionID);

            Assert.That(itemSectionPlacement.FindPlacementsByItem(item1.ItemID).Count == 0);

            item.DeleteItem(item1.ItemID);
        }
    }
}

