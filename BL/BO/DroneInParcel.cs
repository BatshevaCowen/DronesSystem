using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneInParcel
    {
        public int Id { get; set; }
        public double Battery { get; set; } // battery status
        public Location Location { get; set; } // current location
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"Drone ID is {Id}, \n";
            result += $"Battery precent is: {Battery}, \n";
            result += $"Drone location is {Location} \n";
            
            return result;
        }
    }
}
