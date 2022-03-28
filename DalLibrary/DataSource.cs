using DO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace DAL
{
    public static class DataSource
    {
        /// <summary>
        /// database of DO entities
        /// </summary>
        public static int OrdinalNumber  = 1000000;
        public static List<Drone> Drones { get; set; } = new(5) { };
        public static List<Station> Stations { get; set; } = new(5) { };
        public static List<Customer> Customer { get; set; } = new(100) { };
        public static List<Parcel> Parcels { get; set; } = new(1000) { };
        public static List<DroneCharge> DroneCharges { get; set; } = new(10) { };
        public static List<User>  userList;

        static Random r = new() { };

        //public static object Station { get; set; }

        internal class Config
        {
            internal static double Light { get => 10; }
            internal static double Available { get => 1; }
            internal static double Medium { get => 50; }
            internal static double Heavy { get => 150; }
            internal static double ChargingRate { get => 10.25; }
        }
        public static DateTime MyDateTime()
        {
            DateTime myDateTime = new(r.Next(0, 60), 0) { };
            return myDateTime;
        }

        /// <summary>
        /// create list of all users
        /// </summary>
        /// <returns>list of all users</returns>
        private static List<User> CreateUsers()
        {
            List<User> users = new List<User> { };
            List<string> names = new List<string> { "shira", "lea", "rachel", "avraham", "david", "dani", "oshri", "eliezer", "avraham", "itamar" };
            for (int i = 1; i < 11; i++)
            {
                users.Add(new User { UserName = names[i - 1], Password = "1234", Permission = Permit.User, MyActivity = Activity.On });
            }
            users.Add(new User { UserName = "shirel", Password = "shirel", Permission = Permit.Admin, MyActivity = Activity.On });
            users.Add(new User { UserName = "batsheva", Password = "batsheva", Permission = Permit.Admin, MyActivity = Activity.On });
            return users;
        }

        /// <summary>
        /// Initialize all the lists
        /// </summary>
        public static void Initialize()
        {
            // names of different entities 
            string[] arrDroneModel = new string[5] { "Drone1", "Drone2", "Drone3", "Drone4", "Drone5" };
            string[] arrStation = new string[2] { "station1", "station2" };
            string[] arrClientFirstName = new string[10] { "shira", "lea", "rachel", "avraham", "david", "dani", "oshri", "eliezer", "avraham", "itamar" };

            //adding drones
            for (int i = 1; i <= 5; i++)
            {
                Drones.Add(new Drone()
                {
                    Id = r.Next(1000, 9999), //4-9 digit
                    Model = arrDroneModel[i - 1],
                    MaxWeight = RandomEnumValue<WeightCategories>(),
                    Battery = r.Next(0, 100),
                });
            }

            //adding stations
            for (int i = 1; i <= 2; i++)
            {
                Stations.Add(new Station()
                {
                    Id = r.Next(10000, 100000), //5-6 digits
                    Name = arrStation[i - 1],
                    ChargeSpots = r.Next(1, 100),
                    Longitude = r.Next(-180, 179) + r.NextDouble(),
                    Latitude = r.Next(-90, 89) + r.NextDouble(),
                });
            }
            //TEST
            Stations.Add(new Station()
            {
                Id = 12345,
                Name = "miki",
                ChargeSpots = 10,
                Longitude = 33.3,
                Latitude = 44.4
                
            });
            //DroneCharge droneCharge = new()
            //{
            //    DroneId = 123456,
            //    StationId=12345,
            //};
            //DataSource.DroneCharges.Add(droneCharge);

            //adding customers
            for (int i = 0; i < 10; i++)
            {
                Customer.Add(new Customer()
                {
                    Id = r.Next(100000000,999999999),
                    Name = arrClientFirstName[i],
                    Phone = "05" + r.Next(0, 8) + "-" + r.Next(1000000, 9999999),
                    Longitude = r.Next(-180, 179) + r.NextDouble(),
                    Latitude = r.Next(-90, 89) + r.NextDouble(),
                    User = new User() 
                    { 
                        UserName= CreateUsers().ToArray()[i].UserName,
                        Password= CreateUsers().ToArray()[i].Password,
                        MyActivity= CreateUsers().ToArray()[i].MyActivity,
                        Permission= CreateUsers().ToArray()[i].Permission,
                    }
                });
            }
            for (int i = 0; i <= 4; i++)
            {
                //choose two different ids for sender and target from Customer's id
                int senderId = Customer[i].Id;
                int targetId;
                // the sender can't be the reciver
                do
                {
                    targetId = Customer[i+1].Id;
                } while (targetId == senderId);

                Parcels.Add(new Parcel()
                {
                    Id = ++OrdinalNumber,    //serial number
                    SenderId = senderId,
                    ReceiverId = targetId,
                    Weight = RandomEnumValue<WeightCategories>(),
                    Priority = RandomEnumValue<Priorities>(),
                    DroneID = Drones[i].Id,
                    Create = MyDateTime(),
                    Assigned = MyDateTime(),
                    PickedUp = MyDateTime(),
                    Supplied = MyDateTime()
                });

            }
            //adding parcels
            for (int i = 5; i <= 8; i++)
            {
                if (i != 8)
                {
                    //choose two different ids for sender and target from Customer's id
                    int senderId = Customer[i].Id;
                    int targetId;
                    // the sender can't be the reciver
                    do
                    {
                        targetId = Customer[i + 1].Id;
                    } while (targetId == senderId);

                    Parcels.Add(new Parcel()
                    {
                        Id = ++OrdinalNumber,    //serial number
                        SenderId = senderId,
                        ReceiverId = targetId,
                        Weight = RandomEnumValue<WeightCategories>(),
                        Priority = RandomEnumValue<Priorities>(),
                       
                        Create = DateTime.MinValue,
                        Assigned = DateTime.MinValue,
                        PickedUp = DateTime.MinValue,
                        Supplied = DateTime.MinValue
                    });

                }
                else
                {
                    Parcels.Add(new Parcel()
                    {
                        Id = ++OrdinalNumber,    //serial number
                        SenderId = Customer[8].Id,
                        ReceiverId = Customer[9].Id,
                        Weight = RandomEnumValue<WeightCategories>(),
                        Priority = RandomEnumValue<Priorities>(),
                       
                        Create = DateTime.MinValue,
                        Assigned = DateTime.MinValue,
                        PickedUp = DateTime.MinValue,
                        Supplied = DateTime.MinValue,
                    });
                }
            }
          

                userList = CreateUsers();
        }


        /// <summary>
        /// function for random enums
        /// from https://stackoverflow.com/questions/3132126/how-do-i-select-a-random-value-from-an-enumeration
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static T RandomEnumValue<T>()
        {
            var v = Enum.GetValues(typeof(T));
            return (T)v.GetValue(r.Next(v.Length));
        }
    }
}