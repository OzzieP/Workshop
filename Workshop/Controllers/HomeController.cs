using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workshop.Models;

using Workshop.ViewModels;

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

            FeuxViewModel viewModel = new FeuxViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult GestionImprevu(FormCollection formCollection)
        {
            try
            {
                Dictionary<String, Boolean> arr = new Dictionary<string, bool>();
                arr.Add("C1-VR1", false);
                arr.Add("C1-VR2", false);
                arr.Add("C1-HR1", true);
                arr.Add("C1-HR2", true);

                ViewBag.stateFeux = arr;

                var abc = Request.Form["C1-VR1"];
                var abc2 = Request.Form["C1-VR2"];
                var abc3 = Request.Form["C1-HR1"];
                var abc4 = Request.Form["C1-HR2"];

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
            DatabaseHelper database = new DatabaseHelper();
            List<Etat> lundi = database.SelectNombreVoitureParVoie(1);
            List<Etat> mardi = database.SelectNombreVoitureParVoie(2);
            List<Etat> mercredi = database.SelectNombreVoitureParVoie(3);
            List<Etat> jeudi = database.SelectNombreVoitureParVoie(4);
            List<Etat> vendredi = database.SelectNombreVoitureParVoie(5);
            List<Etat> samedi = database.SelectNombreVoitureParVoie(6);
            List<Etat> dimanche = database.SelectNombreVoitureParVoie(0);
            ViewBag.Message = "Page information.";
            ViewBag.lundi = lundi;
            ViewBag.mardi = mardi;
            ViewBag.mercredi = mercredi;
            ViewBag.jeudi = jeudi;
            ViewBag.vendredi = vendredi;
            ViewBag.samedi = samedi;
            ViewBag.dimanche = dimanche;


            return View();
        }

        /*public ActionResult CreateDatabase()
        {
            DatabaseHelper.ScriptInsertion();
            return null;
        }*/
    }
}