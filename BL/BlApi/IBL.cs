using BO;
using System;
using System.Collections.Generic;


namespace BlApi
{
    public interface IBL
    {
        #region Add:
        /// <summary>
        /// Add Station
        /// </summary>
        /// <param name="station"></param>
        void AddStation(Station station);

        /// <summary>
        /// Add Drone
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="stationId"></param>
        void AddDrone(Drone drone, int stationId);

        /// <summary>
        /// Add Customer
        /// </summary>
        /// <param name="customer"></param>
        void AddCustomer(Customer customer);

        /// <summary>
        /// Add Parcel
        /// </summary>
        /// <param name="parcel"></param>
        void AddParcel(Parcel parcel);

        /// <summary>
        /// Adds User to data
        /// 
        /// Throws BOBadUserException
        /// </summary>
        /// <param name="tmpUser"></param>
        void AddUser(User tmpUser);
        #endregion

        #region Update:


        /// <summary>
        /// Update drone's name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        void UpdateDroneName(int id, string model);

        /// <summary>
        /// Update station
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="charging_spots"></param>
        void UpdateStetion(int id, string name, int charging_spots);
        /// <summary>
        /// Update Customer
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="phone"></param>
        void UpdateCustomer(int id, string name, string phone);

        /// <summary>
        ///  Charging drone
        /// </summary>
        /// <param name="id"></param>
        void UpdateChargeDrone(int id);
        /// <summary>
        /// discharge drone
        /// </summary>
        /// <param name="droneID"></param>
        /// <param name="chargingTime"></param>
        void DischargeDrone(int droneID, TimeSpan chargingTime);

        /// <summary>
        /// Update parcel to drone
        /// </summary>
        /// <param name="droneId"></param>
        void UpdateParcelToDrone(int droneId);

        /// <summary>
        /// Updete that the parcel has picked up by a drone
        /// </summary>
        /// <param name="droneId"></param>
        void UpdateParcelPickUpByDrone(int droneId);
        void UpdateParcelSuppliedByDrone(int droneId);
        void UpdateStationListDroneInCharge(int stationId, int droneId);
        #endregion

        #region Show Lists:
        IEnumerable<StationToList> ShowStationList();
        IEnumerable<DroneToList> ShowDroneList();
        IEnumerable<CustomerToList> ShowCustomerList();
        IEnumerable<ParcelToList> ShowParcelList();
        IEnumerable<ParcelToList> ShowParcelList(User user); //show parcel for user
        IEnumerable<ParcelToList> ShowNonAssociatedParcelList();
        IEnumerable<StationToList> ShowChargeableStationList();

        #endregion

        #region Get by ID;
        Station GetStation(int requestedId);
        Drone GetDrone(int droneId);
        Customer GetCustomer(int customerId);
        Parcel GetParcel(int parcelId);
        Parcel GetParcelByDroneId(int droneId);
        /// <summary>
        /// Returns User that has that name
        /// 
        /// Throws BOBadUserException
        /// </summary>
        /// <param name="userName">Name of user</param>
        /// <returns></returns>
        User GetUser(string userName);
        /// <summary>
        /// Finds the customer by his user 
        /// </summary>
        /// <param name="user"></param>
        /// <returns>Customer</returns>
        DO.Customer GetCustomer_ByUsername(User user);
        #endregion


        ParcelStatus FindParcelStatus(DO.Parcel parcel);

        double CalculateDistance(double longitude1, double latitude1, double longitude2, double latitude2);

        List<DroneInCharging> GetDroneInCgargingList(int stationId);
        void RemoveParcel(int idparcel);
    }
}