using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DO;


namespace DAL
{
    internal sealed  partial class DalObject : DalApi.IDal
    {
        #region Coustumer
        /// <summary>
        /// add Customer to the Customers list
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public void AddCustomer(Customer c)
        {
            if (DataSource.Customer.Exists(x => x.Id == c.Id))
            {
                throw new CustomerException($"ID {c.Id} already exists!!");
            }
            else
                DataSource.Customer.Add(c);
        }
        /// <summary>
        /// update customer name and phone---
        /// </summary>
        /// <param name="custumerId"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void UpdateCustumer(int custumerId, string name, string phone)
        {
            if (!DataSource.Customer.Exists(x => x.Id == custumerId))
            {
                throw new Exception($"custumer {custumerId} is not exite!!");
            }
            Customer customer = DataSource.Customer.Find(x => x.Id == custumerId);
            DataSource.Customer.Remove(customer);
            customer.Name = name;
            customer.Phone = phone;
            DataSource.Customer.Add(customer);
        }
        /// <summary>
        /// Get Customer by id
        /// </summary>
        /// <param name="id"></param>
        public Customer GetCustomer(int IDc)
        {
            //if ID does not exist for customer
            if (!DataSource.Customer.Exists(item => item.Id == IDc))
            {
                throw new CustomerException($"ID: {IDc} does not exist!!");
            }
            return DataSource.Customer.First(c => c.Id == IDc);
        }

        /// <summary>
        /// Show list of Customers
        /// </summary>
        public IEnumerable<Customer> ShowCustomerList(Func<Customer,bool> predicate = null)
        {
            if (predicate == null)
            {
                List<Customer> CustomerList = new();
                foreach (Customer element in DataSource.Customer)
                {
                    CustomerList.Add(element);
                }
                return CustomerList;
            }
            else
                return DataSource.Customer.Where(predicate).ToList();
        }
        /// <summary>
        /// Finds the customer by his user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Customer</returns>
        public Customer GetCustomer_ByUsername(User user)
        {
            //if Username does not exist for customer
            if (!DataSource.Customer.Exists(item => item.User.UserName == user.UserName))
            {
                throw new CustomerException($"User: {user} does not exist!!");
            }
            return DataSource.Customer.First(c => c.User.UserName == user.UserName);
        }
        
        #endregion
    }
}
