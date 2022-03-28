using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;

namespace BL
{
    internal sealed partial class BL : IBL
    {
        /// <summary>
        /// Add customer
        /// </summary>
        /// <param name="customer"></param>
        /// <
        /// cref="NotImplementedException"></exception>
        public void AddCustomer(Customer customer)
        {
            // Customer ID must be 9 digits
            if (customer.Id < 100000000 || customer.Id >= 1000000000)
            {
                throw new CustomerIdExeption(customer.Id, "Customer ID must be 9 digits");
            }
            // Phone number is 10 digits (or 9 digits- for a telephone at home) + one "-"
            if (customer.Phone.Length != 10 && customer.Phone.Length != 11)
            {
                throw new PhoneException(customer.Phone, "Phone number is ilegal!");
            }
            // Phone number should start with a 0
            if (customer.Phone[0] != '0')
            {
                throw new PhoneException(customer.Phone, "Phone number should start with a 0");
            }
            DO.Customer c = new()
            {
                Id = customer.Id,
                Name = customer.Name,
                Phone = customer.Phone,
                Longitude = customer.Location.Longitude,
                Latitude = customer.Location.Latitude
                
            };
      
            c.User = new()
            {
                Password = customer.User.Password,
                UserName = customer.User.UserName,
                MyActivity = DO.Activity.On,
                Permission = DO.Permit.User
            };

            dalo.AddCustomer(c);
        }
        /// <summary>
        /// Update customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        public void UpdateCustomer(int id, string name, string phone)
        {
            dalo.UpdateCustumer(id, name, phone);
        }
      
        /// <summary>
        /// Get customer by ID
        /// </summary>
        /// <param name="IDc"></param>
        /// <returns></returns>
        public Customer GetCustomer(int IDc)
        {
            DO.Customer c = dalo.GetCustomer(IDc);
            Customer customer = new();
            customer.Id = c.Id;
            customer.Name = c.Name;
            customer.Phone = c.Phone;
            customer.Location = new()
            {
                Latitude = c.Latitude,
                Longitude = c.Longitude,
            };
            //Packages that the sending customer has

            List<DO.Parcel> parcelSendin = dalo.GetListOfParcelSending(customer.Id);
            List<DO.Parcel> parcelReciever = dalo.GetListOfParcelRecirver(customer.Id);
           
            foreach (DO.Parcel item in parcelSendin)
            {
                ParcelCustomer parcelCustomer = new ();
                parcelCustomer.Id = item.Id;
                parcelCustomer.Priority = (Priority)item.Priority;
                parcelCustomer.Weight = (Weight)item.Weight;
                if(item.Create==DateTime.MinValue)
                {
                    parcelCustomer.ParcelStatus = ParcelStatus.Created;
                }
                if (item.Supplied == DateTime.MinValue)
                {
                    parcelCustomer.ParcelStatus = ParcelStatus.Supplied;
                }
                if (item.PickedUp == DateTime.MinValue)
                {
                    parcelCustomer.ParcelStatus = ParcelStatus.PickedUp;
                }
                if (item.Assigned == DateTime.MinValue)
                {
                    parcelCustomer.ParcelStatus = ParcelStatus.Assigned;
                }
                parcelCustomer.CustomerInParcel = new()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                };
                
                //add Details of the sending customer
                customer.SentParcels = new();
                customer.SentParcels.Add(parcelCustomer);
            }
            foreach (DO.Parcel item in parcelReciever)
            {
                ParcelCustomer parcelCustomer = new()
                {
                    Id = item.Id,
                    Priority = (Priority)item.Priority,
                    Weight = (Weight)item.Weight
                };
                if (item.Create == DateTime.MinValue)
                {
                    parcelCustomer.ParcelStatus = ParcelStatus.Created;
                }
                if (item.Supplied == DateTime.MinValue)
                {
                    parcelCustomer.ParcelStatus = ParcelStatus.Supplied;
                }
                if (item.PickedUp == DateTime.MinValue)
                {
                    parcelCustomer.ParcelStatus = ParcelStatus.PickedUp;
                }
                if (item.Assigned == DateTime.MinValue)
                {
                    parcelCustomer.ParcelStatus = ParcelStatus.Assigned;
                }
                parcelCustomer.CustomerInParcel = new()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                };

