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
        /// <summary>
        /// add Station to the stations list
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public void AddStation(Station s)
        {
            if (DataSource.Stations.Exists(station => station.Id == s.Id))
            {
                throw new StationException($"ID {s.Id} already exists!!");
            }
            else
                DataSource.Stations.Add(s);
        }
        /// <summary>
        /// Update station data
        /// </summary>
        /// <param name="StationId"></param>
        /// <param name="name"></param>
        /// <param name="charging_spots"></param>
        public void UpdateStetion(int StationId, string name, int charging_spots)
        {
            if (!DataSource.Stations.Exists(x => x.Id == StationId))
            {
                throw new Exception($"station id {StationId} dose not exit!");
            }
            Station station = DataSource.Stations.Find(x => x.Id == StationId);
            DataSource.Stations.Remove(station);
            station.ChargeSpots = charging_spots;
            station.Name = name;
            DataSource.Stations.Add(station);
        }
        /// <summary>
        /// View Station
        /// </summary>
        /// <param name="id"></param>
        public Station GetStation(int id)
        {
            if (!DataSource.Stations.Exists(item => item.Id == id))
            {
                throw new StationException($"ID: {id} does not exist!!");
            }
            return DataSource.Stations.First(c => c.Id == id);
        }

        /// <summary>
        /// View Station List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Station> ShowStationList(Func<Station, bool> predicate = null)
        {
            if (predicate == null)
            {
                List<Station> stationList = new();
                foreach (Station element in DataSource.Stations)
                {
                    stationList.Add(element);
                }
                return stationList;
            }
            return DataSource.Stations.Where(predicate).ToList();
        }

       



        /// <summary>
        /// updates the number of available charging spots
        /// </summary>
        /// <param name="stationId"></param>
        public void UpdateChargeSpots(int stationId)
        {
            Station station = DataSource.Stations.Find(x => x.Id == stationId);
            DataSource.Stations.Remove(station);
            station.ChargeSpots--;
            DataSource.Stations.Add(station);
        }

        public void UpdateAddDroneToCharge(int dronId, int stationId)
        {
            DataSource.DroneCharges.Add(new DroneCharge()
            {
                DroneId = dronId,
                StationId = stationId
            });
        }

        /// <summary>
        /// Uncharge drone and update the station
        /// </summary>
        /// <param name="dronId"></param>
        /// <param name="stationId"></param>
        public void UpdateRemoveDroneToCharge(int dronId, int stationId)
        {
            DroneCharge droneCharge = new()
            {
                DroneId = dronId,
                StationId = stationId
            };
            //מחיקת מופע לרשימה
            DataSource.DroneCharges.Remove(droneCharge);
            Drone d= DataSource.Drones.Find(x => x.Id == dronId);
            DataSource.Drones.Remove(d);
            d.Status = DroneStatuses.Available;
            DataSource.Drones.Add(d);
        }

        /// <summary>
        /// The function recives station ID and returns all of the drones that are charging in that station
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public List<DroneCharge> GetListOfDronInCharge(int stationId)
        {
            List<DroneCharge> newDroneCharges = new ();
            foreach (DroneCharge item in DataSource.DroneCharges)
            {
                if(item.StationId==stationId)
                {
                    newDroneCharges.Add(item);
                }
            }
            return newDroneCharges;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="stationId"></param>
        /// <returns></returns>
        public List<Tuple<int, double>> GetListOfDronInChargeing(int stationId)
        {
            List<Tuple<int, double>> listOnIDandBattery = new List<Tuple<int, double>>();
            foreach (DroneCharge item in DataSource.DroneCharges)
            { 
                if (item.StationId == stationId)
                {
                    Drone d = DataSource.Drones.Find(x => x.Id==item.DroneId);
                    listOnIDandBattery.Add(new Tuple<int, double>(d.Id,d.Battery));
                }
            }
            return listOnIDandBattery;
        }
        /// <summary>
        /// A function that returns a minimum distance between a point and a station
        /// </summary>
        /// <param name="senderLattitude">Lattitude of sender</param>
        /// <param name="senderLongitude">longitude of sender</param>
        /// <param name="flag">Optional field for selecting a nearby station flag = false or available nearby station flag = true</param>
        /// <returns>Station closest to the point</returns>
        public Station GetClosestStation(double senderLattitude, double senderLongitude)
        {
            bool flag = false;
            double minDistance = 1000000000000;
            Station station = new();
            if (!flag)
            {
                foreach (var s in DataSource.Stations)
                {
                    double dictance = Math.Sqrt(Math.Pow(s.Latitude - senderLattitude, 2) + Math.Pow(s.Longitude - senderLongitude, 2));
                    if (minDistance > dictance)
                    {
                        minDistance = dictance;
                        station = s;
                    }
                }
            }
            else
            {
                foreach (var s in DataSource.Stations.Where(s => s.ChargeSpots > 0))
                {
                    double dictance = Math.Sqrt(Math.Pow(s.Latitude - senderLattitude, 2) + Math.Pow(s.Longitude - senderLongitude, 2));
                    if (minDistance > dictance)
                    {
                        minDistance = dictance;
                        station = s;
                    }
                }
            }
            return station;
        }

        /// <summary>
        /// A function that calculates the distance between a customer's location and a station for charging
        /// </summary>
        /// <param name="targetId">target Id</param>
        /// <returns>Minimum distance to the nearest station</returns>
        public double GetDistanceBetweenLocationAndClosestStation(int Reciverid)
        {
            double minDistance = 1000000000000;
            Customer target = GetCustomer(Reciverid);
            foreach (var s in DataSource.Stations)
            {
                double dictance = Math.Sqrt(Math.Pow(s.Latitude - target.Latitude, 2) + Math.Pow(s.Longitude - target.Longitude, 2));
                if (minDistance > dictance)
                {
                    minDistance = dictance;
                }
            }
            return minDistance;
        }
    }
}
