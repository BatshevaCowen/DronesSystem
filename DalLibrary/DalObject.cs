using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DalApi;
using DO;


namespace DAL
{
    /// <summary>
    /// constractor 
    /// </summary>
    internal sealed partial class DalObject : IDal
    {
        #region Definition
        //singelton
        //internal static IDal Instance;

        static readonly IDal instance = new DalObject();

        internal static IDal Instance { get => instance; }

        static DalObject() { }

        /// <summary>
        /// Initialize
        /// </summary>
        private DalObject()
        {
            DataSource.Initialize();
        }

        public double[] PowerRequest()
        {

            double[] arr = new double[5]{

            DataSource.Config.Available,
            DataSource.Config.Heavy,
            DataSource.Config.Light,
            DataSource.Config.Medium,
            DataSource.Config.ChargingRate };
            return arr;
        }

        /// <summary>
        /// Looking for the closest station with available charging spots
        /// </summary>
        /// <param name="longitude"></param>
        /// <param name="latitude"></param>
        /// <returns></returns>
        public List<Distance> MinimumDistance(double longitude, double latitude)
        {
            List<Distance> listDis = new List<Distance>();
            foreach (Station element in DataSource.Stations)
            {
                Distance Distance = new() { };
                double dis;
                dis = (element.Longitude - longitude) * (element.Longitude - longitude) + (element.Latitude - longitude) * (element.Latitude - longitude);
                dis = Math.Sqrt(dis);
                Distance.Id = element.Id;
                Distance.Length = dis;

                listDis.Add(Distance);
            }
            return listDis;
        }
        //--BONUS--: another option that recives coordinates and print the distance from it to a station or a customer
        public double CalculateDistance(double longitude1, double latitude1, double longitude2, double latitude2)
        {
            // this function is from https://stackoverflow.com/questions/27928/calculate-distance-between-two-latitude-longitude-points-haversine-formula
            // (with changes for C#)
            var p = 0.017453292519943295;    // Math.PI / 180
            var a = 0.5 - Math.Cos((latitude2 - latitude1) * p) / 2 +
                    Math.Cos(latitude1 * p) * Math.Cos(latitude2 * p) *
                    (1 - Math.Cos((longitude2 - longitude1) * p)) / 2;

            return 12742 * Math.Asin(Math.Sqrt(a)); // 2 * R; R = 6371 km
        }
        #endregion
    }
}
