﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using DatabaseAPI.TableItem;

namespace LocatilesWebApp.Models
{
    public class BLL : IBLL
    {
        private DatabaseService _db = new DatabaseService(new SqlStoreDatabaseFactory());


        public List<string> SearchOptimization(string searchString)
        {
            List<string> _searchList = new List<string>(); 
            string[] sa = searchString.Split(' ');
            for (int s = 0; s < sa.Length; s++)
            {
                _searchList.Add(sa[s]);
                
            }
            return _searchList;
        }
        public  List<PresentationItemGroup> GetPresentationItemGroups(List<string> searchStringList)
        {

            List<PresentationItem> _presentationItems = new List<PresentationItem>();
            List<PresentationItemGroup> _presentationItemGroups = new List<PresentationItemGroup>();


            List<Item> _searchresultItems = new List<Item>();

            foreach (var s in searchStringList)
            {
                List<Item> _searchWord = _db.TableItem.SearchItems(s);
                _searchresultItems.AddRange(_searchWord);

            }

            foreach (var i in _searchresultItems)
            {
                ItemGroup searchresultItemGroup = _db.TableItemGroup.GetItemGroup(i.ItemGroupID);
                List<StoreSection> itemStoreSections = _db.TableItemSectionPlacement.FindPlacementsByItem(i.ItemID);

                List<Point> itemPlacementList = new List<Point>();
                foreach (var section in itemStoreSections)
                {
                    itemPlacementList.Add(new Point ((int)section.CoordinateX, (int)section.CoordinateY));
                    
                }
                _presentationItems.Add(new PresentationItem(i.Name, searchresultItemGroup.ItemGroupName, itemPlacementList));

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

        public void GetFloorPlan(string filepath)
        {
            _db.TableFloorplan.DownloadFloorplan(filepath);
        }

    }
}
