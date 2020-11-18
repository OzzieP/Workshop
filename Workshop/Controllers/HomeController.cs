using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workshop.Models;

namespace Workshop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Boolean[] stateFeux = new Boolean[] { true, false, true, false };
            ViewBag.stateFeux = stateFeux;
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

        public ActionResult Information()
        {
            ViewBag.Message = "Page information.";

            return View();
        }

        /*public ActionResult CreateDatabase()
        {
            DatabaseHelper.ScriptInsertion();
            return null;
        }*/
    }
}