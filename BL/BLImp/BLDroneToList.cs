using BlApi;
using BO;

namespace BL
{
    internal sealed partial class BL : IBL
    {
        /// <summary>
        /// Add drone to list
        /// </summary>
        /// <param name="drone"></param>
        /// <param name="station"></param>
        public void AddDroneToList(Drone drone, double latitude, double longitude)
        {
            DroneToList droneToList = new()
            {
                Id = drone.Id,
                Model = drone.Model,
                Battery = drone.Battery,
                DroneStatuses = drone.DroneStatuses,
                Weight = drone.Weight,
            };
            droneToList.Location = new()
            {
                Latitude = latitude,
                Longitude = longitude,
            };
            if (drone.ParcelInTransfer.Id != 0)
            {
                droneToList.ParcelNumberTransferred = drone.ParcelInTransfer.Id;
            }
            else
                droneToList.ParcelNumberTransferred = 0;
            DronesL.Add(droneToList);
        }
    }
}
    

