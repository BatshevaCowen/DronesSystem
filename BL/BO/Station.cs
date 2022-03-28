using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Location Location { get; set; }
        public int AvailableChargingSpots { get; set; } //Number of available charging spots in free
        public List<DroneInCharging> droneInChargings= new(); //list of drones in charging

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"Station ID is {Id}, \n";
            result += $"Name: {Name}, \n";
            result += $"Location:\n{Location}";
            result += $"Number of Available charging spots in the station: {AvailableChargingSpots}, \n";
            result += $"List of drones in charging: \n";
            foreach (DroneInCharging item in droneInChargings)
            {
                result += $"{item} \n";
            }
            return result;
        }

    }
   
}

