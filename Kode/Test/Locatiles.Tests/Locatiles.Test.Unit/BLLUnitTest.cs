using System;
using System.Collections.Generic;
using System.Linq;
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

namespace Locatiles.Test.Unit
{
    [TestFixture]
    public class BLLUnitTest
    {
        private IDatabaseService _db;
        private ITableItem _tableItem;
        private ITableFloorplan _tableFloorplan;
        private ITableItemGroup _tableItemGroup;
        private ITableItemSectionPlacement _tablePlacement;
        private ITableStoreSection _tableStoreSection;
        private IBLL _uut;
        private ISearcher _searcher;

        [SetUp]
        public void setup()
        {         
            //Creates Substitutes
            _db = Substitute.For<IDatabaseService>();
            _tableItem = Substitute.For<ITableItem>();
            _tableFloorplan = Substitute.For<ITableFloorplan>();
            _tableItemGroup = Substitute.For<ITableItemGroup>();
            _tablePlacement = Substitute.For<ITableItemSectionPlacement>();
            _tableStoreSection = Substitute.For<ITableStoreSection>();
            _searcher = Substitute.For<ISearcher>();

            //Sets common empty returns
            _tablePlacement.FindPlacementsByItem(Arg.Any<long>()).Returns(new List<StoreSection>());
            _tableItemGroup.GetItemGroup(Arg.Any<long>()).Returns(new ItemGroup("",0,0));
            LoadBLL();
        }

        private void LoadBLL()
        {
            // Database substitute returns substitutes 
            _db.TableItem.Returns(_tableItem);
            _db.TableFloorplan.Returns(_tableFloorplan);
            _db.TableItemGroup.Returns(_tableItemGroup);
            _db.TableItemSectionPlacement.Returns(_tablePlacement);
            _db.TableStoreSection.Returns(_tableStoreSection);

            //Creates uut
            _uut = new BLL(_searcher, _db);
        }

        [Test]
        public void GetFloorPlan_Called_TableFloorplanDownloadFloorplanCalled()
        {
            _uut.GetFloorPlan("test");
            _tableFloorplan.Received().DownloadFloorplan(Arg.Any<string>());
        }

        [Test]
        public void GetPresentationItemGroups_SearcherSearchReturnsOneItemWithPlacement_ReturnsOnePresentationItemGroup()
        {
            _searcher.Search(Arg.Any<string>()).Returns(new List<Item>() { new Item(0, "", 0) });
            _tableItemGroup.GetItemGroup(Arg.Any<long>()).Returns(new ItemGroup("TestGroup", 0, 0));
            _tablePlacement.FindPlacementsByItem(Arg.Any<long>()).Returns(new List<StoreSection>() {new StoreSection(0,"",0,0,0)});
            List<PresentationItemGroup> list =_uut.GetPresentationItemGroups("");
            Assert.That(list.Count, Is.EqualTo(1) );
        }

        [Test]
        public void GetPresentationItemGroups_SearcherSearchReturnsOneItemWithNoPlacement_ReturnsZeroPresentationItemGroups()
        {
            _searcher.Search(Arg.Any<string>()).Returns(new List<Item>() { new Item(0, "", 0) });
            _tableItemGroup.GetItemGroup(Arg.Any<long>()).Returns(new ItemGroup("TestGroup", 0, 0));
            List<PresentationItemGroup> list = _uut.GetPresentationItemGroups("");
            Assert.That(list.Count, Is.EqualTo(0));
        }

        [Test]
        public void GetPresentationItemGroups_SearcherSearchReturnsOneItemWithPlacement_ReturnsCorrectPresentationItemGroup()
        {
            _searcher.Search(Arg.Any<string>()).Returns(new List<Item>() { new Item(0, "TestItem", 0) });
            _tableItemGroup.GetItemGroup(Arg.Any<long>()).Returns(new ItemGroup("TestGroup", 0, 0));
            _tablePlacement.FindPlacementsByItem(Arg.Any<long>()).Returns(new List<StoreSection>() { new StoreSection(0, "", 3, 5, 0) });
            List<PresentationItemGroup> list = _uut.GetPresentationItemGroups("");

            Assert.That(list[0].Name, Is.EqualTo("TestGroup"));
            Assert.That(list[0].PresentationItems[0].Itemname, Is.EqualTo("TestItem"));
            var location = list[0].PresentationItems[0].ItemPlacementList[0];
            Assert.That(location.X==3 && location.Y == 5);
        }

        [Test]
        public void GetPresentationItemGroups_SearcherSearchReturnsTwoItemsWithSameGroup_ReturnsOnePresentationItemGroupWithTwoItems()
        {
            _searcher.Search(Arg.Any<string>()).Returns(new List<Item>() { new Item(0, "", 0), new Item(0,"",0) });
            _tableItemGroup.GetItemGroup(Arg.Any<long>()).Returns(new ItemGroup("TestGroup", 0, 0));
            _tablePlacement.FindPlacementsByItem(Arg.Any<long>()).Returns(new List<StoreSection>() { new StoreSection(0, "", 0, 0, 0) });
            List<PresentationItemGroup> list = _uut.GetPresentationItemGroups("");
            Assert.That(list.Count, Is.EqualTo(1));
            Assert.That(list[0].PresentationItems.Count, Is.EqualTo(2));
        }

        [Test]
        public void GetPresentationItemGroups_SearcherSearchReturnsTwoItemsWithDifferentGroup_ReturnsTwoPresentationItemGroupsWithOneItemEach()
        {
            _searcher.Search(Arg.Any<string>()).Returns(new List<Item>() { new Item(0, "", 1), new Item(0, "", 2) });
            _tableItemGroup.GetItemGroup(1).Returns(new ItemGroup("TestGroup1", 0, 0));
            _tableItemGroup.GetItemGroup(2).Returns(new ItemGroup("TestGroup2", 0, 0));
            _tablePlacement.FindPlacementsByItem(Arg.Any<long>()).Returns(new List<StoreSection>() { new StoreSection(0, "", 0, 0, 0) });
            List<PresentationItemGroup> list = _uut.GetPresentationItemGroups("");
            Assert.That(list.Count, Is.EqualTo(2));
            Assert.That(list[0].PresentationItems.Count, Is.EqualTo(1));
            Assert.That(list[1].PresentationItems.Count, Is.EqualTo(1));
        }
    }
}
