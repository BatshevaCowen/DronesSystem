using System;

namespace DO
{
    public struct Drone
    {
        public int Id { get; set; }
        public string Model { get; set; }
        public WeightCategories MaxWeight { get; set; }
        public double Battery { get; set; } //charging level
        public DroneStatuses Status { get; set; }

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Model is {Model}, \n";
            result += $"Drone weight is {MaxWeight}, \n";
            result += $"Battery precent is: {Battery}, \n";
            result += $"Drone state is {Status}, \n";

            return result;
        }

    }
}