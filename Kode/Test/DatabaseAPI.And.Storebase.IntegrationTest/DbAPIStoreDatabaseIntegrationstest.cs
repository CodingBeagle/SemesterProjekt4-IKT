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
            locItemGroup = itemGroup.GetItemGroup(itemGroup.CreateItemGroup("TestItemGroup5")); //opretter ItemGroup på database samt local kopi
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

        [Test]
        public void CreateItem_CreateItemCalled_GetItemReturnsCreatedItem()
        {
            string testString = "TestItem5";
            long CreateID = item.CreateItem(testString, locItemGroup.ItemGroupID);
            Item locItem = item.GetItem(CreateID);
            Assert.That(locItem.Name == testString && locItem.ItemGroupID == locItemGroup.ItemGroupID);
        }


        [Test]
        public void DeleteItem_DeleteItemCalled_GetReturnsNull()
        {
            string testString = "TestItem6";
            long CreateID = item.CreateItem(testString, locItemGroup.ItemGroupID);
            item.DeleteItem(CreateID);
            locItem = item.GetItem(CreateID);

            Assert.That(locItem == null);

        }


    }
}