                //add Details of the rciever customer
                customer.ReceiveParcels = new();
                customer.ReceiveParcels.Add(parcelCustomer);
            }
            return customer;
        }

        /// <summary>
        /// Show LIST of customers
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<CustomerToList> ShowCustomerList()
        {
            List<CustomerToList> customerList = new();
            int sentAndProvided = 0, sentNOTProvided = 0, parcelRecived = 0, parcelOnItsWay = 0;
            //get the list of CUSTOMERS from the dalobject (we need to return CustomerToList)
            var custumers = dalo.ShowCustomerList();
            foreach (var item in custumers)
            {
                Customer customer = new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    Phone = item.Phone
                };
                //Number of packages that the customer have sent and recived
                List<DO.Parcel> parcelSent = dalo.GetListOfParcelSending(customer.Id);
                List<DO.Parcel> parcelRecieved = dalo.GetListOfParcelRecirver(customer.Id);
                
                //parcel at customer- SENT parcels
                foreach (DO.Parcel item1 in parcelSent)
                {
                    ParcelCustomer parcelCustomer = new()
                    {
                        Id = item.Id,
                        Priority = (Priority)item1.Priority,
                        Weight = (Weight)item1.Weight
                    };
                    //finds the parcel status
                    parcelCustomer.ParcelStatus = FindParcelStatus(item1);
                    //counts the number of parcels that sent and provided/not provided
                    if (parcelCustomer.ParcelStatus == ParcelStatus.Supplied)
                        sentAndProvided++;
                    else if (parcelCustomer.ParcelStatus != ParcelStatus.Supplied)
                        sentNOTProvided++;

                    //customer in parcel- SENDER
                    parcelCustomer.CustomerInParcel = new()
                    {
                         Id = customer.Id,
                         Name=customer.Name,
                    };

                    //add Details of the SENDER
                    customer.SentParcels = new();
                    customer.SentParcels.Add(parcelCustomer);
                }

                //parcel at customer- parcels RECIVED
                foreach (DO.Parcel item2 in parcelRecieved)
                {
                    ParcelCustomer parcelCustomer = new()
                    {
                        Id = item.Id,
                        Priority = (Priority)item2.Priority,
                        Weight = (Weight)item2.Weight
                    };
                    //finds the parcel status
                    parcelCustomer.ParcelStatus = FindParcelStatus(item2);
                    if (parcelCustomer.ParcelStatus == ParcelStatus.Supplied)
                        parcelRecived++;
                    else if (parcelCustomer.ParcelStatus != ParcelStatus.Supplied)
                        parcelOnItsWay++;
                    //customer in parcel- RECIVER
                    parcelCustomer.CustomerInParcel = new()
                    {
                        Id = customer.Id,
                        Name=customer.Name,
                    };

                    //add Details of the rciever customer
                    customer.ReceiveParcels = new();
                    customer.ReceiveParcels.Add(parcelCustomer);
                }
                CustomerToList customerToList = new()
                {
                    Id = customer.Id,
                    Name = customer.Name,
                    Phone = customer.Phone,
                    SentAndProvidedParcels = sentAndProvided,
                    SentButNOTProvidedParcels = sentNOTProvided,
                    RecivedParcels = parcelRecived,
                    ParcelsOnTheWay = parcelOnItsWay
                };
                customerList.Add(customerToList);
            }
            return customerList;
        }

        

        /// <summary>
        /// Finds the customer by his user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Customer</returns>
        public DO.Customer GetCustomer_ByUsername(User user)
        {
            DO.User user1 = new DO.User()
            {
                UserName = user.UserName,
                Password = user.Password,
                Permission = DO.Permit.User,
                MyActivity = DO.Activity.On
            };
            DO.Customer customer = new DO.Customer();
            customer = dalo.GetCustomer_ByUsername(user1);
            return customer;
        }
    }
}
