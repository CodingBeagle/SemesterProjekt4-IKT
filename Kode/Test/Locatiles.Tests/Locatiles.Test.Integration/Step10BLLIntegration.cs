using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using DatabaseAPI.TableFloorplan;
using DatabaseAPI.TableItem;
using DatabaseAPI.TableItemGroup;
using DatabaseAPI.TableItemSectionPlacement;
using LocatilesWebApp.Models;
using NSubstitute;
using NUnit.Framework;

namespace Locatiles.Test.Integration
{
    [TestFixture]
    public class Step10BLLIntegration
    {
        private IBLL _bll;
        private ISearcher _searcher;
        private IDatabaseService _db;
        private int storesectionID;
        private string itemGroupName;
        private string itemName;
        [SetUp]
        public void SetUp()
        {

            _searcher = new Searcher();
            _bll = new BLL(_searcher);
            _db  = new DatabaseService(new SqlStoreDatabaseFactory());
            itemGroupName = "Step10ItemGroup";
            itemName = "Step10Item";
            storesectionID = (int)_db.TableStoreSection.CreateStoreSection("Step10StoreSection", 100, 100, 1);
            _db.TableItemSectionPlacement.PlaceItem(_db.TableItem.CreateItem(itemGroupName, _db.TableItemGroup.CreateItemGroup("Step10ItemGroup")), storesectionID);
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }

        [Test]
        public void GetPresentationItemGroups_SearchForExistingItem_ReturnsPresentationItemGroup()
        {
            List<PresentationItemGroup> searchResult = _bll.GetPresentationItemGroups(itemName);
            Assert.That(searchResult.Any(pig => pig.Name == itemGroupName));
        }

        private void CleanUp()
        {
            // Cleanup items
            List<Item> itemsToCleanUp = _db.TableItem.SearchItems("Step10Item");
            foreach (var item in itemsToCleanUp)
            {
                _db.TableItem.DeleteItem(item.ItemID);
            }

            // Cleanup item groups
            List<ItemGroup> itemGroupsToCleanup = _db.TableItemGroup.SearchItemGroups("Step10ItemGroup");
            foreach (var itemGroup in itemGroupsToCleanup)
            {
                _db.TableItemGroup.DeleteItemGroup(itemGroup.ItemGroupID);
            }

            //Cleanup storesections
            _db.TableStoreSection.DeleteStoreSection(storesectionID);
        }
    }
}
