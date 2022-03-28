using System;
using System.Collections.Generic;


namespace DO
{
    public struct Station
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int ChargeSpots { get; set; }
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Name is {Name}, \n";
            //Location ---BONOS OPTION---
            result += $"Location is {SexagesimalAngle.FromDouble(Latitude)}, {SexagesimalAngle.FromDouble(Longitude)}\n";
            result += $"Number of available charging spots: {ChargeSpots}, \n";

            return result;
        }


    }
}