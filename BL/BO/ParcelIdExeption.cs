using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    internal class ParcelIdExeption : Exception
    {
        private int id;
        private string v;

        public ParcelIdExeption()
        {
        }

        public ParcelIdExeption(string message) : base(message)
        {
        }
        /// <summary>
        /// Exception- Parcel ID must be 7-10 digits
        /// </summary>
        /// <param name="id"></param>
        /// <param name="v"></param>
        public ParcelIdExeption(int id, string v)
        {
            this.id = id;
            this.v = v;
        }

        public ParcelIdExeption(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParcelIdExeption(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}