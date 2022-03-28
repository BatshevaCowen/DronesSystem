using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    internal class StationException : Exception
    {
        private int id;
        private string errMsg;

        public StationException()
        {
        }

        public StationException(string message) : base(message)
        {

        }
        /// <summary>
        /// קורא לפונקציה שמקבלת סטרינג ואז זה יעבור כהודעה
        /// </summary>
        /// <param name="id"></param>
        /// <param name="errMsg"></param>
        public StationException(int id, string errMsg) : this(string.Format("{0} {1}", id, errMsg))
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
}
