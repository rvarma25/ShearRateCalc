using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShearRateRangeCalc.Models
{
    public class Chip
    {
        /// <summary>
        /// Like A, B,  C etc
        /// </summary>
        public string PressureSensorType { get; set; }
        /// <summary>
        /// 02, 05, 10, 20, 30 
        /// 02 refers to 20 μm, 05 to 50 μm
        /// </summary>
        public int ChannelDepth { get; set; } 
        /// <summary>
        /// Min Measurable Shear Stress
        /// </summary>
        public double TauMin { get; set; }
        /// <summary>
        /// Max Measurable Shear Stress
        /// </summary>
        public double TauMax{ get; set; }
        /// <summary>
        /// Conversion Factor shear rate (1/s) to flow rate ratio (μl/min)
        /// </summary>
        public double K { get; set; }

        public string ChipType
        {
            get
            {
                return PressureSensorType + ChannelDepth.ToString("00"); ;
            }
        }
        /// <summary>
        /// This is 1/10 th of channel depth, so like 2 μm for 20 μm depth
        /// </summary>
        public int MaxParticleSize
        {
            get
            {
                return ChannelDepth ;
            }
        }
    }
}
