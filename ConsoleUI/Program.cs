using DalApi;
using DO;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleUI
{
    class Program
    {
        enum MenuOptions { Add = 1, Update, Show_One, Show_List, Calculate_distance, Exit }
        enum EntityOptions { Station = 1, Drone, Custumer, Parcel, Exit }
        enum UpdateOptions { Assignement = 1, Pickedup, Delivery, Recharge, Discharge, Exit }
        enum ListOptions { Station = 1, Drone, Custumer, Parcel, UnAsignementParcel, AvailbleChagingStation, Exit }
        enum DistanceOptions { From_Station = 1, From_Customer, Exit }

        private static void ShowMenu()
        {
            MenuOptions menuOptions;
            EntityOptions entityOptions;
            UpdateOptions updateOptions;
            DalApi.IDal dalobject = DalApi.DLFactory.GetDL();
            do
            {
                Console.WriteLine("WELCOME!");
                Console.WriteLine("option:\n 1- Add,\n 2- Update,\n 3- Show_One,\n 4- Show_List,\n 5- Exit,\n");
                menuOptions = (MenuOptions)int.Parse(Console.ReadLine());
                switch (menuOptions)
                {
                    //adding options
                    case MenuOptions.Add:
                        Console.WriteLine("Adding option:\n 1-BaseStation,\n 2-Drone,\n 3-Custumer,\n 4-Parcel,\n 5-Exit");

                        entityOptions = (EntityOptions)int.Parse(Console.ReadLine());
                        switch (entityOptions)
                        {
                            // add a new station
                            case EntityOptions.Station:
                                Console.WriteLine("Please insert ID, StationName (string), longitude, latitude and charging level ");
                                int id_S, Position;
                                double longitude, latitude;
                                int.TryParse(Console.ReadLine(), out id_S);
                                string StationName = Console.ReadLine();
                                double.TryParse(Console.ReadLine(), out longitude);
                                double.TryParse(Console.ReadLine(), out latitude);
                                int.TryParse(Console.ReadLine(), out Position);
                                Station s = new Station
                                {
                                    Name = StationName,
                                    Id = id_S,
                                    Latitude = latitude,
                                    Longitude = longitude,
                                    ChargeSpots = Position
                                };
                                dalobject.AddStation(s);
                                Console.WriteLine("\nStation added successfully! \n");
                                break;

                            //ADD drone
                            case EntityOptions.Drone:
                                Console.WriteLine("please enter ID, Model (string), Weight categories, Status and Batery level (double)");
                                int id_D, Weight, status;
                                double battery;
                                int.TryParse(Console.ReadLine(), out id_D);
                                string model = Console.ReadLine();
                                Console.WriteLine("enter WeightCategories 1-Light 2-Medium, 3-Heavy");
                                int.TryParse(Console.ReadLine(), out Weight);
                                Console.WriteLine("enter DroneStatuses 1-Available 2-Maintenance, 3-Shipping");
                                int.TryParse(Console.ReadLine(), out status);
                                double.TryParse(Console.ReadLine(), out battery);
                                Drone d = new()
                                {
                                    Id = id_D,
                                    Model = model,
                                    MaxWeight = (WeightCategories)Weight,
                                  //  Status= (DroneStatuses)status,
                                    Battery = battery
                                };
                                dalobject.AddDrone(d);
                                Console.WriteLine("\nDrone added successfully! \n");
                                break;

                            //ADD customer
                            case EntityOptions.Custumer:
                                Console.WriteLine("please enter Customer ID, Name, Phone number, Longitude and Latitude ");
                                int id_C;
                                int.TryParse(Console.ReadLine(), out id_C);
                                string name_C = Console.ReadLine();
                                //נצטרך לבדוק תקינות של מס טלפון
                                Console.WriteLine("enter phone number");
                                string phone_C = Console.ReadLine();
                                Console.WriteLine("enter longitude and latitude");
                                double longitude_C;
                                double.TryParse(Console.ReadLine(), out longitude_C);
                                double latitude_C;
                                double.TryParse(Console.ReadLine(), out latitude_C);
                                Customer c = new()
                                {
                                    Id = id_C,
                                    Name = name_C,
                                    Phone = phone_C,
                                    Latitude = latitude_C,
                                    Longitude = longitude_C
                                };
                                dalobject.AddCustomer(c);
                                Console.WriteLine("\nCustomer added successfully! \n");
                                break;
                            //ADD parcel
                            case EntityOptions.Parcel:
                                Console.WriteLine("Please enter parcel ID");
                                int.TryParse(Console.ReadLine(), out int id_P);
                                Console.WriteLine("Please enter the sender's ID");
                                int.TryParse(Console.ReadLine(), out int id_Psender);
                                Console.WriteLine("Please enter target ID");
                                int.TryParse(Console.ReadLine(), out int id_Ptarget);
                                Console.WriteLine("Please enter parcel weight: 1-Light, 2-Medium, 3-Heavy");
                                int.TryParse(Console.ReadLine(), out int weight_P);
                                Console.WriteLine("Please enter parcel priority: 1-Regular, 2-Fast, 3-Emergency");
                                int.TryParse(Console.ReadLine(), out int priority_P);
                                Console.WriteLine("Please enter drone ID");
                                int.TryParse(Console.ReadLine(), out int id_Pdrone);
                                Console.WriteLine("Please enter time to prepare a package for delivery in format ##:##");
                                DateTime.TryParse(Console.ReadLine(), out DateTime requested_P);
                                Parcel p = new();

                                //try to order the steduled_P pickedUp_P and delivary_P if the user know this
                                //else he added this after
                                try
                                {
                                    Console.Write("pleas enter the time of steduled_P pickedUp_P and delivary_P if you don't know prese 0:");
                                    DateTime ans = DateTime.Parse(Console.ReadLine());
                                    DateTime.TryParse(Console.ReadLine(), out DateTime steduled_P);
                                    DateTime.TryParse(Console.ReadLine(), out DateTime pickedUp_P);
                                    DateTime.TryParse(Console.ReadLine(), out DateTime delivary_P);
                                    p.Assigned = steduled_P;
                                    p.PickedUp = pickedUp_P;
                                    p.Supplied = delivary_P;

                                }
                                catch
                                {
                                    p.Assigned = DateTime.Now;
                                    p.PickedUp = DateTime.Now;
                                    p.Supplied = DateTime.Now;
                                }

                                p.Id = id_P;
                                p.SenderId = id_Psender;
                                p.ReceiverId = id_Ptarget;
                                p.Weight = (WeightCategories)weight_P;
                                p.Priority = (Priorities)priority_P;
                                p.Create = requested_P;
                                p.DroneID = id_Pdrone;

                                dalobject.AddParcel(p);
                                Console.WriteLine("\nParcel added successfully! \n");
                                break;

                            // EXIT
                            case EntityOptions.Exit:
                               break;
                        }
                        break;

                    //UPDATE functions
                    case MenuOptions.Update:
                        {
                            Console.WriteLine("Updating option:\n 1-Parcel to drone,\n 2-Parcel pickedup by drone,\n 3-Supply parcel to customer,\n 4-Send drone to charge,\n 5-Discharge drone, \n 6- Exit");
                            updateOptions = (UpdateOptions)int.Parse(Console.ReadLine());
                            switch (updateOptions)
                            {
                                case UpdateOptions.Assignement:
                                    Console.WriteLine("Please enter Drone ID");
                                    int drone_id;
                                    int.TryParse(Console.ReadLine(), out drone_id);
                                    Console.WriteLine("Please enter Parcel ID");
                                    int parcel_id;
                                    int.TryParse(Console.ReadLine(), out parcel_id);
                                    dalobject.UpdateParcelToDrone(parcel_id, drone_id);
                                    Console.WriteLine("\nParcel updated to drone successfully!\n");
                                    break;

                                case UpdateOptions.Pickedup:
                                    Console.WriteLine("Please enter Drone ID");
                                    int drone_id2;
                                    int.TryParse(Console.ReadLine(), out drone_id2);
                                    Console.WriteLine("Please enter Parcel ID");
                                    int parcel_id2;
                                    int.TryParse(Console.ReadLine(), out parcel_id2);
                                    dalobject.UpdateParcelPickedupByDrone(parcel_id2, drone_id2);
                                    Console.WriteLine("\nParcel pick up updated successfully!\n");
                                    break;

                                case UpdateOptions.Delivery:
                                    Console.WriteLine("Please enter Customer ID");
                                    int customer_id;
                                    int.TryParse(Console.ReadLine(), out customer_id);
                                    Console.WriteLine("Please enter Parcel ID");
                                    int parcel_id3;
                                    int.TryParse(Console.ReadLine(), out parcel_id3);
                                    dalobject.UpdateDeliveryToCustomer(parcel_id3, customer_id);
                                    Console.WriteLine("\nParcel updated to customer successfully!\n");
                                    break;

                                case UpdateOptions.Recharge:
                                    Console.WriteLine("Please enter Drone ID");
                                    int drone_id4;
                                    int.TryParse(Console.ReadLine(), out drone_id4);
                                    Console.WriteLine("choose a station for charging");
                                    // show the list os stations to choose from
                                    dalobject.ShowStationList();
                                    int station_id;
                                    int.TryParse(Console.ReadLine(), out station_id);
                                    dalobject.SendDroneToCharge(drone_id4, station_id);//*******
                                    Console.WriteLine("\nDrone updated to- charge status successfully!\n");
                                    break;

                                case UpdateOptions.Discharge:
                                    Console.WriteLine("Please enter Drone ID");
                                    int drone_id5;
                                    int.TryParse(Console.ReadLine(), out drone_id5);
                                    Console.WriteLine("choose a station for charging");
                                    dalobject.ReleaseDroneFromCharging(drone_id5);//****
                                    Console.WriteLine("\nDrone updated to- discharge status successfully!\n");
                                    break;

                                case UpdateOptions.Exit:
                           
                                    break;
                            }
                            break;
                        }

                    //SHOW options
                    case MenuOptions.Show_One:
                        Console.WriteLine("View item options: \n 1- Station \n 2- Drone\n 3- Custumer\n 4- Parcel\n 5- Exit\n");
                        entityOptions = (EntityOptions)int.Parse(Console.ReadLine());
                        Console.WriteLine($"Enter a requested {entityOptions} id");
                        switch (entityOptions)
                        {
                            //show station
                            case EntityOptions.Station:
                                int Id_S;
                                int.TryParse(Console.ReadLine(), out Id_S);
                                Console.WriteLine(dalobject.GetStation(Id_S));
                                break;
                            //show drone
                            case EntityOptions.Drone:
                                int Id_D;
                                int.TryParse(Console.ReadLine(), out Id_D);
                                Console.WriteLine(dalobject.GetDrone(Id_D));
                                break;
                            //show customer
                            case EntityOptions.Custumer:
                                int Id_C;
                                int.TryParse(Console.ReadLine(), out Id_C);
                                Console.WriteLine(dalobject.GetCustomer(Id_C));
                                break;
                            //show parcel
                            case EntityOptions.Parcel:
                                int Id_P;
                                int.TryParse(Console.ReadLine(), out Id_P);
                                Console.WriteLine(dalobject.GetParcel(Id_P));
                                break;
                            //EXIT
                            case EntityOptions.Exit:                          
                                break;
                        }
                        //int requestion;
                        //int.TryParse(Console.ReadLine(), out requestion);
                        break;

                    //SHOW LIST options:
                    case MenuOptions.Show_List:
                        Console.WriteLine(" List options:\n 1- BaseStation  \n 2- Drone \n 3- Custumer\n 4- Parcel\n 5- UnAsignementParcel\n 6- AvailbleChagingStation\n 7- Exit \n");
                        ListOptions listOptions = (ListOptions)int.Parse(Console.ReadLine());
                        switch (listOptions)
                        {
                            // prints the list of the stations
                            case ListOptions.Station:
                                IEnumerable<Station> BaseStationList = dalobject.ShowStationList();
                                foreach (Station element in BaseStationList) //prints the elements in the list
                                    Console.WriteLine(element);
                                    break;
                            // prints the list of the drones
                            case ListOptions.Drone:
                               
                                IEnumerable<Drone> DroneList;
                                DroneList = dalobject.ShowDroneList();
                                foreach (Drone element in DroneList) //prints the elements in the list
                                    Console.WriteLine(element);
                                break;
                            // prints the list of the customers
                            case ListOptions.Custumer:
                                IEnumerable<Customer> CustomerList;
                                CustomerList = dalobject.ShowCustomerList();
                                foreach (Customer element in CustomerList) //prints the elements in the list
                                    Console.WriteLine(element);
                                break;
                            // prints the list of the parcels
                            case ListOptions.Parcel:
                                IEnumerable<Parcel> ParcelList;
                                ParcelList = dalobject.ShowParcelList();
                                foreach (Parcel element in ParcelList) //prints the elements in the list
                                    Console.WriteLine(element);
                                break;
                            // prints the list of the stations that available for charging
                            case ListOptions.AvailbleChagingStation:
                                List<Station> ChargeableBaseStationList = new();
                                ChargeableBaseStationList = (List<Station>)dalobject.ShowStationList(x => x.ChargeSpots > 0);
                                foreach (Station element in ChargeableBaseStationList) //prints the elements in the list
                                    Console.WriteLine(element);
                                break;
                            // prints the list of the non associated parcel
                            case ListOptions.UnAsignementParcel:
                                dalobject.ShowNonAssociatedParcelList();
                                List<Parcel> NonAssociatedParcelList =  dalobject.ShowNonAssociatedParcelList().ToList();
                                foreach (Parcel element in NonAssociatedParcelList) //prints the elements in the list
                                    Console.WriteLine(element);
                                break;
                            case ListOptions.Exit:
                                return;                              
                        }
                        break;
                    //--BONUS--: another option that recives coordinates and print the distance from it to a station or a customer
                    //אפשרות נוספת שקולטת קואורדינטות נקודה כלשהי ומדפיסה מרחק מבסיס או מלקוח כלשהו לנקודה הזו 
                    case MenuOptions.Calculate_distance:
                        Console.WriteLine("Insert longitude coordinates");
                        double longitudeCoor;
                        double.TryParse(Console.ReadLine(), out longitudeCoor);
                        Console.WriteLine("Insert latitude coordinates");
                        double latitudeCoor;
                        double.TryParse(Console.ReadLine(), out latitudeCoor);
                        Console.WriteLine("Choose distance options:\n 1- from Station\n 2- from Customer\n 3- Exit");
                        DistanceOptions distanceOptions = (DistanceOptions)int.Parse(Console.ReadLine());
                        switch (distanceOptions)
                        {
                            //calculate the distance from a station
                            case DistanceOptions.From_Station:
                                Console.WriteLine("Please enter station ID");
                                int stationID;
                                int.TryParse(Console.ReadLine(), out stationID);
                                Station s = dalobject.GetStation(stationID); //finds the station by its ID
                                double distance_station = dalobject.CalculateDistance(longitudeCoor, latitudeCoor, s.Longitude, s.Latitude);
                                Console.WriteLine($"The distance between your coordination and the station is: {distance_station}");
                                break;
                            //calculate the distance from a customer
                            case DistanceOptions.From_Customer:
                                Console.WriteLine("Please enter customer ID");
                                int customerID;
                                int.TryParse(Console.ReadLine(), out customerID);
                                Customer c = dalobject.GetCustomer(customerID); //finds the customer by his ID
                                double distance_customer = dalobject.CalculateDistance(longitudeCoor, latitudeCoor, c.Latitude, c.Latitude);
                                Console.WriteLine($"The distance between your coordination and the customer is: {distance_customer}");
                                break;
                            // Exit
                            case DistanceOptions.Exit:
                               
                                break;
                        }
                        break;
                    case MenuOptions.Exit:
                        break;
                }

            }
            while (menuOptions != MenuOptions.Exit);
        }
        static void Main(string[] arg)
        {

            ShowMenu();
        }

    }
}
