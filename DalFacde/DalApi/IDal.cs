using DO;
using System.Collections.Generic;
using System;
using DalApi;

namespace DalApi
{
    public interface IDal
    {
        #region ADD:
        /// <summary>
        /// Add Customer
        /// </summary>
        /// <param name="c"></param>
        void AddCustomer(Customer c);
        /// <summary>
        /// Add Drone
        /// </summary>
        /// <param name="d"></param>
        void AddDrone(Drone d);
        /// <summary>
        /// Add Parcel
        /// </summary>
        /// <param name="p"></param>
        void AddParcel(Parcel p);
        /// <summary>
        /// Add Station
        /// </summary>
        /// <param name="s"></param>
        void AddStation(Station s);
        /// <summary>
        /// BONUS- Return the distance betwin 2 point
        /// </summary>
        /// <param name="longitude1"></param>
        /// <param name="latitude1"></param>
        /// <param name="longitude2"></param>
        /// <param name="latitude2"></param>
        /// <returns>double</returns>
        double CalculateDistance(double longitude1, double latitude1, double longitude2, double latitude2);
        /// <summary>
        /// send drone to charging
        /// </summary>
        /// <param name="drone_id"></param>
        /// <param name="longt"></param>
        /// <param name="latit"></param>
        void ChargeDrone_needToCheck_IfWork(int drone_id, double longt,double latit);
        /// <summary>
        /// send drone to release from charging
        /// </summary>
        /// <param name="droneId"></param>
        void ReleaseDroneFromCharging(int droneId);
        /// <summary>
        /// add user
        /// throw BadUserException
        /// </summary>
        /// <param name="tmpUser">user to add</param>
        void AddUser(User tmpUser);
        #endregion

        #region Get by ID:

        /// <summary>
        /// Get Station by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Station GetStation(int id);

        /// <summary>
        /// Get Customer by id
        /// </summary>
        /// <param name="IDc"></param>
        /// <returns></returns>
        Customer GetCustomer(int IDc);

        /// <summary>
        /// Get Drone by drone
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Drone GetDrone(int id);

        /// <summary>
        /// Get Parcel by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Parcel GetParcel(int id);

        /// <summary>
        /// The function recives station ID and returns all of the drones that are charging in that station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        List<DroneCharge> GetListOfDronInCharge(int stationId);

        /// <summary>
        /// List of packages that belong to the customer sender
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Parcel> GetListOfParcelSender(int id);

        /// <summary>
        /// List of packages that belong to the customer reciever
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        List<Parcel> GetListOfParcelRecirver(int id);

        /// <summary>
        /// Search for the package in delivery mode
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        
        Parcel GetParcelInTransferByDroneId(int droneId);
        /// <summary>
        ///  Get Parce lBy DroneIds parcel
        /// </summary>
        /// <param name="DroneId"></param>
        /// <returns></returns>
        Parcel GetParcelByDroneId(int DroneId);
        /// <summary>
        /// A function that calculates the distance between a customer's location and a base station for charging
        /// </summary>
        /// <param name="Reciverid"></param>
        /// <returns></returns
        double GetDistanceBetweenLocationAndClosestBaseStation(int Reciverid);

        /// <summary>
        ///  A function that calculates the distance between two points on the map
        /// </summary>
        /// <param name="senderId"></param>
        /// <param name="targetId"></param>
        /// <returns></returns>
        double GetDistanceBetweenLocationsOfParcels(int senderId, int targetId);

        /// <summary>
        /// get solid user by his name
        /// throw BadUserException
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        User GetUser(string userName);

        #endregion

        #region  SHOW LISTS:
        /// <summary>
        /// Show Station List
        /// </summary>
        /// <returns></returns>
        IEnumerable<Station> ShowStationList(Func<Station, bool> predicate = null);
        /// <summary>
        /// Show Customer List
        /// </summary>
        /// <returns></returns>
        IEnumerable<Customer> ShowCustomerList(Func<Customer, bool> predicate = null);
        /// <summary>
        /// Show Drone List
        /// </summary>
        /// <returns></returns>
        IEnumerable<Drone> ShowDroneList(Func<Drone, bool> predicate = null);
        /// <summary>
        /// Show Parcel List
        /// </summary>
        /// <returns></returns>
        IEnumerable<Parcel> ShowParcelList(Func<Parcel, bool> predicate = null);
        /// <summary>
        /// show list of non-associated parcels
        /// </summary>
        /// <returns></returns>
        IEnumerable<Parcel> ShowNonAssociatedParcelList();
        /// <summary>
        /// get all users
        /// </summary>
        /// <returns>IEnumerable implemented by users</returns>
        IEnumerable<User> GetAllUsers();
        /// <summary>
        /// get all users that satisfies the condition
        /// throw ReadDataException
        /// </summary>
        /// <param name="predicate">the condition (bool)</param>
        /// <returns>IEnumerable implemented by users satisfies the cindition</returns>
        IEnumerable<User> GetAllUsersBy(Predicate<User> predicate);
        #endregion

