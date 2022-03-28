using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    internal class PriorityException : Exception
    {
        private Priority priority;
        private string v;

        public PriorityException()
        {
        }

        public PriorityException(string message) : base(message)
        {
        }
        /// <summary>
        /// Exception- Parcel's priority should be between 1-3
        /// </summary>
        /// <param name="priority"></param>
        /// <param name="v"></param>
        public PriorityException(Priority priority, string v)
        {
            this.priority = priority;
            this.v = v;
        }

        public PriorityException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected PriorityException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}