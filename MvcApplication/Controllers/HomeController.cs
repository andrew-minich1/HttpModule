using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Product()
        {
            return View();
        }
        public ActionResult AboutUs()
        {
            return View();
        }

        [Authorize]
        public ActionResult Career()
        {
            return View();
        }

    }
}
