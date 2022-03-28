using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using DO;

namespace DAL
{
    internal sealed partial class DalObject : DalApi.IDal
    {
        #region Drone
        /// <summary>
        /// add Drone to the drons list
        /// </summary>
        /// <param name="d"></param>
        /// <returns></returns>
        public void AddDrone(Drone d)
        {
            if (DataSource.Drones.Exists(drone => drone.Id == d.Id))
            {
                throw new DroneException($"ID {d.Id} already exists!!");
            }
            else
                DataSource.Drones.Add(d);
        }
        /// <summary>
        /// Update name of drone
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        public void UpdateNameOfDrone(int id, string model)
        {
            Drone drone = DataSource.Drones.Find(x => x.Id == id);
            DataSource.Drones.Remove(drone);
            drone.Model = model;
            DataSource.Drones.Add(drone);
        }
        /// <summary>
        /// view function for Drone
        /// </summary>
        /// <param name="id"></param>
        public Drone GetDrone(int id)
        {
            if (!DataSource.Drones.Exists(item => item.Id == id))
            {
                throw new DroneException($"ID: {id} does not exist!!");
            }
            return DataSource.Drones.Find(c => c.Id == id);
        }
        /// <summary>
        /// view lists functions for Drone
        /// </summary>
        public IEnumerable<Drone> ShowDroneList(Func<Drone, bool> predicate = null)
        {
            if (predicate == null)
            {
                List<Drone> DroneList = new List<Drone>();
                foreach (Drone element in DataSource.Drones)
                {
                    DroneList.Add(element);
                }
                return DroneList;
            }
            else
                return DataSource.Drones.Where(predicate).ToList();
        }

        /// <summary>
        /// Send a drone to charge
        /// </summary>
        /// <param name="droneId">the drone to send to charge</param>
        /// <param name="stationId">the station to send it to charge</param>
        public void SendDroneToCharge(int droneId, int stationId)
        {
            Drone drone = GetDrone(droneId);
            Station station = GetStation(stationId);
            DataSource.Stations.Remove(station);

            DataSource.DroneCharges.Add(new DroneCharge
            {
                DroneId = drone.Id,
                StationId = station.Id
            });
            station.ChargeSpots--;

            DataSource.Stations.Add(station);
        }
        /// <summary>
        /// release a drone from charge
        /// </summary>
        /// <param name="droneId">the id of the drone to release</param>
        public void ReleaseDroneFromCharging(int droneId)
        {
            Drone drone = GetDrone(droneId);
            DataSource.Drones.Remove(drone);

            DroneCharge droneCharge = DataSource.DroneCharges.Find(x => x.DroneId == droneId);


            int stationId = droneCharge.StationId;
            Station station = GetStation(stationId);
            DataSource.Stations.Remove(station);

            station.ChargeSpots++;

            DataSource.Stations.Add(station);
            DataSource.Drones.Add(drone);
            DataSource.DroneCharges.Remove(droneCharge);

        }
        /// <summary>
        /// discharge drone
        /// <param name="droneID"></param>
        /// <param name="droneLatitude"></param>
        /// <param name="droneLongitude"></param>
        /// <exception cref="Exception"></exception>
        public Station DischargeDroneByLocation(int droneID, double droneLatitude, double droneLongitude)
        {
            Drone d = DataSource.Drones.Find(x => x.Id == droneID);
            Station s = new Station();
            foreach (Station item in DataSource.Stations) //finds the station
            {
                if (item.Latitude == droneLatitude && item.Longitude == droneLongitude)
                {
                    DataSource.Stations.Remove(s);
                    s = item;
                    s.ChargeSpots++;
                    DataSource.Stations.Add(s);
                    return s;
                }
            }
            throw new Exception("couldn't find station by drones location");
        }

        /// <summary>
        /// Update the station to have one less spot for charging (because we sent a drone to charg there)
        /// </summary>
        /// <param name="StationId"></param>
        /// <param name="drone"></param>
        public Station UpdateStationChargingSpots(int StationId)
        {
            Station station = DataSource.Stations.Find(x => x.Id == StationId);
            station.ChargeSpots -= 1;
            return station;
        }
        /// <summary>
        /// Method of applying drone power
        /// </summary>
        /// <returns>An array of the amount of power consumption of a drone for each situation</returns>
      

        public void updateBatteryDrone(int id, double dis)
        {
            Drone d = DataSource.Drones.Find(x => x.Id == id);
            DataSource.Drones.Remove(d);
            d.Battery -= dis * 0.1;
            DataSource.Drones.Add(d);
        }
        #endregion
    }
}
