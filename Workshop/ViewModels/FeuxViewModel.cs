using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Workshop.Models;

namespace Workshop.ViewModels
{
    public class FeuxViewModel
    {
        public List<TempFeux> Feux { get; set; }


        public FeuxViewModel()
        {
            Feux = new List<TempFeux>
            {
                new TempFeux("C1-VR1", 0),
                new TempFeux("C1-VR2", 0),
                new TempFeux("C1-HR1", 0),
                new TempFeux("C1-HR2", 0)
            };
        }
    }
}