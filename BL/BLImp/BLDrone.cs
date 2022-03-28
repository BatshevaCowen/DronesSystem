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
        /// Add drone
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="stationId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public void AddDrone(Drone drone, int stationId)
        {
            //ID is less than 4 digits or more than 9 digits
            if (drone.Id < 1000 || drone.Id > 999999999)
            {
                throw new DronelIdException(drone.Id, "Drone ID must be between 4 to 9 digits");
            }
            //Weight category must be between 1-3
            if ((double)drone.Weight < 1 || (double)drone.Weight > 3)
            {
                throw new WeightCategoryException(drone.Weight, "Weight category must be between 1-3");
            }
            //station ID should be 5-6 digits
            if (stationId < 10000 || stationId >= 1000000)
            {
                throw new StationException(stationId, "Station ID should be 5 to 6 digits");
            }

            DO.Drone d = new()
            {
                Id = drone.Id,
                Model = drone.Model,
                MaxWeight = (DO.WeightCategories)drone.Weight,
                Battery = r.Next(20, 40),
                Status = DO.DroneStatuses.Maintenance //when added a new drone it goes to initial charging
            };

            //get Station to update Location
            DO.Station station = dalo.GetStation(stationId);
            drone.Battery = d.Battery;
            dalo.AddDrone(d); //adds the drone to the dal object
            AddDroneToList(drone, station.Latitude, station.Longitude);
            dalo.UpdateChargeSpots(station.Id);
            if (station.ChargeSpots > 0)
            {
                dalo.UpdateAddDroneToCharge(drone.Id, station.Id);
            }
            else
            {
                throw new Exception("there are not availble ChargeSpots!");
            }
        }

        /// <summary>
        /// Update drone's name
        /// </summary>
        /// <param name="id"></param>
        /// <param name="model"></param>
        public void UpdateDroneName(int id, string model)
        {
            //if the recived ID does not exist
            if (!DronesL.Exists(item => item.Id == id))
            {
                throw new DronelIdException(id, $"Drone ID: {id} Does not exist!!");
            }
            //update BL drone to list 
            DronesL.Find(item => item.Id == id).Model = model;
            dalo.UpdateNameOfDrone(id, model);
        }

        /// <summary>
        /// Charging drone
        /// </summary>
        /// <param name="droneId"></param>
        public void UpdateChargeDrone(int droneId)
        {
            DO.Station station = new();

            //finds the drone by the recived ID
            DroneToList dronel = DronesL.Find(x => x.Id == droneId);

            //if the drone is available- it can be sent for charging
            if (dronel.DroneStatuses == DroneStatuses.Available)
            {
                //list of the distances from the drone to every station
                List<DO.Distance> disStationFromDrone = dalo.MinimumDistance(dronel.Location.Longitude, dronel.Location.Latitude);
                double minDistance = double.MaxValue;
                int idS, counter = 0;
                bool flag = false;
                //number of distances in the list
                int sized = disStationFromDrone.Count;
                //goes over the list
                while (flag == false && counter <= sized)
                {
                    foreach (DO.Distance item in disStationFromDrone)
                    {
                        //to find the station with the minimum distance from the drone
                        if (item.Length <= minDistance)
                        {
                            minDistance = item.Length;
                            idS = item.Id;
                        }

                        station = dalo.GetStation(item.Id);
                        //if there is an available charging spot in the station
                        if (station.ChargeSpots > 0)
                        {
                            //only if there is enough battery
                            if (dronel.Battery > minDistance * 10 / 100)
                            {
                                flag = true;
                                //function to update Battery, drone mode and distance
                                UpdateDroneToStation(droneId, station.Id, minDistance);
                            }
                            else
                                throw new DroneBatteryException("there is not enough battery to send the drone to charge in the station ");
                        }
                        counter++;
                        disStationFromDrone.Remove(item);
                        if (flag)
                            break;
                    }
                }
                if (flag == false)
                {
                    throw new Exception("drone can not be sent for charging! ");
                }
            }
            else if (dronel.DroneStatuses != DroneStatuses.Available)
            {
                throw new Exception("drone can not be sent for charging it is not Available!");
            }
        }

        /// <summary>
        /// Update drone to station
        /// </summary>
        /// <param name="droneId"></param>
        /// <param name="stationId">where the drone is going to charge</param>
        /// <param name="minDistance">the distance between the drone and the station</param>
        public void UpdateDroneToStation(int droneId, int stationId, double minDistance)
        {
            //update for the way to the station
            //finds the drone by its ID
            DroneToList dronel = DronesL.Find(x => x.Id == droneId);
            DO.Station station = new();
            //finds the station by its ID
            station = dalo.GetStation(stationId);
            //update the drone to charging status
            dronel.DroneStatuses = DroneStatuses.Maintenance;
            //update the drone's location to the charging station location - latitude and longitude
            dronel.Location.Latitude = station.Latitude;
            dronel.Location.Longitude = station.Longitude;
            double droneBattery = minDistance * 0.1;
            //the battery is going down on the way to the station
            dronel.Battery -= droneBattery;
            //עידכון עמדות טעינה פנוייות 
            dalo.UpdateChargeSpots(station.Id);
            //הוספת מופע לרשימת הרחפנים בטעינה
            dalo.UpdateAddDroneToCharge(droneId, station.Id);
        }

        /// <summary>
        /// Discharge drone
        /// </summary>
        /// <param name="droneID"></param>
        /// <param name="chargingTime"></param>
        /// <exception cref="Exception"></exception>
        public void DischargeDrone(int droneID, TimeSpan chargingTime)
        {
            // save value in second
            double value = (chargingTime.Hours + chargingTime.Minutes / 100.0 + chargingTime.Seconds / 10000.0) * (chargingTime > TimeSpan.Zero ? 1 : -1);
            //finds the drone by its ID
            DroneToList dronel = DronesL.Find(x => x.Id == droneID);
            DO.Station station = new();
            //only a drone that was in charging c would be discharge
            if (dronel.DroneStatuses == DroneStatuses.Maintenance)
            {
                double droneLocationLatitude = dronel.Location.Latitude;
                double droneLocationLongitude = dronel.Location.Longitude;
                station = dalo.DischargeDroneByLocation(droneID, droneLocationLatitude, droneLocationLongitude);
                DroneInCharging droneInCharge = new();
                dronel.Battery += value * dalo.PowerRequest()[4];
                DronesL.Remove(dronel);
                dronel.DroneStatuses = DroneStatuses.Available;
                DronesL.Add(dronel);
                //remove the drone frome the list of droneChargings
                dalo.UpdateRemoveDroneToCharge(droneID, station.Id);
            }
            else
            {
                throw new Exception("drone can't be discharged");
            }
        }

        /// <summary>
        /// Get drone by ID
        /// </summary>
        /// <param name="droneId"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public Drone GetDrone(int droneId)
        {
            DO.Drone d = dalo.GetDrone(droneId);
            DroneStatuses droneStatuses = DronesL.Find(x => x.Id == droneId).DroneStatuses;
            Drone drone = new()
            {
                Id = d.Id,
                Model = d.Model,
                Battery = d.Battery,
                DroneStatuses = droneStatuses,
                Weight = (Weight)d.MaxWeight
            };
            //to find the locations drone---
            foreach (var item in DronesL)
            {
                if(item.Id==droneId)
                {//
                    drone.Location = new()
                    {
                        Latitude = item.Location.Latitude,
                        Longitude = item.Location.Longitude

                    };
                }
            }
      

            
            if (drone.DroneStatuses != DroneStatuses.Shipping)
            {
                return drone;
            }
            else
            {
                //Package data in transfer mode 
                DO.Parcel parcel = dalo.GetParcelInTransferByDroneId(droneId);
                ParcelInTransfer parcelInTransfer = new()
                {
                    Id = parcel.Id,
                    Priority = (Priority)parcel.Priority,
                    Weight = (Weight)parcel.Weight,
                    ParcelTransferStatus = ParcelTransferStatus.OnTheWayToDestination
                };
                parcelInTransfer.Sender = new()
                {
                    Id = parcel.SenderId,
                };
                drone.ParcelInTransfer = parcelInTransfer;
            }
            return drone;
        }

        /// <summary>
        /// Show LIST of drones
        /// </summary>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public IEnumerable<DroneToList> ShowDroneList()
        {
            List<DroneToList> dronestl = new List<DroneToList>(DronesL);
            return dronestl;

        }
    }
}
