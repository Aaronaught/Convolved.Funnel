using System;
using System.Runtime.Serialization;

namespace Convolved.Funnel
{
    /// <summary>
    /// The base class for all exceptions in the Funnel framework.
    /// </summary>
    [Serializable]
    public class FunnelException : Exception
    {
        /// <inheritdoc cref="Exception()" />
        /// <summary>
        /// Initializes a new instance of the <see cref="FunnelException"/> class.
        /// </summary>
        public FunnelException()
        {
        }

        /// <inheritdoc cref="Exception(String)" />
        /// <summary>
        /// Initializes a new instance of the <see cref="FunnelException"/> class with a specified
        /// error message.
        /// </summary>
        public FunnelException(string message) 
            : base(message)
        {
        }

        /// <inheritdoc cref="Exception(String, Exception)" />
        /// <summary>
        /// Initializes a new instance of the <see cref="FunnelException"/> class with a specified
        /// error message and a reference to the inner exception that is the cause of this exception.
        /// </summary>
        public FunnelException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <inheritdoc cref="Exception(SerializationInfo, StreamingContext)" />
        /// <summary>
        /// Initializes a new instance of the <see cref="FunnelException"/> class with serialized data.
        /// </summary>
        protected FunnelException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }
}