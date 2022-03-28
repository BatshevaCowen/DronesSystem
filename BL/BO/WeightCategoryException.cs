using System;
using System.Runtime.Serialization;

namespace BO
{
    [Serializable]
    internal class WeightCategoryException : Exception
    {
        private Weight weight;
        private string v;

        public WeightCategoryException()
        {
        }

        public WeightCategoryException(string message) : base(message)
        {
        }
        /// <summary>
        /// Exception- weight category ust be between 1-3
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="v"></param>
        public WeightCategoryException(Weight weight, string v)
        {
            this.weight = weight;
            this.v = v;
        }

        public WeightCategoryException(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected WeightCategoryException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}