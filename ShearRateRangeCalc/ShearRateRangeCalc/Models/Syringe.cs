using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShearRateRangeCalc.Models
{
    public class Syringe
    {
        public string Size { get; set; }
        /// <summary>
        /// in μl/min
        /// </summary>
        public double RegularPumpMinFlowRate { get; set; }

        public double RegularPumpMaxFlowRate { get; set; }

        public double FastPumpMinFlowRate { get; set; }

        public double FastPumpMaxFlowRate { get; set; }
    }
}
