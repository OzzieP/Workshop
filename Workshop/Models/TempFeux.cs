using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workshop.Models
{
    public class TempFeux
    {
        public string Matricule { get; set; }

        public int Voitures { get; set; }

        public TempFeux(string matricule, int voitures)
        {
            Matricule = matricule;
            Voitures = voitures;
        }
    }
}