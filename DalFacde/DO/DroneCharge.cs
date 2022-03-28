using System;

    namespace DO
    {
        public struct DroneCharge
        {
            public int StationId { get; set; }
            public int DroneId { get; set; }

            /// <summary>
            /// ToString
            /// </summary>
            /// <returns></returns>
            public override string ToString()
            {
                String result = "";
                result += $"ID is {StationId}, \n";
                result += $"Name is {DroneId}, \n";
                return result;
            }
        }
    }