﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Workshop.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            Dictionary<String, Boolean> arr = new Dictionary<string, bool>();

            arr["C1-VR1"] = true;
            arr["C1-VR2"] = true;
            arr["C1-HP1"] = true;
            arr["C1-HP2"] = true;
            arr["C1-HR1"] = false;
            arr["C1-HR2"] = false;
            arr["C1-HV1"] = false;
            arr["C1-HV2"] = false;

            ViewBag.stateFeux = arr;

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
    }
}