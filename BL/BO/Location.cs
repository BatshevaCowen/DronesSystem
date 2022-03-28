using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Location
    {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"({SexagesimalAngle.FromDouble(Latitude)}, {SexagesimalAngle.FromDouble(Longitude)})\n";
            return result;
        }
    }
}
