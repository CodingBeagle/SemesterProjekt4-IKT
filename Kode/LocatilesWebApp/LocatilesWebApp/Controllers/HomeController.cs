using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Web.Mvc;
using System.Web.Services;
using DatabaseAPI;
using DatabaseAPI.DatabaseModel;
using DatabaseAPI.Factories;
using LocatilesWebApp.Models;


namespace LocatilesWebApp.Controllers
{
    public class HomeController : Controller
    {
        private IBLL _bll;
        private Model _model;
        
        public HomeController()
        {
            _bll = new BLL( new SearchOptimizer());
            _model = new Model();
        }

        public HomeController(   IBLL bll)
        {
            _bll = bll;
            _model = new Model();
        }


        // GET: Home
        public ActionResult Index()
        {
            // Download floorplan
            _bll.GetFloorPlan(Server.MapPath("/Pictures/"));

            return View(); 
        }

        [HttpGet]
        public ActionResult SearchItems(string searchtext)
        {
            // Get Presentation Item Groups
            _model.PresentationItemGroups = _bll.GetPresentationItemGroups(searchtext);

            // Load Viewbag with Presentation Item Groups
            ViewBag.PresentationGroup = _model.PresentationItemGroups;

            return View("Index");
        }
    }
}