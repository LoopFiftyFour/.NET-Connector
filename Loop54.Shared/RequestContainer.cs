using Loop54.Model.Request;
using Loop54.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54
{
    /// <summary>
    /// This class wraps a request object together with options for the api call.
    /// </summary>
    /// <typeparam name="T">A <see cref="Model.Request.Request"/> type</typeparam>
    public class RequestContainer<T> where T : Request
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="requestData">The request to wrap.</param>
        public RequestContainer(T requestData)
        {
            Request = requestData;
        }

        /// <summary>
        /// Contains the request data to send to the engine.
        /// </summary>
        public T Request { get; set; }
        
        /// <summary>
        /// Overrides for client meta data. Should be used if the UserId needs to be set from your 
        /// internal customer id. If no override is provided all the meta-data will be taken from 
        /// the current <see cref="IRemoteClientInfo"/>.
        /// </summary>
        public UserMetaData MetaDataOverrides { get; set; }
    }
}
