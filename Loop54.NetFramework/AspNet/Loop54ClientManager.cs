using System;
using System.Linq;

namespace Loop54.AspNet
{
    /// <summary>
    /// Static class used to manage the <see cref="ILoop54Client"/> used by the application.
    /// </summary>
    public static class Loop54ClientManager
    {
        private static Loop54ClientProvider _clients;

        /// <summary>
        /// Run this method once at the startup of your application. It will create a <see cref="ILoop54Client"/> with the provided settings. To
        /// retrieve the client use  <see cref="Client()"/>. If run more than once a new <see cref="ILoop54Client"/> will be created and will
        /// overwrite the existing one each time. If you need to use multiple loop54 endpoints use <see cref="StartUp(Loop54Settings)"/>.
        /// </summary>
        /// <param name="endpoint">
        /// Your Loop54 endpoint. This must be HTTPS by default. If you cannot use HTTPS use <see cref="StartUp(Loop54Settings)"/> instead.
        /// </param>
        public static void StartUp(string endpoint) 
            => StartUp(new Loop54Settings(endpoint ?? throw new ArgumentNullException(nameof(endpoint))));

        /// <summary>
        /// Run this method once at the startup of your application. It will create a <see cref="ILoop54Client"/> with the provided settings. To
        /// retrieve the client use <see cref="Client()"/>. If run more than once a new <see cref="ILoop54Client"/> will be created and will
        /// overwrite the existing one each time. If you need to use multiple loop54 endpoints use <see cref="StartUp(Loop54SettingsCollection)"/>.
        /// </summary>
        /// <param name="settings">The settings the loop54 client should use.</param>
        public static void StartUp(Loop54Settings settings) 
            => StartUp(Loop54SettingsCollection.Create().Add("Default", settings ?? throw new ArgumentNullException(nameof(settings))));

        /// <summary>
        /// Run this method once at the startup of your application. It will create a named <see cref="ILoop54Client"/> per setting. To retrieve the
        /// client use <see cref="Client(string)"/> using the same name. If run more than once new <see cref="ILoop54Client"/>s will be created and
        /// will overwrite the existing ones.
        /// </summary>
        /// <param name="settings">The settings the loop54 client should use.</param>
        public static void StartUp(Loop54SettingsCollection settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (!settings.Any())
                throw new ArgumentException("The settings collection must not be empty.");

            _clients = new Loop54ClientProvider(new HttpContextInfoProvider(), settings);
        }

        /// <summary>
        /// Get the single default <see cref="ILoop54Client"/>. If <see cref="StartUp(string)"/> or <see cref="StartUp(Loop54Settings)"/> hasn't been
        /// executed an exception will be thrown.
        /// </summary>
        /// <returns>The <see cref="ILoop54Client"/> singleton instance.</returns>
        public static ILoop54Client Client() => CheckClient().GetSingleOrThrow();

        /// <summary>
        /// Get a named instance of the <see cref="ILoop54Client"/>. If <see cref="StartUp(Loop54SettingsCollection)"/> hasn't been run with the same
        /// instance name, an exception will be thrown.
        /// </summary>
        /// <param name="instanceName">Name of the client instance, for example 'swedish', 'english' or 'content'</param>
        /// <returns>The named instance.</returns>
        public static ILoop54Client Client(string instanceName) => CheckClient().GetNamed(instanceName);

        private static Loop54ClientProvider CheckClient()
        {
            if (_clients == null)
                throw new ApplicationException($"The Loop54 client is not initialized. Please run {nameof(StartUp)} before getting the client.");

            return _clients;
        }
    }
}
