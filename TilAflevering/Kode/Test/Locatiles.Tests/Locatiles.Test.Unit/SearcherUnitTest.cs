using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using LocatilesWebApp.Models;
using NSubstitute;
using NUnit.Framework;

namespace Locatiles.Test.Unit
{
    [TestFixture]
    public class SearcherUnitTest
    {
        private ISearcher _uut;
        private IDatabaseService _db;

        [SetUp]
        public void SetUp()
        {
            _db = Substitute.For<IDatabaseService>();
            _uut = new Searcher(_db);
            _db.TableItem.SearchItems(Arg.Any<string>()).Returns(new List<Item>());

        }

        [Test]
        public void Search_SearchItemsCalledWithSearchStr_SearchItemsReceivedCallWithSearchStr()
        {
            string searchStr = "unittest";
            _uut.Search(searchStr);
            _db.TableItem.Received(1).SearchItems(searchStr);
        }

        [Test]
        public void Search_SearchStrSplitIn2_SearchItemsReceivedCall2times()
        {
            string searchStr = "unit test";
            _uut.Search(searchStr);
            _db.TableItem.ReceivedWithAnyArgs(2).SearchItems(null);
        }

        [Test]
        public void Search_SearchStrContainOnlySpaces_SearchItemsReceivedOneCallWithEmptyStr()
        {
            string searchStr = "    ";
            _uut.Search(searchStr);
            _db.TableItem.Received(1).SearchItems("");
        }

        [Test]
        public void Search_SearchStrContainSameWordTwice_SearchResultDoesNotHaveDublicates()
        {
            _db.TableItem.SearchItems(Arg.Is("unit")).Returns(new List<Item>() {new Item(1, "unit", 1)});
            string searchStr = "unit unit";
            List<Item> _searchResult = _uut.Search(searchStr);
            Assert.That(_searchResult.Count, Is.EqualTo(1));

        }

        [Test]
        public void Search_SearchStrSplitInMany_EmptyStrGetsRemoved()
        {
            string searchStr = "unit test             result  ";
            _uut.Search(searchStr);
            _db.TableItem.ReceivedWithAnyArgs(3).SearchItems(null);
        }
    }
}
