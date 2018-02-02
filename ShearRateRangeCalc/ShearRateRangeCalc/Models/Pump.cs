using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq;

namespace ShearRateRangeCalc.Models
{
    public class Pump
    {
        public enum PumpType
        {
            Regular,
            Fast
        };

        public string  Name { get; set; }

        public PumpType Type { get; set; }

        //public static List<string> GetPumpTypes()
        //{
        //    List<string> s = new List<string>();
        //    foreach (PumpType p in Enum.GetValues(typeof(PumpType)))
        //    {
        //        s.Add(p.ToString());
        //    }
        //    return s;
        //}

        //public static PumpType GetPumpType(string s)
        //{
        //    foreach (PumpType p in Enum.GetValues(typeof(PumpType)))
        //    {
        //        if (s == p.ToString())
        //        {
        //            return p;
        //        }
        //    }
        //    return PumpType.Regular;
        //}

        public const double FAST_PUMP_FLOW_RATE_MIN = 2;
        public const double FAST_PUMP_FLOW_RATE_MAX = 40000;
        public const double REGULAR_PUMP_FLOW_RATE_MIN = 1;
        public const double REGULAR_PUMP_FLOW_RATE_MAX = 20000;

    }
}
