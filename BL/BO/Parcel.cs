using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Parcel
    {
        public int Id { get; set; }
        public CustomerInParcel Sender { get; set; } //the sender of the parcel
        public CustomerInParcel Resiver { get; set; } //the person who recive the parcel
        public Weight Weight { get; set; } //wheight category
        public Priority Priority { get; set; } // parcel's priority
        public DroneInParcel DroneInParcel { get; set; }
        public DateTime? ParcelCreationTime { get; set; } // the time of the parcel's creation
        public DateTime? AssignmentToParcelTime { get; set; } // the time when the parcel have assigned
        public DateTime? CollectionTime { get; set; } // percel's collection time
        public DateTime? SupplyTime { get; set; } //parcel's supply time
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"Parcel ID is: {Id}, \n";
            result += $"Details about the customer who sent the parcel: {Sender}, \n";
            result += $"Details about the customer who recive the parcel: {Resiver}, \n";
            result += $"The parcel's weight is: {Weight}, \n";
            result += $"The parcel's priority is: {Priority}, \n";
            result += $"Details about the drone of the parcel: {DroneInParcel}, \n";
            result += $"The parcel's creation time: {ParcelCreationTime}, \n";
            //will print the next times only if happend already
            if(AssignmentToParcelTime!=DateTime.MinValue)
                result += $"The parcel's assignment is: {AssignmentToParcelTime}, \n";
            if (CollectionTime != DateTime.MinValue)
                result += $"The parcel's collection is: {CollectionTime}, \n";
            if (SupplyTime != DateTime.MinValue)
                result += $"The parcel's supply is: {SupplyTime}, \n";
            
            return result;
        }
    }
}
