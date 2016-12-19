using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace LocatilesWebApp.Models
{
    public class BLL : IBLL
    {
        private readonly ISearcher _searcher;
        private IDatabaseService _db;

        public BLL(ISearcher searcher, IDatabaseService db =null)
        {
            if(db == null)
                _db = new DatabaseService(new SqlStoreDatabaseFactory());
            else
                _db = db;
            
            _searcher = searcher;
        }
       
        public  List<PresentationItemGroup> GetPresentationItemGroups(string searchString)
        {

            List<PresentationItem> _presentationItems = new List<PresentationItem>();
            List<PresentationItemGroup> _presentationItemGroups = new List<PresentationItemGroup>();
            List<Item> _searchresultItems = _searcher.Search(searchString);

            // Creates and adds presentationsItems to list
            foreach (var i in _searchresultItems)
            {
                ItemGroup searchresultItemGroup = _db.TableItemGroup.GetItemGroup(i.ItemGroupID);
                List<StoreSection> itemStoreSections = _db.TableItemSectionPlacement.FindPlacementsByItem(i.ItemID);

                List<Point> itemPlacementList = new List<Point>();
                foreach (var section in itemStoreSections)
                {
                    itemPlacementList.Add(new Point ((int)section.CoordinateX, (int)section.CoordinateY));
                    
                }
                if(itemPlacementList.Any())
                    _presentationItems.Add(new PresentationItem(i.Name, searchresultItemGroup.ItemGroupName, itemPlacementList));

            }

            // Distributes presentationItems to PresentationItemsGroups with same itemgroup name
            foreach (var pi in _presentationItems)
            {
                var tempPIG = _presentationItemGroups.FirstOrDefault(g => g.Name == pi.Itemgroupname);

                // if there is no existing PresentationItemGroup with same Itemgroup name
                if (tempPIG == null)
                {
                    //Create new PresentationItemGroup
                    PresentationItemGroup PIG = new PresentationItemGroup(pi.Itemgroupname, new List<PresentationItem>() {pi});
                    _presentationItemGroups.Add(PIG);
                }
                else // if there is a PresentationItemGroup with same Itemgroup name
                {
                    tempPIG.PresentationItems.Add(pi);
                }
            }

            return _presentationItemGroups;
        }

        public void GetFloorPlan(string filepath)
        {
            _db.TableFloorplan.DownloadFloorplan(filepath);
        }

    }
}
