using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using LocatilesWebApp.Models;
using NSubstitute;
using NUnit.Framework;

namespace Locatiles.Test.Integration
{
    [TestFixture]
    public class Step8SearcherIntegration
    {
        private ISearcher _searcher;
        private IDatabaseService _db;

        [SetUp]
        public void SetUp()
        {
            _db = new DatabaseService(new SqlStoreDatabaseFactory());
            _searcher = new Searcher(_db);
            _db.TableItem.CreateItem("Step8TestItem", _db.TableItemGroup.CreateItemGroup("Step8TestItemGroup"));
        }

        [TearDown]
        public void TearDown()
        {
            CleanUp();
        }

        [Test]
        public void Search_SearchForKnownTestItem_SearchResultContainTestItem()
        {
            string searchStr = "Step8TestItem";
            List<Item> _searchResult = _searcher.Search(searchStr);
            Assert.That(_searchResult.Any(i => i.Name == searchStr));
        }

        private void CleanUp()
        {
            // Cleanup items
            List<Item> itemsToCleanUp = _db.TableItem.SearchItems("Step8TestItem");
            foreach (var item in itemsToCleanUp)
            {
                _db.TableItem.DeleteItem(item.ItemID);
            }
            // Cleanup itemGroups
            List<ItemGroup> itemGroupsToCleanUp = _db.TableItemGroup.SearchItemGroups("Step8TestItemGroup");

            foreach (var itemGroup in itemGroupsToCleanUp)
            {
                _db.TableItemGroup.DeleteItemGroup(itemGroup.ItemGroupID);
            }


        }
    }
}
