using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using LocatilesWebApp.Controllers;
using LocatilesWebApp.Models;
using NUnit.Framework;

namespace Locatiles.Test.Integration
{
    [TestFixture]
    public class Step11HomeControllerIntegration
    {
        private HomeController _homeController;
        private ISearcher _searcher;
        private IDatabaseService _db;
        private IBLL _bll;
        private int storesectionID;
        private string itemGroupName;
        private string itemName;

        [SetUp]
        public void SetUp()
        {
            _searcher = new Searcher();
            _db = new DatabaseService(new SqlStoreDatabaseFactory());
            _bll = new BLL(_searcher, _db);
            _homeController = new HomeController();

            itemGroupName = "Step11ItemGroup";
            itemName = "Step11TestItem";
            storesectionID = (int)_db.TableStoreSection.CreateStoreSection("Step11StoreSection", 100, 100, 1);
            _db.TableItemSectionPlacement.PlaceItem(_db.TableItem.CreateItem(itemName, _db.TableItemGroup.CreateItemGroup(itemGroupName)), storesectionID);
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }

        [Test]
        public void SearchItems_GetPresentationItemGroupsReturnList_ViewBagContainCorrectList()
        {
            var result = _homeController.SearchItems(itemGroupName) as ViewResult;
            var data = (List<PresentationItemGroup>)result.ViewData["PresentationItemGroups"];
            Assert.That(itemGroupName, Is.EqualTo(data[0].Name));
        }

        [Test]
        public void SearchItems_SearchItemsCalled_ReturnsIndexView()
        {
            string searchStr = "Step11TestItem";
            var result = _homeController.SearchItems(searchStr) as ViewResult;
            Assert.AreEqual("Index", result.ViewName);
        }

        private void CleanUp()
        {
            // Cleanup items
            List<Item> itemsToCleanUp = _db.TableItem.SearchItems("Step11TestItem");
            foreach (var item in itemsToCleanUp)
            {
                _db.TableItem.DeleteItem(item.ItemID);
            }

            // Cleanup item groups
            List<ItemGroup> itemGroupsToCleanup = _db.TableItemGroup.SearchItemGroups("Step11ItemGroup");
            foreach (var itemGroup in itemGroupsToCleanup)
            {
                _db.TableItemGroup.DeleteItemGroup(itemGroup.ItemGroupID);
            }

            //Cleanup storesections
            _db.TableStoreSection.DeleteStoreSection(storesectionID);
        }

    }
}
