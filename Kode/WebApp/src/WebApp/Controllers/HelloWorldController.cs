using Microsoft.AspNetCore.Mvc;
using System.Text.Encodings.Web;

namespace WebApp.Controllers
{
    public class HelloWorldController : Controller
    {
        // 
        // GET: /HelloWorld/

        public IActionResult Index()
        {
            return View();
        }

        // 
        // GET: /HelloWorld/Welcome/ 

        public IActionResult Welcome(string name, int num = 1)
        {
            ViewData["Message"] = "Hello " + name;
            ViewData["Num"] = num;
            return View();
        }
    }
}