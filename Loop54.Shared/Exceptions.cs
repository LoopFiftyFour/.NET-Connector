using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.Exceptions
{
    
    /// <summary>
    /// Thrown when a connection to the engine cannot be made. Could have multiple causes.
    /// </summary>
    public class EngineNotFoundException:Exception
    {
        internal EngineNotFoundException(string url, Exception innerException)
            : base("Could not connect to Loop54 at \"" + url + "\".", innerException)
        {
        }
    }

    /// <summary>
    /// Thrown when the engine is reachable, but the response status code was 500.
    /// </summary>
    public class EngineErrorException : Exception
    {
        internal EngineErrorException(string url, Exception innerException)
            : base("Loop54 Engine at \"" + url + "\" responded with an error.", innerException)
        {
        }
    }

    /// <summary>
    /// Thrown when the requested data was not found on the response object.
    /// </summary>
    public class DataNotFoundException : Exception
    {
        internal DataNotFoundException(string dataIndex)
            :base("Could not find data \"" + dataIndex + "\".")
        {
        }
    }

    /// <summary>
    /// Thrown when data on the request object was not of the requested type.
    /// </summary>
    public class WrongTypeException : Exception
    {
        internal WrongTypeException(string dataIndex, Type expectedType, Type actualType)
            : base(
                "Data at key \"" + dataIndex + "\" was expected to be of type \"" + expectedType + "\", but was \"" +
                actualType + "\".")

        {
        }
    }

    /// <summary>
    /// Thrown when the data returned from the engine could not be deserialized.
    /// </summary>
    public class DeserializationException : Exception
    {
        internal DeserializationException()
            : base("Could not deserialize data from engine.")
        {
        }
    }
}
