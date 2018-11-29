using Loop54.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54
{
    /// <summary>
    /// A client info provider that returns null values for all user-specific metadata such as Id, Referrer, etc. 
    /// Useful for implementing engine requests where there is no context, such as batch jobs.
    /// </summary>
    public class NullClientInfoProvider : IRemoteClientInfoProvider
    {
        public NullClientInfo ClientInfo { get; set; } = new NullClientInfo();

        public IRemoteClientInfo GetRemoteClientInfo()
        {
            return ClientInfo;
        }
    }
}
