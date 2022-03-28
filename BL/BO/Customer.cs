using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class Customer
    {
        public int Id { get; set; }
        public String Name { get; set; }
        public String Phone { get; set; }
        public Location Location { get; set; }
        public List<ParcelCustomer> SentParcels { get; set; }//List of packages of the sender
        public List<ParcelCustomer> ReceiveParcels { get; set; }//List of packages of the reciver
        public User User { get; set; } //username and password of the customer to the system

        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"Customer ID is: {Id}, \n";
            result += $"Customer name is {Name}, \n";
            result += $"Customer phone number is: {Phone}, \n";
            result += $"Customer Location is: {Location}, \n";
            result += $"List of parcels from the sender: {SentParcels}, \n";
            result += $"List of parcels to the reciver: {ReceiveParcels}, \n";
            
            return result;
        }
    }

    /// <summary>
    /// exception
    /// </summary>
    [Serializable]
    internal class CustomerIdExeption : Exception
    {
        private int id;
        private string v;

        public CustomerIdExeption()
        {
        }

        public CustomerIdExeption(string message) : base(message)
        {
        }
        /// <summary>
        /// Exception- Customer ID must be 9 digits
        /// Exception- Sender ID can't be like Reciver ID
        /// </summary>
        /// <param name="id"></param>
        /// <param name="v"></param>
        public CustomerIdExeption(int id, string v)
        {
            this.id = id;
            this.v = v;
        }

        public CustomerIdExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomerIdExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
