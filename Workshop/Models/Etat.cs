﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workshop.Models
{
    public class Etat
    {
        public int idEtat { get; set; }
        public Feu feu { get; set; }
        public Jour jour { get; set; }
        public Horaire horaire { get; set; }
        public int nbPassant { get; set; }
        public bool etat { get; set; }
    }
}