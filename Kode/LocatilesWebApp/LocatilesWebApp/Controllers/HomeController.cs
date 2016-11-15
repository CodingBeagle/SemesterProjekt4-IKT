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
        private FloorplanBLL _floorplan = new FloorplanBLL();
        private ItemInfoBLL _itemInfoBll = new ItemInfoBLL();

        // GET: Home
        public ActionResult Index()
        {
            // Download floorplan
            _floorplan.GetFloorPlan(Server.MapPath("/Pictures/"));


            return View(); 
        }

        [HttpGet]
        public ActionResult SearchItems(string searchtext)
        {

            List<PresentationItemGroup> searchResult = _itemInfoBll.GetPresentationItemGroups(searchtext);

            ViewBag.Items = searchResult;



            return View("Index");
        }
    }
}