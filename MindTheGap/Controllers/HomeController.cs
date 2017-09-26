using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MindTheGap.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult FAQ()
        {
            ViewBag.Message = "Frequently Asked Questions.";

            return View();
        }
        public ActionResult ProfilePage()
        {
            ViewBag.Message = "User Profile Page.";

            return View();
        }

    }

}