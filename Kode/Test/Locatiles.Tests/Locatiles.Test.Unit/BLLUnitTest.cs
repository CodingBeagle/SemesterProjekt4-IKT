using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DatabaseAPI;
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
            LoadBLL();
            _uut.GetFloorPlan("test");
            _tableFloorplan.Received().DownloadFloorplan(Arg.Any<string>());
        }
    }
}