        #region Update:
        /// <summary>
        ///  Adds an organ to the list of droneCharge
        /// </summary>
        /// <param name="dronId"></param>
        /// <param name="stationId"></param>
        void UpdateAddDroneToCharge(int dronId, int stationId);
        /// <summary>
        /// Uncharge drone and update the station
        /// </summary>
        /// <param name="dronId"></param>
        /// <param name="stationId"></param>
        void UpdateRemoveDroneToCharge(int dronId, int stationId);
        /// <summary>
        /// Looking for the closest station with available charging spots
        /// </summary>
        /// <param name="lang"></param>
        /// <param name="lati"></param>
        /// <returns></returns>
        List<Distance> MinimumDistance(double lang, double lati);
        /// <summary>
        /// discharge drone
        /// </summary>
        /// <param name="droneID"></param>
        /// <param name="droneLatitude"></param>
        /// <param name="droneLongitude"></param>
        Station DischargeDroneByLocation(int droneID, double droneLatitude, double droneLongitude);
        /// <summary>
        /// update the Battry of the Drone in layer data sourse
        /// </summary>
        /// <param name="dronId"></param>
        void updateBatteryDrone(int dronId,double dis);
        /// <summary>
        ///  Update parcel delivered to Customer
        /// </summary>
        /// <param name="parcel_id"></param>
        /// <param name="customer_id"></param>
        void UpdateDeliveryToCustomer(int parcel_id, int customer_id);
        /// <summary>
        ///  Update parcel delivered to Customer
        /// </summary>
        /// <param name="parcel_id"></param>
        /// <param name="drone_id"></param>
        void UpdateParcelPickedupByDrone(int parcel_id, int drone_id);
        /// <summary>
        /// update function: parcel to drone by id and return true if the parcel found
        /// </summary>
        /// <param name="parcel_id"></param>
        /// <param name="drone_id"></param>
        int UpdateParcelToDrone(int parcel_id, int drone_id);
        /// <summary>
        /// return the station in order to update the locations drone 
        /// </summary>
        /// <param name="StationId"></param>
        /// <returns></returns>
        Station UpdateStationChargingSpots(int StationId);
        /// <summary>
        /// Update name of drone
        /// </summary>
        /// <param name="DroneId"></param>
        /// <param name="model"></param>
        void UpdateNameOfDrone(int DroneId, string model);
        /// <summary>
        /// Update station data
        /// </summary>
        /// <param name="StationId"></param>
        /// <param name="name"></param>
        /// <param name="charging_spots"></param>
        void UpdateStetion(int StationId, string name, int charging_spots);
        /// <summary>
        /// Update custumer data
        /// </summary>
        /// <param name="custumerId"></param>
        /// <param name="name"></param>
        /// <param name="phon"></param>
        void UpdateCustumer(int custumerId, string name, string phon);
        /// <summary>
        /// Update station ChargeSpots
        /// </summary>
        /// <param name="stationId"></param>
        void UpdateChargeSpots(int stationId);
        /// <summary>
        /// Method of applying drone power
        /// </summary>
        /// <returns></returns>
        double[] PowerRequest();
        /// <summary>
        ///get the min destance station
        /// </summary>
        /// <param name="latitude"></param>
        /// <param name="longitude"></param>
        /// <returns></returns>
        Station GetClosestStation(double latitude, double longitude);
        /// <summary>
        /// Method of applying drone power
        /// </summary>
        /// <returns></returns>
        double[] PowerConsumptionRequest();
        /// <summary>
        /// send a drone to charge
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId"></param>
        public void SendDroneToCharge(int droneId, int stationId);
        /// <summary>
        /// update user (delete the old and add the new)
        /// throw BadUserException
        /// </summary>
        /// <param name="userToUpdate">user To Update</param>
        void UpdateUser(User userToUpdate);

        #endregion

        /// <summary>
        /// delete user
        /// throw BadUserException
        /// </summary>
        /// <param name="userName">name of user to delete</param>
        void DeleteUser(string userName);
        List<Tuple<int, double>> GetListOfDronInChargeing(int stationId);
        void RemoveParcel(Parcel p);
    }
}