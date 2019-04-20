using System;

namespace Sesim.Models.Exceptions
{
    /// <summary>
    /// An exception indicating that the object currently reading does not meet the expectation 
    /// of what should be
    /// </summary>
    [System.Serializable]
    public class DeformedObjectException : System.Exception
    {
        public DeformedObjectException() { }
        public DeformedObjectException(string message) : base(message) { }
        public DeformedObjectException(string message, System.Exception inner) : base(message, inner) { }
        protected DeformedObjectException(
            System.Runtime.Serialization.SerializationInfo info,
            System.Runtime.Serialization.StreamingContext context) : base(info, context) { }
    }
}