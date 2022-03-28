using System;

namespace DO
{
    public struct Parcel
    {
        public int Id { get; set; }
        public int SenderId { get; set; }
        public int ReceiverId { get; set; }
        public WeightCategories Weight { get; set; }
        public Priorities Priority { get; set; }
        public int DroneID { get; set; }
        public DateTime? Create { get; set; } //Time to create a package for the sender
        public DateTime? Assigned { get; set; } //Time to assign the package to the drone
        public DateTime? PickedUp { get; set; } //Time to pick up the package from the sender
        public DateTime? Supplied { get; set; } //Time of arrival of the package to the recipient

        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Sending customer is {SenderId}, \n";
            result += $"Reciving customer is {ReceiverId}, \n";
            result += $"Package weight is {Weight}, \n";
            result += $"Priority is {Priority}, \n";
            result += $"Drone ID is {DroneID}, \n";
            result += $"Creating package time is {Create}, \n";
            result += $"Package to drone affiliation time is {Assigned}, \n";
            result += $"Collecting package time is {PickedUp}, \n";
            result += $"Client reciving time is {Supplied}, \n";

            return result;
        }

    }

}
