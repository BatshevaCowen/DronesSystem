using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BO
{
    public class CustomerInParcel
    {
        public int Id { get; set; }
        public String Name { get; set; }
        /// <summary>
        /// ToString
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            String result = "";
            result += $"Customer ID is: {Id}, \n";
            result += $"Customer name is {Name}, \n";
            
            return result;
        }
    }
}
