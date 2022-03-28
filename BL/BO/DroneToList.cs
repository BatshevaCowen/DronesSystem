using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class DroneToList
    {
        public int Id { get; set; }
        // public string model { get; set; }
        public string Model { get; set; }
        public Weight Weight { get; set; }
        public double Battery { get; set; }
        public DroneStatuses DroneStatuses { get; set; }
        public Location Location { get; set; }
        public int ParcelNumberTransferred { get; set; }//Parcel number transferred If there is 

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"Drone ID is: {Id}, \n";
            result += $"Drone Model is: {Model}, \n";
            result += $"Drone weight is: {Weight}, \n";
            result += $"Battery precent is: {Battery}, \n";
            result += $"Drone status is: {DroneStatuses}, \n";
            result += $"{Location} ";
            //only if there is parcel in transfer
            if(ParcelNumberTransferred!=0)
                result += $"Parcel's number in transfer {ParcelNumberTransferred}, \n";

            return result;
        }
    }
}
