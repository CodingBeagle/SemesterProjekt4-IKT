using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using NUnit.Framework;

namespace DatabaseAPI.StoreDatabase.Integration
{
    [TestFixture]
    class Step5TableItemSectionPlacementIntegration
    {
        private IDatabaseService _db;
        private int _floorplanID = 1;
        private long _storeSectionID1 = 0;
        private long _storeSectionID2 = 0;
        private long _itemID1 = 0;
        private long _itemID2 = 0;
        private long _itemgroupID = 0;

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
        public void TableItemSectionPlacement_PlaceItemListItemInSection_ItemIsPlacedInSection()
        {
            _itemgroupID = _db.TableItemGroup.CreateItemGroup("Step5Integration_TestItemGroup");
            _itemID1 = _db.TableItem.CreateItem("Step5Integration_TestItem", _itemgroupID);
            _storeSectionID1 = _db.TableStoreSection.CreateStoreSection("Step5Integration_TestStoreSection", 20, 20,
                _floorplanID);

            _db.TableItemSectionPlacement.PlaceItem(_itemID1, _storeSectionID1);

            List<Item> fetchedItems = _db.TableItemSectionPlacement.ListItemsInSection(_storeSectionID1);

            foreach (var item in fetchedItems)
            {
                Assert.That(item.ItemID, Is.EqualTo(_itemID1));
            }
        }

        [Test]
        public void TableItemSectionPlacement_PlaceItemFindPlacementsByItem_ItemIsFoundInSection()
        {
            _itemgroupID = _db.TableItemGroup.CreateItemGroup("Step5Integration_TestItemGroup");
            _itemID1 = _db.TableItem.CreateItem("Step5Integration_TestItem", _itemgroupID);
            _storeSectionID1 = _db.TableStoreSection.CreateStoreSection("Step5Integration_TestStoreSection", 20, 20,
                _floorplanID);

            _db.TableItemSectionPlacement.PlaceItem(_itemID1, _storeSectionID1);

            List<StoreSection> fetchedStoreSections = _db.TableItemSectionPlacement.FindPlacementsByItem(_itemID1);

            foreach (var section in fetchedStoreSections)
            {
                Assert.That(section.StoreSectionID, Is.EqualTo(_storeSectionID1));
            }
        }

        [Test]
        public void TableItemSectionPlacement_DeleteAllPlacementsInSection_ReturnsEmptySection()
        {
            _itemgroupID = _db.TableItemGroup.CreateItemGroup("Step5Integration_TestItemGroup");
            _itemID1 = _db.TableItem.CreateItem("Step5Integration_TestItem1", _itemgroupID);
            _itemID2 = _db.TableItem.CreateItem("Step5Integration_TestItem2", _itemgroupID);
            _storeSectionID1 = _db.TableStoreSection.CreateStoreSection("Step5Integration_TestStoreSection", 20, 20,
                _floorplanID);


            _db.TableItemSectionPlacement.PlaceItem(_itemID1, _storeSectionID1);
            _db.TableItemSectionPlacement.PlaceItem(_itemID2, _storeSectionID1);

            _db.TableItemSectionPlacement.DeleteAllPlacementsInSection(_storeSectionID1);

            List<Item> fetchedItems = _db.TableItemSectionPlacement.ListItemsInSection(_storeSectionID1);

            Assert.That(fetchedItems.Count, Is.EqualTo(0));

        }

        [Test]
        public void TableItemSectionPlacement_DeletePlacementsByItem_ReturnsNoPlacements()
        {
            _itemgroupID = _db.TableItemGroup.CreateItemGroup("Step5Integration_TestItemGroup");
            _itemID1 = _db.TableItem.CreateItem("Step5Integration_TestItem1", _itemgroupID);
            _storeSectionID1 = _db.TableStoreSection.CreateStoreSection("Step5Integration_TestStoreSection1", 20, 20,
                _floorplanID);
            _storeSectionID2 = _db.TableStoreSection.CreateStoreSection("Step5Integration_TestStoreSection2", 30, 30,
                _floorplanID);

            _db.TableItemSectionPlacement.PlaceItem(_itemID1, _storeSectionID1);
            _db.TableItemSectionPlacement.PlaceItem(_itemID1, _storeSectionID2);

            _db.TableItemSectionPlacement.DeletePlacementsByItem(_itemID1);

            List<StoreSection> fetchedStoreSections = _db.TableItemSectionPlacement.FindPlacementsByItem(_itemID1);

            Assert.That(fetchedStoreSections.Count, Is.EqualTo(0));
        }

        [Test]
        public void TableItemSectionPlacement_DeletePlacement_PlacementDeleted()
        {
            _itemgroupID = _db.TableItemGroup.CreateItemGroup("Step5Integration_TestItemGroup");
            _itemID1 = _db.TableItem.CreateItem("Step5Integration_TestItem", _itemgroupID);
            _storeSectionID1 = _db.TableStoreSection.CreateStoreSection("Step5Integration_TestStoreSection", 20, 20,
                _floorplanID);

            _db.TableItemSectionPlacement.PlaceItem(_itemID1, _storeSectionID1);

            _db.TableItemSectionPlacement.DeletePlacement(_itemID1, _storeSectionID1);

            List<StoreSection> fetchedStoreSections = _db.TableItemSectionPlacement.FindPlacementsByItem(_itemID1);

            Assert.That(fetchedStoreSections, Is.Empty);
        }

        private void CleanUp()
        {
            if (_itemID1 != 0)
            {
                _db.TableItem.DeleteItem(_itemID1);    
            }

            if (_itemID2 != 0)
            {
                _db.TableItem.DeleteItem(_itemID2);
            }

            if (_itemgroupID != 0)
            {
                _db.TableItemGroup.DeleteItemGroup(_itemgroupID);    
            }

            if (_storeSectionID1 != 0)
            {
                _db.TableStoreSection.DeleteStoreSection(_storeSectionID1);
            }

            if (_storeSectionID2 != 0)
            {
                _db.TableStoreSection.DeleteStoreSection(_storeSectionID2);
            }
        }
    }
}
