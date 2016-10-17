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


        [SetUp]
        public void SetUp()
        {
            floorplan = _storedatabasefactory.CreateTableFloorplan();
            item = _storedatabasefactory.CreateTableItem();
            itemGroup = _storedatabasefactory.CreateTableItemGroup();
            itemSectionPlacement = _storedatabasefactory.CreateTableItemSectionPlacement();
            sqlTableStore = _storedatabasefactory.CreateTableStoreSection();
        }

        [Test]
        public void CreateItem_CreateItemCalled_GetItemReturnsCreatedItem()
        {
            string testString = "Bacon";
            long testItemGroupID = itemGroup.CreateItemGroup("Frost kød");
            Item returnItem = item.GetItem(item.CreateItem(testString, testItemGroupID));
            
            Assert.That(returnItem.Name == testString && returnItem.ItemGroupID == testItemGroupID);
        }



    }
}

