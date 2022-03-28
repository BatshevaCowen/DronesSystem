using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DO
{
    [Serializable]
    public class ParcelException : Exception
    {
        private int id;
        private string errMsg;
        private Severity severity;

        public ParcelException()
        {
        }

        public ParcelException(string message) : base(message)
        {
        }

        public ParcelException(string message, Exception innerException) : base(message, innerException)
        {
        }

        public ParcelException(int id, string errMsg, Severity severity) : base(errMsg)
        {
            this.id = id;
            this.errMsg = errMsg;
            this.severity = severity;
        }

        protected ParcelException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class CustomerException : Exception
    {
        public CustomerException()
        {
        }

        public CustomerException(string message) : base(message)
        {
        }

        public CustomerException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected CustomerException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class StationException : Exception
    {
        private int id;
        private string errMsg;

        public StationException()
        {
        }

        public StationException(string message) : base(message)
        {

        }

        internal static Exception IDalNotFound(string v, string typ)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// קורא לפונקציה שמקבלת סטרינג ואז זה יעבור כהודעה
        /// </summary>
        /// <param name="id"></param>
        /// <param name="errMsg"></param>
        public StationException(int id, string errMsg) : this(string.Format("{0} {1]", id, errMsg))
        {
            this.id = id;
            this.errMsg = errMsg;
        }
        /// <summary>
        ///
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public StationException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected StationException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }

    [Serializable]
    public class DroneException : Exception
    {
        private int id;
        private string errMsg;

        public DroneException()
        {
        }

        public DroneException(string message) : base(message)
        {
        }

        public DroneException(int id, string errMsg) : this(string.Format("{0} {1]", id, errMsg))
        {
            this.id = id;
            this.errMsg = errMsg;
        }

        public DroneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected DroneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
    [Serializable]
    public class ReadDataException : Exception
    {
        public ReadDataException() : base() { }
        public ReadDataException(string message) : base(message) { }
        public ReadDataException(string message, Exception inner) : base(message, inner) { }
        protected ReadDataException(SerializationInfo info, StreamingContext context) : base(info, context) { }
    }

    [Serializable]
    public class XMLFileLoadCreateException : Exception
    {
        public string xmlFilePath;
        public XMLFileLoadCreateException(string xmlPath) : base() { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message) :
            base(message)
        { xmlFilePath = xmlPath; }
        public XMLFileLoadCreateException(string xmlPath, string message, Exception innerException) :
            base(message, innerException)
        { xmlFilePath = xmlPath; }
        public override string ToString() => base.ToString() + $", fail to load or create xml file: {xmlFilePath}";
    }

    [Serializable]
    public class BadBusException : Exception
    {
        public int iD;
        public BadBusException(int id) : base() => iD = id;
        public BadBusException(string message, int id) : base(message) => iD = id;
        public BadBusException(string message, Exception inner, int id) : base(message, inner) => iD = id;
        protected BadBusException(SerializationInfo info, StreamingContext context, int id) : base(info, context) => iD = id;
        public override string ToString() => base.ToString() + $", bad Bus id: {iD}";
    }

    [Serializable]
    public class BadLineException : Exception
    {
        public int iD;
        public BadLineException(int id) : base() => iD = id;
        public BadLineException(string message, int id) : base(message) => iD = id;
        public BadLineException(string message, Exception inner, int id) : base(message, inner) => iD = id;
        protected BadLineException(SerializationInfo info, StreamingContext context, int id) : base(info, context) => iD = id;
        public override string ToString() => base.ToString() + $", bad Line id: {iD}";
    }

    [Serializable]
    public class BadStationException : Exception
    {
        public int iD;
        public BadStationException(int id) : base() => iD = id;
        public BadStationException(string message, int id) : base(message) => iD = id;
        public BadStationException(string message, Exception inner, int id) : base(message, inner) => iD = id;
        protected BadStationException(SerializationInfo info, StreamingContext context, int id) : base(info, context) => iD = id;
        public override string ToString() => base.ToString() + $", bad Station id: {iD}";
    }

    [Serializable]
    public class BadUserException : Exception
    {
        public string userName;
        public BadUserException(string user) : base() => userName = user;
        public BadUserException(string message, string user) : base(message) => userName = user;
        public BadUserException(string message, Exception inner, string user) : base(message, inner) => userName = user;
        protected BadUserException(SerializationInfo info, StreamingContext context, string user) : base(info, context) => userName = user;
        public override string ToString() => base.ToString() + $", bad User name {userName}";
    }

    [Serializable]
    public class BadLineStationException : Exception
    {
        public int line;
        public int station;
        public BadLineStationException(int tmpLine, int tmpStation) : base() { line = tmpLine; station = tmpStation; }

        public BadLineStationException(string message, int tmpLine, int tmpStation) : base(message) { line = tmpLine; station = tmpStation; }
        public BadLineStationException(string message, Exception inner, int tmpLine, int tmpStation) : base(message, inner) { line = tmpLine; station = tmpStation; }
        protected BadLineStationException(SerializationInfo info, StreamingContext context, int tmpLine, int tmpStation) : base(info, context) { line = tmpLine; station = tmpStation; }
        public override string ToString() => base.ToString() + $", bad LineStation : {"/r"} line :{line}, station:{station}";
    }

    [Serializable]
    [Obsolete("This Class wasn't used in the project, needs more implementation")]
    public class BadUserTripException : Exception
    {
        public string userName;
        public BadUserTripException(string user) : base() => userName = user;
        public BadUserTripException(string message, string user) : base(message) => userName = user;
        public BadUserTripException(string message, Exception inner, string user) : base(message, inner) => userName = user;
        protected BadUserTripException(SerializationInfo info, StreamingContext context, string user) : base(info, context) => userName = user;
        public override string ToString() => base.ToString() + $", bad UserTrip name {userName}";
    }

    [Serializable]
    public class BadPairStationException : Exception
    {
        public int firstStation;
        public int lastStation;
        public BadPairStationException(int tmpFirstStation, int tmpLastStation) : base() { firstStation = tmpFirstStation; lastStation = tmpLastStation; }

        public BadPairStationException(string message, int tmpFirstStation, int tmpLastStation) : base(message) { firstStation = tmpFirstStation; lastStation = tmpLastStation; }
        public BadPairStationException(string message, Exception inner, int tmpFirstStation, int tmpLastStation) : base(message, inner) { firstStation = tmpFirstStation; lastStation = tmpLastStation; }
        protected BadPairStationException(SerializationInfo info, StreamingContext context, int tmpFirstStation, int tmpLastStation) : base(info, context) { firstStation = tmpFirstStation; lastStation = tmpLastStation; }
        public override string ToString() => base.ToString() + $", bad PairStation : {"/r"} firstStation :{firstStation}, lastStation:{lastStation}";
    }

    [Serializable]
    [Obsolete("This Class wasn't used in the project, needs more implementation")]
    public class BadBusInTravelException : Exception
    {
        public int license;
        public int lineNumber;
        public DateTime departureTime;
        public BadBusInTravelException(int tmpLicense, int tmpLineNumber, DateTime tmpDepartureTime) : base() { license = tmpLicense; lineNumber = tmpLineNumber; departureTime = tmpDepartureTime; }
        public BadBusInTravelException(string message, int tmpLicense, int tmpLineNumber, DateTime tmpDepartureTime) : base(message) { license = tmpLicense; lineNumber = tmpLineNumber; departureTime = tmpDepartureTime; }
        public BadBusInTravelException(string message, Exception inner, int tmpLicense, int tmpLineNumber, DateTime tmpDepartureTime) : base(message, inner) { license = tmpLicense; lineNumber = tmpLineNumber; departureTime = tmpDepartureTime; }
        protected BadBusInTravelException(SerializationInfo info, StreamingContext context, int tmpLicense, int tmpLineNumber, DateTime tmpDepartureTime) : base(info, context) { license = tmpLicense; lineNumber = tmpLineNumber; departureTime = tmpDepartureTime; }
        public override string ToString() => base.ToString() + $", bad BusInTravel: license = {license}, lineNumber = {lineNumber}, departureTime = {departureTime}";
    }

    [Serializable]
    public class BadLineDepartingException : Exception
    {
        public int lineNumber;
        public TimeSpan startTime;
        public BadLineDepartingException(int tmpLineNumber, TimeSpan tmpStartTime) : base() { lineNumber = tmpLineNumber; startTime = tmpStartTime; }
        public BadLineDepartingException(string message, int tmpLineNumber, TimeSpan tmpStartTime) : base(message) { lineNumber = tmpLineNumber; startTime = tmpStartTime; }
        public BadLineDepartingException(string message, Exception inner, int tmpLineNumber, TimeSpan tmpStartTime) : base(message, inner) { lineNumber = tmpLineNumber; startTime = tmpStartTime; }
        protected BadLineDepartingException(SerializationInfo info, StreamingContext context, int tmpLineNumber, TimeSpan tmpStartTime) : base(info, context) { lineNumber = tmpLineNumber; startTime = tmpStartTime; }
        public override string ToString() => base.ToString() + $", bad LineDeparting: lineNumber = {lineNumber}, startTime = {startTime}";
    }
}