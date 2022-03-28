using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class DroneBatteryException : Exception
    {
        public DroneBatteryException()
        {
        }

        public DroneBatteryException(string message) : base(message)
        {
        }

        public DroneBatteryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneBatteryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}