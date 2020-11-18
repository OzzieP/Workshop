using System;
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

            arr.Add("C1-VR1", true);
            arr.Add("C1-VR2", true);
            arr.Add("C1-HR1", false);
            arr.Add("C1-HR2", false);

            ViewBag.stateFeux = arr;
            return View();
        }

        [HttpPost]
        public ActionResult GestionImprevu(FormCollection collection)
        {
            try
            {
                Dictionary<String, Boolean> arr = new Dictionary<string, bool>();
                arr.Add("C1-VR1", false);
                arr.Add("C1-VR2", false);
                arr.Add("C1-HR1", true);
                arr.Add("C1-HR2", true);

                ViewBag.stateFeux = arr;

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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