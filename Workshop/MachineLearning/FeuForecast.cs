using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workshop.MachineLearning
{
    public class FeuForecast
    {
        public string Jour { get; set; }

        public float Semaine { get; set; }

        public float Heure { get; set; }

        public float PassantsActuel { get; set; }

        public float EstimationInferieure { get; set; }

        public float EstimationSuperieure { get; set; }

        public float Estimation { get; set; }

        public FeuForecast()
        {

        }

        public FeuForecast(string jour, float semaine, float heure, float passantsActuel, float estimationInferieure, float estimation, float estimationSuperieure)
        {
            Jour = jour;
            Semaine = semaine;
            Heure = heure;
            PassantsActuel = passantsActuel;
            EstimationInferieure = estimationInferieure;
            Estimation = estimation;
            EstimationSuperieure = estimationSuperieure;
        }
    }
}