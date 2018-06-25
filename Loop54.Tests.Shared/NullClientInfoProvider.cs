using Loop54.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54.Tests
{
    public class NullClientInfoProvider : IRemoteClientInfoProvider
    {
        public NullClientInfo ClientInfo { get; set; } = new NullClientInfo();

        public IRemoteClientInfo GetRemoteClientInfo()
        {
            return ClientInfo;
        }
    }
}
