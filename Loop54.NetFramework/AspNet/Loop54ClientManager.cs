using Loop54.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Loop54.AspNet
{
    /// <summary>
    /// Static class used to manage the <see cref="ILoop54Client"/> used by the application.
    /// </summary>
    public static class Loop54ClientManager
    {
        private static ILoop54Client _loop54Client;

        /// <summary>
        /// Run this method once at the startup of your application. It will create a <see cref="ILoop54Client"/> with the provided settings. To retrieve the client use 
        /// <see cref="Client"/>. If run more than once a new <see cref="ILoop54Client"/> will be created and overwrite the existing each time.
        /// </summary>
        /// <param name="endpoint">Your Loop54 endpoint. This must be HTTPS by default. If you cannot use HTTPS use <see cref="StartUp(Loop54Settings)"/> instead.</param>
        public static void StartUp(string endpoint) => StartUp(new Loop54Settings(endpoint));

        /// <summary>
        /// Run this method once at the startup of your application. It will create a <see cref="ILoop54Client"/> with the provided settings. To retrieve the client use 
        /// <see cref="Client"/>. If run more than once a new <see cref="ILoop54Client"/> will be created and overwrite the existing each time.
        /// </summary>
        /// <param name="settings">The settings the loop54 client should use.</param>
        public static void StartUp(Loop54Settings settings)
        {
            var infoProvider = new HttpContextInfoProvider();
            var requestManager = new RequestManager(settings);

            _loop54Client = new Loop54Client(requestManager, infoProvider);
        }

        /// <summary>
        /// Get the instantiated <see cref="ILoop54Client"/>. If <see cref="StartUp(string)"/> or <see cref="StartUp(Loop54Settings)"/> hasn't been executed an exception will be thrown.
        /// </summary>
        /// <returns>The <see cref="ILoop54Client"/> singleton instance.</returns>
        public static ILoop54Client Client()
        {
            if (_loop54Client == null)
                throw new ApplicationException($"Loop54 client is not initialized. Please run {nameof(StartUp)} once before using the client.");

            return _loop54Client;
        }
    }
}
