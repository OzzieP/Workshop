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
            DatabaseHelper database = new DatabaseHelper();
            Dictionary<String, Etat> arr = new Dictionary<string, Etat>();

            arr.Add("C1-VR1", database.GetEtatFeu("C1-VR1"));
            arr.Add("C1-VR2", database.GetEtatFeu("C1-VR2"));
            arr.Add("C1-HR1", database.GetEtatFeu("C1-HR1"));
            arr.Add("C1-HR2", database.GetEtatFeu("C1-HR2"));

            ViewBag.stateFeux = arr;

            FeuxViewModel viewModel = new FeuxViewModel();

            return View(viewModel);
        }

        [HttpPost]
        public ActionResult GestionImprevu(FormCollection formCollection)
        {
            try
            {
                DatabaseHelper database = new DatabaseHelper();
                Boolean lastFiveMinuteFalse = true;

                int C1_VR1 = int.Parse(Request.Form["C1-VR1"]);
                int C1_VR2 = int.Parse(Request.Form["C1-VR2"]);
                int C1_HR1 = int.Parse(Request.Form["C1-HR1"]);
                int C1_HR2 = int.Parse(Request.Form["C1-HR2"]);

                //je vérifie que c=ca fait pas 5 minutes de rouge
                //si pas 5 minutes de rouge -> celui qui a le plus de voiture est vert ainsi que son opposé
                //par conséquence les autres passent sont rouge
                //si plus de 5 minutes -> celui qui a le plus de voiture passe rouge pendant la minute
                //par conséquence les autres passent au vert

                if (C1_VR1 > C1_HR1 && C1_VR1 > C1_HR2)
                {
                    lastFiveMinuteFalse = database.LastFiveMinuteFalse("C1-HR1");
                } 
                else if (C1_VR2 > C1_HR1 && C1_VR2 > C1_HR2)
                {
                    lastFiveMinuteFalse = database.LastFiveMinuteFalse("C1-HR1");
                } 
                else if ((C1_HR1 > C1_VR1 && C1_HR1 > C1_VR2))
                {
                    lastFiveMinuteFalse = database.LastFiveMinuteFalse("C1-VR1");
                } 
                else if (C1_HR2 > C1_VR1 && C1_HR2 > C1_VR2)
                {
                    lastFiveMinuteFalse = database.LastFiveMinuteFalse("C1-VR1");
                }

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
            List<Etat> etats = database.SelectNombreVoitureParVoie();
            ViewBag.Message = "Page information.";
            ViewBag.data = etats;

            return View();
        }

        /*public ActionResult CreateDatabase()
        {
            DatabaseHelper.ScriptInsertion();
            return null;
        }*/
    }
}