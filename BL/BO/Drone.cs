using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public Weight Weight { get; set; }
        public double Battery { get; set; } //charging level
        public DroneStatuses DroneStatuses { get; set; }
        public ParcelInTransfer ParcelInTransfer { get; set; }
        public Location Location { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Model is {Model}, \n";
            result += $"Drone weight is {Weight}, \n";
            result += $"Battery precent is: {Battery}, \n";
            result += $"Drone status is: {DroneStatuses}, \n";
            result += $"Drone's parcel in transfer is: {ParcelInTransfer}, \n";
            result += $"Drone's location is: {Location}, \n";

            return result;
        }
    }
}
