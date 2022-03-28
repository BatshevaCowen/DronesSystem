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
    {        /// <summary>
             /// Add station
             /// </summary>
             /// <param name="station"></param>
             /// <exception cref="NotImplementedException"></exception>
        public void AddStation(Station station)
        {
            //station ID sould be 5 - 6 digits
            if (station.Id < 10000 || station.Id >= 1000000)
            {
                throw new StationException(station.Id, "station ID sould be 5-6 digits");
            }
            DO.Station s = new()
            {
                Name = station.Name,
                Id = station.Id,
                Longitude = station.Location.Longitude,
                Latitude = station.Location.Latitude,
                ChargeSpots = station.AvailableChargingSpots,
            };
            dalo.AddStation(s);//send the new station to DAL 
        }

        /// <summary>
        /// Update station
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="charging_spots"></param>
        public void UpdateStetion(int id, string name, int charging_spots)
        {
            dalo.UpdateStetion(id, name, charging_spots);
        }

        /// <summary>
        /// Get station by ID
        /// </summary>
        /// <param name="requestedId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Station GetStation(int stationID)
        {
            DO.Station s = dalo.GetStation(stationID);
            Station station = new()
            {
                Name = s.Name,
                Id = s.Id,
                AvailableChargingSpots = s.ChargeSpots
            };
            station.Location = new()
            {
                Latitude = s.Latitude,
                Longitude = s.Longitude
            };
            //list of drone and 
            List<DO.DroneCharge> droneCharges = dalo.GetListOfDronInCharge(stationID);
            foreach (DO.DroneCharge item in droneCharges)
            {
                DroneInCharging droneInCharging = new();
                droneInCharging.Id = item.DroneId;
                DroneToList droneToList = DronesL.Find(x => x.Id == item.DroneId);
                droneInCharging.Battery = droneToList.Battery;
                station.droneInChargings.Add(droneInCharging);
            }
            return station;
        }

        /// <summary>
        /// Show LIST of stations
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<StationToList> ShowStationList()
        {
            List<StationToList> stationList = new();
            var stations = dalo.ShowStationList();
            foreach (DO.Station item in stations)
            {
                StationToList station = new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    AvailableChargingSpots = item.ChargeSpots
                };
                //list of drones that are currently charging in the station 
                List<DO.DroneCharge> droneCharges = dalo.GetListOfDronInCharge(station.Id);
                station.UnavailableChargingSpots = droneCharges.Count;
                stationList.Add(station);
            }
            return stationList;
        }

        /// <summary>
        /// Show LIST of chargeable stations
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<StationToList> ShowChargeableStationList()
        {
            List<StationToList> stationListWithAvailableChargingSpots = new List<StationToList>();
            IEnumerable<DO.Station> stations = dalo.ShowStationList(x => x.ChargeSpots > 0);
            foreach (var item in stations)
            {
                StationToList stationTL = new()
                {
                    Id = item.Id,
                    Name = item.Name,
                    UnavailableChargingSpots = GetStation(item.Id).droneInChargings.Count(),
                    AvailableChargingSpots = item.ChargeSpots - (GetStation(item.Id).droneInChargings.Count())
                };
                stationListWithAvailableChargingSpots.Add(stationTL);
            }
            return stationListWithAvailableChargingSpots;
        }

        /// <summary>
        /// Add drone to the list of drines that are currantly charging in the station
        /// and also UPDATE the number of available charging spots in the station
        /// </summary>
        /// <param name="stationId"></param>
        /// <param name="droneId"></param>
        public void UpdateStationListDroneInCharge(int stationId, int droneId)
        {
            Station station = GetStation(stationId);
            Drone drone = GetDrone(droneId);
            DroneInCharging droneInCharging = new()
            {
                Id = drone.Id,
                Battery = drone.Battery
            };
            station.droneInChargings.Add(droneInCharging);
            //update the number of available charging spots in the station
            station.AvailableChargingSpots--;
        }
        public List<DroneInCharging> GetDroneInCgargingList(int stationId)
        {
            List<DroneInCharging> droneInChargingsList = new List<DroneInCharging>();
            dalo.GetListOfDronInChargeing(stationId);
            return droneInChargingsList;
        }
    }
}
