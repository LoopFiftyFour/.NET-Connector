using Loop54.Model.Response;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54
{
    
    /// <summary>
    /// Thrown when a connection to the engine cannot be made. Could have multiple causes.
    /// </summary>
    public class EngineNotReachableException : Exception
    {
        internal EngineNotReachableException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Thrown when the engine is reachable, but the response status code was not 200.
    /// </summary>
    public class EngineStatusCodeException : Exception
    {
        /// <summary>
        /// Details about the error provided by the engine.
        /// </summary>
        public ErrorDetails Details { get; set; }

        internal EngineStatusCodeException(ErrorDetails details)
            : base(details.Title)
        {
            Details = details;
        }
    }
    
    /// <summary>
    /// Thrown when the data returned from the engine could not be serialized or deserialize.
    /// </summary>
    public class SerializationException : Exception
    {
        internal SerializationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }

    /// <summary>
    /// Thrown when the we cannot read the client info properly.
    /// </summary>
    public class ClientInfoException : Exception
    {
        internal ClientInfoException(string message)
            : base(message)
        {
        }
    }

    /// <summary>
    /// Thrown when the custom data returned from the engine could not be retrieved properly.
    /// </summary>
    public class CustomDataException : Exception
    {
        /// <summary>
        /// The data that failed to be deserialized or cast. 
        /// Will be null if no data was found at all.
        /// </summary>
        public object FailedData { get; }

        internal CustomDataException(string message, object data = null)
            : base(message)
        {
            FailedData = data;
        }

        internal CustomDataException(string message, Exception innerException, object data = null)
            : base(message, innerException)
        {
            FailedData = data;
        }
    }
}
