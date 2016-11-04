using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using BLL;

namespace LocatilesWebApp.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            Class1 c1 = new Class1();
            string testString = c1.getItem();
            return View((Object)testString);
        }
    }
}