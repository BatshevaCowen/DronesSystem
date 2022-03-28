using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BO;
using BlApi;
using DalApi;
using System.Threading;

namespace BL
{
    internal sealed partial class BL : IBL
    {
        private IDal dalo; //access to dal object
        private List<DroneToList> dronesL;
        static Random r = new() { };

        public List<DroneToList> DronesL { get => dronesL; }

        #region Singleton

        public static IBL instance { get; } = new BL();

        #endregion Singleton
        static BL() { }
        private BL()
        {
            //Access to the layer DAL
            dalo = DLFactory.GetDL();
            dronesL = new List<DroneToList>();
            var Drones = dalo.ShowDroneList();
            DronesInitialize(Drones);
            
        }
        /// <summary>
        /// Constractor for drones initializing
        /// </summary>
        /// <param name="drones"></param>
        public void DronesInitialize(IEnumerable<DO.Drone> drones)
        {

            //find A package that has not yet been delivered but the drone has already been associated
            List<DO.Parcel> parcels = dalo.ShowParcelList().ToList();
            DroneToList droneBL;

            foreach (var droneDL in drones)
            {
                droneBL = new DroneToList()
                {
                    Id = droneDL.Id,
                    Model = droneDL.Model,
                    Weight = (Weight)droneDL.MaxWeight
                };
                droneBL.Location = new();

                List<DO.Parcel> parcelList = parcels.FindAll(p => p.DroneID == droneBL.Id);

                if (parcelList.Count != 0) //If there is a package that has not yet been delivered but the drone has already been associated
                {
                    droneBL.DroneStatuses = DroneStatuses.Shipping;
                    //If the package was associated but not collected
                    foreach (var p in parcels.Where(p => p.PickedUp == DateTime.MinValue))
                    {
                        // The location of the drone will be at the station closest to the sender
                        int senderId = p.SenderId;
                        double senderLattitude = dalo.GetCustomer(senderId).Latitude;
                        double senderLongitude = dalo.GetCustomer(senderId).Longitude;
                        //מרחק בין 2 תחנות הנמוך ביותר
                        DO.Station st = dalo.GetClosestStation(senderLattitude, senderLongitude);
                        droneBL.Location = new Location
                        {
                            Latitude = st.Latitude,
                            Longitude = st.Longitude
                        };
                        droneBL.Battery = r.Next(0, 101);
                    }
                    //If the package has been collected but has not yet been delivered
                    foreach (var p in parcels.Where(p => p.PickedUp != DateTime.MinValue && p.Supplied == DateTime.MinValue))
                    {
                        //The location of the drone will be at the location of the sender
                        int senderId = p.SenderId;
                        double senderLattitude = dalo.GetCustomer(senderId).Latitude;
                        double senderLongitude = dalo.GetCustomer(senderId).Longitude;
                        DO.Station st = dalo.GetClosestStation(senderLattitude, senderLongitude);
                        droneBL.Location = new Location
                        {
                            Latitude = st.Latitude,
                            Longitude = st.Longitude
                        };

                        double distance = dalo.GetDistanceBetweenLocationsOfParcels(p.SenderId, p.ReceiverId)
                            + dalo.GetDistanceBetweenLocationAndClosestStation(p.ReceiverId);
                        switch (p.Weight)
                        {
                            case DO.WeightCategories.Light:
                                droneBL.Battery = r.Next((int)(distance * dalo.PowerRequest()[1] + 1), 101);
                                break;
                            case DO.WeightCategories.Medium:
                                droneBL.Battery = r.Next((int)(distance * dalo.PowerRequest()[2] + 1), 101);
                                break;
                            case DO.WeightCategories.Heavy:
                                droneBL.Battery = r.Next((int)(distance * dalo.PowerRequest()[3] + 1), 101);
                                break;
                            default:
                                break;
                        }
                    }
                }
                else //the drone is not in delivery
                {
                    droneBL.DroneStatuses = (DroneStatuses)r.Next(1, 3); //Maintenance or Available
                    if (droneBL.DroneStatuses == DroneStatuses.Maintenance)
                    {
                        //Its location will be drawn between the purchasing stations
                        List<DO.Station> stations = dalo.ShowStationList().ToList();
                        int index = r.Next(0, stations.Count());
                        droneBL.Location = new()
                        {
                            Latitude = stations[index].Latitude,
                            Longitude = stations[index].Longitude
                        };

                        droneBL.Battery = r.Next(0, 21);
                    }
                    else if (droneBL.DroneStatuses == DroneStatuses.Available)
                    {
                        //Its location will be raffled off among customers who have packages provided to them
                        List<DO.Parcel> parcelsDelivered = parcels.FindAll(p => p.Supplied != DateTime.MinValue);
                        int index = r.Next(0, parcelsDelivered.Count());
                        DO.Customer customer = dalo.GetCustomer(parcelsDelivered[index].ReceiverId);
                        droneBL.Location = new()
                        {

                            Latitude = customer.Latitude,
                            Longitude = customer.Longitude
                        };
                        // Battery mode will be recharged between a minimal charge that will allow it to reach the station closest to charging and a full charge
                        double distance = dalo.GetDistanceBetweenLocationAndClosestStation(parcelsDelivered[index].ReceiverId);

                       
                        droneBL.Battery = r.Next((int)(distance * dalo.PowerRequest()[0] + 1));

                    }
                }
                DronesL.Add(droneBL);
            }
           
            //get Station to update Location
            DO.Station station = dalo.GetStation(12345);
           
       
        }
        
        /// <summary>
        /// calculate the distance between 2 locations
        /// </summary>
        /// <param name="longitude1"></param>
        /// <param name="latitude1"></param>
        /// <param name="longitude2"></param>
        /// <param name="latitude2"></param>
        /// <returns></returns>
        public double CalculateDistance(double longitude1, double latitude1, double longitude2, double latitude2)
        {
            double dis = dalo.CalculateDistance(longitude1, latitude1, longitude2, latitude2);
            return dis;
        }
    }
}


//Random random = new Random();
//var parcelList = dalo.ShowParcelList().ToList();
//for (int i = 0; i < parcelList.Count(); i++)
//{
//    var parcel = parcelList[i];
//    object obj = new DateTime(2021, random.Next(0, 12), random.Next(0, 28), random.Next(0, 60), random.Next(0, 60), random.Next(0, 24));
//    parcel.Create = (DateTime)obj;
//    parcel.Assigned = parcel.Create.AddDays(random.Next(0, 3)).AddSeconds(random.Next(0, 60)).AddMinutes(random.Next(0, 60)).AddHours(random.Next(0, 24));
//    parcel.PickedUp = parcel.Assigned.AddDays(random.Next(0, 3)).AddSeconds(random.Next(0, 60)).AddMinutes(random.Next(0, 60)).AddHours(random.Next(0, 24));
//    parcel.Supplied = parcel.PickedUp.AddDays(random.Next(0, 3)).AddSeconds(random.Next(0, 60)).AddMinutes(random.Next(0, 60)).AddHours(random.Next(0, 24));
//    dalo.RemoveParcel(parcel);
//    dalo.AddParcel(parcel);
//}