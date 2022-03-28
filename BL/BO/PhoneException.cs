using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    internal class PhoneException : Exception
    {
        private string phone;
        private string v;

        public PhoneException()
        {
        }

        public PhoneException(string message) : base(message)
        {
        }
        /// <summary>
        /// Exception- Phone number is 10 digits (or 9 digits- for a telephone at home)
        /// Exception- Phone number should start with a 0
        /// </summary>
        /// <param name="phone"></param>
        /// <param name="v"></param>
        public PhoneException(string phone, string v)
        {
            this.phone = phone;
            this.v = v;
        }

        public PhoneException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PhoneException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}