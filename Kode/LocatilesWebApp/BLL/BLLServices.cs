using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using DatabaseAPI.TableItem;

namespace BLL
{
    public class ItemInfoBLL
    {
        private DatabaseService _db = new DatabaseService(new SqlStoreDatabaseFactory());

        public  List<PresentationItemGroup> GetPresentationItemGroups(string searchString)
        {

            List<PresentationItem> _presentationItems = new List<PresentationItem>();
            List<PresentationItemGroup> _presentationItemGroups = new List<PresentationItemGroup>();


            List<Item> _searchresultItems = _db.TableItem.SearchItems(searchString);

            foreach (var i in _searchresultItems)
            {
                ItemGroup searchresultItemGroup = _db.TableItemGroup.GetItemGroup(i.ItemGroupID);
                List<StoreSection> itemStoreSections = _db.TableItemSectionPlacement.FindPlacementsByItem(i.ItemID);

                List<Point> _coordinates = new List<Point>();
                foreach (var section in itemStoreSections)
                {
                    _coordinates.Add(new Point ((int)section.CoordinateX, (int)section.CoordinateY));
                    
                }
                _presentationItems.Add(new PresentationItem(i.Name, searchresultItemGroup.ItemGroupName, _coordinates));

            }

            foreach (var pi in _presentationItems)
            {
                var tempPIG = _presentationItemGroups.FirstOrDefault(g => g.Name == pi.Itemgroupname);
                if (tempPIG == null)
                {
                    PresentationItemGroup PIG = new PresentationItemGroup(pi.Itemgroupname, new List<PresentationItem>() {pi});
                    _presentationItemGroups.Add(PIG);
                }
                else
                {
                    tempPIG.PresentationItems.Add(pi);
                }
            }

            return _presentationItemGroups;
        }
    }

    public class FloorplanBLL
    {
        private DatabaseService _db = new DatabaseService(new SqlStoreDatabaseFactory());

        public void GetFloorPlan(string filepath)
        {
            _db.TableFloorplan.DownloadFloorplan(filepath);
        }

    }
}
