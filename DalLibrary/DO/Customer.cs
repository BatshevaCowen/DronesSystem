using System;
namespace DO
{
    public struct Customer
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public User User { get; set; } //username and password of the customer to the system

        public override string ToString()
        {
            String result = "";
            result += $"ID is {Id}, \n";
            result += $"Name is {Name}, \n";
            result += $"Telephone is {Phone.Substring(0, 3) + "-" + Phone.Substring(3)}, \n";

            //---BONOS OPTION---
            result += $"Latitude is {Latitude}, \n Sexagesimal angle: {SexagesimalAngle.FromDouble(Latitude)}\n";
            result += $"Longitude is {Longitude}, \n Sexagesimal angle: {SexagesimalAngle.FromDouble(Longitude)}\n";

            return result;
        }
    }
}