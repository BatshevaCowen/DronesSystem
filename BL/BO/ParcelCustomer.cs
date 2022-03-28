using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class ParcelCustomer
    {
        public int Id { get; set; }
        public Weight Weight { get; set; }
        public Priority Priority { get; set; }
        public ParcelStatus ParcelStatus { get; set; }
        public CustomerInParcel CustomerInParcel { get; set; }
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $" {Id}, ";
            result += $" {Weight}, ";
            result += $" {Priority}, ";
            result += $"{ParcelStatus}, ";
            result += $"{CustomerInParcel}, ";

            return result;
        }
    }
}
