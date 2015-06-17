using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.Exceptions
{
    

    public class EngineNotFoundException:Exception
    {
        internal EngineNotFoundException(string url, Exception innerException)
            : base("Could not connect to Loop54 at \"" + url + "\".", innerException)
        {
        }
    }

    public class EngineErrorException : Exception
    {
        internal EngineErrorException(string url, Exception innerException)
            : base("Loop54 Engine at \"" + url + "\" responded with an error.", innerException)
        {
        }
    }

    public class DataNotFoundException : Exception
    {
        internal DataNotFoundException(string dataIndex)
            :base("Could not find data \"" + dataIndex + "\".")
        {
        }
    }

    public class WrongTypeException : Exception
    {
        internal WrongTypeException(string dataIndex, Type expectedType, Type actualType)
            : base(
                "Data at key \"" + dataIndex + "\" was expected to be of type \"" + expectedType + "\", but was \"" +
                actualType + "\".")

        {
        }
    }

    public class DeserializationException : Exception
    {
        internal DeserializationException()
            : base("Could not deserialize data from engine.")
        {
        }
    }
}
