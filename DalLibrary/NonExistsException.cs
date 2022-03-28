using System;
using System.Runtime.Serialization;

namespace DAL
{
    [Serializable]
    internal class NonExistsException : Exception
    {
        public NonExistsException()
        {
        }

        public NonExistsException(string message) : base(message)
        {
        }

        public NonExistsException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected NonExistsException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}