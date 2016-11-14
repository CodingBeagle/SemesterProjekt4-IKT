using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Services;
using BLL;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;

namespace LocatilesWebApp.Controllers
{
    public class HomeController : Controller
    {
        private DatabaseService _db = new DatabaseService(new SqlStoreDatabaseFactory());

        // GET: Home
        public ActionResult Index()
        {
            // Download floorplan
            _db.TableFloorplan.DownloadFloorplan(Server.MapPath("/Pictures/"));

            return View(); 
        }

        [HttpGet]
        public ActionResult SearchItems(string searchtext)
        {
            List<Item> searchResult = _db.TableItem.SearchItems(searchtext);

            ViewBag.Items = searchResult;

            var itemCoordinates = new List<List<StoreSection>>();
            foreach (var item in searchResult)
            {
                List<StoreSection> itemStoreSections = _db.TableItemSectionPlacement.FindPlacementsByItem(item.ItemID);

                itemCoordinates.Add(itemStoreSections);
            }

            ViewBag.ItemCoordinates = itemCoordinates;

            return View("Index");
        }
    }
}