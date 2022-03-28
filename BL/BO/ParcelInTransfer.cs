using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelInTransfer
    {
        public int Id { get; set; }
        public ParcelTransferStatus ParcelTransferStatus { get; set; }
        public Priority Priority { get; set; }
        public Weight Weight { get; set; }
        public CustomerInParcel Sender { get; set; }
        public CustomerInParcel Reciver { get; set; }
        public Location CollectingLocation { get; set; }
        public Location SupplyTargetLocation { get; set; }
        public Double TransportDistance { get; set; } //the dictance of the transportation

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"Parcel ID is: {Id}, \n";
            result += $"The Parcel transfer status is: {ParcelTransferStatus}, \n";
            result += $"The parcel's priority is: {Priority}, \n";
            result += $"The parcel's weight is: {Weight}, \n";
            result += $"The parcel's sender is: {Sender}, \n";
            result += $"The parcel's Reciver is: {Reciver}, \n";
            result += $"The location of parcel's collection is: ({SexagesimalAngle.FromDouble(CollectingLocation.Latitude)}, {SexagesimalAngle.FromDouble(CollectingLocation.Longitude)}), \n";
            result += $"The location to supply the parcel is: {SupplyTargetLocation}, \n";
            result += $"The dictance of the transportation is: {TransportDistance}, \n";
            return result;
        }
    }
}
