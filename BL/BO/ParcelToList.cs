using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelToList
    {
        public int Id { get; set; }
        public String SenderName { get; set; } //the name of the customer who sent the parcel
        public String ReciverName { get; set; } //the name of the customer who recived the parcel
        public Weight Weight { get; set; }
        public Priority Priority { get; set; }
        public ParcelStatus ParcelStatus { get; set; }
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"Parcel ID is {Id}, \n";
            result += $"The sender's name is: {SenderName}, \n";
            result += $"The reciver's name is: {ReciverName}, \n";
            result += $"The parcel's weight is: {Weight}, \n";
            result += $"The parcel's priority is: {Priority}, \n";
            result += $"The parcel's status is: {ParcelStatus}, \n";
            return result;
        }
    }
}
