using System;
using System.Runtime.Serialization;

namespace BL
{
    [Serializable]
    internal class RemoveException : Exception
    {
        public RemoveException()
        {
        }

        public RemoveException(string message) : base(message)
        {
        }

        public RemoveException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected RemoveException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}