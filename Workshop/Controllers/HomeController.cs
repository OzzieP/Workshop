using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Workshop.Models;
using System.Web.Script.Serialization;

using Workshop.ViewModels;
using Workshop.MachineLearning;
using System.Globalization;
using System.Diagnostics;

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

        public string RefreshChart()
        {
            DatabaseHelper database = new DatabaseHelper();
            Dictionary<String, Etat> arr = new Dictionary<string, Etat>();

            arr.Add("C1-VR1", database.GetEtatFeu("C1-VR1"));
            arr.Add("C1-VR2", database.GetEtatFeu("C1-VR2"));
            arr.Add("C1-HR1", database.GetEtatFeu("C1-HR1"));
            arr.Add("C1-HR2", database.GetEtatFeu("C1-HR2"));
            var json = new JavaScriptSerializer().Serialize(arr);
            return json;
        }

        public string GetPrev(int idFeu)
        {
            Debug.Write("Debugtest");
            Debug.Write("Mon id feu  :" + idFeu);
            var currentCulture = new CultureInfo("fr-FR");
            var weekNo = currentCulture.Calendar.GetWeekOfYear(
            DateTime.Now,
            currentCulture.DateTimeFormat.CalendarWeekRule,
            currentCulture.DateTimeFormat.FirstDayOfWeek);
            
            DatabaseHelper database = new DatabaseHelper();
            ModelBuilder.CreateModel(idFeu, weekNo);
            List<FeuForecast> feux = ModelBuilder.FeuForecasts;

            var json = new JavaScriptSerializer().Serialize(feux);
            return json;
        }

        [HttpPost]
        public ActionResult GestionImprevu(FormCollection formCollection)
        {
            try
            {
                DatabaseHelper database = new DatabaseHelper();
                /*Boolean lastFiveMinuteFalse = true;*/

                int C1_VR1 = Request.Form["C1-VR1"] != "" ? int.Parse(Request.Form["C1-VR1"]) : 0;
                int C1_VR2 = Request.Form["C1-VR2"] != "" ? int.Parse(Request.Form["C1-VR2"]) : 0;
                int C1_HR1 = Request.Form["C1-HR1"] != "" ? int.Parse(Request.Form["C1-HR1"]) : 0;
                int C1_HR2 = Request.Form["C1-HR2"] != "" ? int.Parse(Request.Form["C1-HR2"]) : 0;

                int heure = DateTime.Now.Hour;
                int minute = DateTime.Now.Minute;
                int jour = (int)DateTime.Today.DayOfWeek;
                DateTime dateTime = new DateTime(2000, 01, 01, heure, minute, 0);

                if ((C1_VR1 > C1_HR1 && C1_VR1 > C1_HR2) || (C1_VR2 > C1_HR1 && C1_VR2 > C1_HR2))
                {
                    /* lastFiveMinuteFalse = database.LastFiveMinuteFalse("C1-HR1");*/
                    database.SetEtatFeu("C1-VR1", 1, dateTime, jour, C1_VR1);
                    database.SetEtatFeu("C1-VR2", 1, dateTime, jour, C1_VR2);
                    database.SetEtatFeu("C1-HR1", 0, dateTime, jour, C1_HR1);
                    database.SetEtatFeu("C1-HR2", 0, dateTime, jour, C1_HR2);
                } 
                else if ((C1_HR1 > C1_VR1 && C1_HR1 > C1_VR2) || (C1_HR2 > C1_VR1 && C1_HR2 > C1_VR2))
                {
                    /*lastFiveMinuteFalse = database.LastFiveMinuteFalse("C1-VR1");*/
                    database.SetEtatFeu("C1-VR1", 0, dateTime, jour, C1_VR1);
                    database.SetEtatFeu("C1-VR2", 0, dateTime, jour, C1_VR2);
                    database.SetEtatFeu("C1-HR1", 1, dateTime, jour, C1_HR1);
                    database.SetEtatFeu("C1-HR2", 1, dateTime, jour, C1_HR2);
                }
                else
                {
                    database.SetnbPassant("C1-VR1", dateTime, jour, C1_VR1);
                    database.SetnbPassant("C1-VR2", dateTime, jour, C1_VR2);
                    database.SetnbPassant("C1-HR1", dateTime, jour, C1_HR1);
                    database.SetnbPassant("C1-HR2", dateTime, jour, C1_HR2);
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
            ViewBag.feu = database.GetListFeu();


            return View();
        }
        
        [HttpPost]
        public ActionResult GenerateForecast()
        {


            return View();
        }

        /*public ActionResult CreateDatabase()
        {
            DatabaseHelper.ScriptInsertion();
            return null;
        }*/
    }
}