using Loop54.Model.Response;
using System;

namespace Loop54
{
    /// <summary>
    /// Thrown when a request to the engine times out, i.e. the engine did not respond within <see cref="Loop54Settings.RequestTimeoutMs"/> ms.
    /// </summary>
    /// <remarks>
    /// For short timeouts (under about 3 seconds) this may also be thrown for a failure to connect that would normally throw another
    /// <see cref="EngineNotReachableException"/>, e.g. a "connection refused" error or DNS resolution error, because the attempt to connect times
    /// out before the "real" error has a chance to happen.
    /// </remarks>
    public class EngineTimeoutException : EngineNotReachableException
    {
        internal EngineTimeoutException(string message)
            : base(message)
        {
        }
    }
   
    /// <summary>
    /// Thrown when a connection to the engine cannot be made. Could have multiple causes.
    /// </summary>
    public class EngineNotReachableException : Exception
    {
        internal EngineNotReachableException(string message)
            : base(message)
        {
        }

        // Return the full message chain in this exception's message, because sometimes this is all we get in an error report
        internal EngineNotReachableException(string message, Exception innerException)
            : base(message + GetMessageChain(innerException), innerException)
        {
        }

        private static string GetMessageChain(Exception ex)
        {
            if (ex == null)
                return "";
            return " --> " + ex.Message + GetMessageChain(ex.InnerException);
        }
    }

    /// <summary>
    /// Thrown when the response returned from an endpoint cannot be parsed as a valid engine response.
    /// The usually means the endpoint URL does not point to an engine.
    /// </summary>
    public class InvalidEngineResponseException : Exception
    {
        internal InvalidEngineResponseException(string responseText, Exception innerException)
            : base("Unexpected engine response text: " + responseText, innerException)
        {
            ResponseText = responseText;
        }

        public string ResponseText { get; }
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
