using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Workshop.MachineLearning
{
    public class ModelOutput
    {
        public int[] ForecastedPassants { get; set; }

        public int[] LowerBoundPassants { get; set; }

        public int[] UpperBoundPassants { get; set; }
    }
}