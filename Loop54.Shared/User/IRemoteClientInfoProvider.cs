using System;

namespace Loop54.User
{
    /// <summary>
    /// An interface for creating <see cref="IRemoteClientInfo"/>. If not running Asp.Net or Asp.Net Core this needs to be implemented by the user of the Loop54 library.
    /// </summary>
    public interface IRemoteClientInfoProvider
    {
        /// <summary>
        /// Creates and returns a <see cref="IRemoteClientInfo"/> that contains information of the current, calling end-user (headers, cookies etc)
        /// </summary>
        /// <returns>A new <see cref="IRemoteClientInfo"/></returns>
        IRemoteClientInfo GetRemoteClientInfo();
    }
}
