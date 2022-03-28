using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class StationToList
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public int AvailableChargingSpots { get; set; } // the number of available charging spots
        public int UnavailableChargingSpots { get; set; } // the number of unavailable charging spots
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"Station ID is: {Id}, \n";
            result += $"Station name is {Name}, \n";
            result += $"The number of available charging spots is: {AvailableChargingSpots}, \n";
            result += $"The number of unavailable charging spots is: {UnavailableChargingSpots}, \n";
            return result;
        }
    }
}
