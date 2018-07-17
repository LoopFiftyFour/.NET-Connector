using Loop54.Http;
using Loop54.User;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loop54
{
    /// <summary>
    /// Class for handling multiple instances of ILoop54Client. Should be used if working against multiple 
    /// instances of the Loop54 e-commerce search engine from the same application. The class is thread safe.
    /// </summary>
    public class Loop54ClientProvider : ILoop54ClientProvider
    {
        private readonly ConcurrentDictionary<string, ILoop54Client> _clients = new ConcurrentDictionary<string, ILoop54Client>();

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="remoteClientInfoProvider">The client info provider to use in the clients.</param>
        /// <param name="settingsCollection">One or more settings to setup clients for.</param>
        public Loop54ClientProvider(IRemoteClientInfoProvider remoteClientInfoProvider, Loop54SettingsCollection settingsCollection)
        {
            if (remoteClientInfoProvider == null)
                throw new ArgumentNullException(nameof(remoteClientInfoProvider));
            
            if (settingsCollection == null)
                throw new ArgumentNullException(nameof(settingsCollection));

            if (!settingsCollection.Any())
                throw new ArgumentException($"The provided '{nameof(settingsCollection)}' cannot be empty.");

            CreateClientsForSettings(settingsCollection, remoteClientInfoProvider);
        }

        private void CreateClientsForSettings(Loop54SettingsCollection settingsCollection, IRemoteClientInfoProvider remoteClientInfoProvider)
        {
            foreach (var setting in settingsCollection)
            {
                if(!_clients.TryAdd(setting.Key, new Loop54Client(new RequestManager(setting.Value), remoteClientInfoProvider)))
                    throw new ApplicationException($"Could not add '{setting.Key}' client.");
            }
        }

        /// <summary>
        /// Get a named instance of ILoop54Client.
        /// </summary>
        /// <param name="instanceName">Name of a instance. For example 'swedish', 'english' or 'content'.</param>
        /// <returns>The named instance.</returns>
        public ILoop54Client GetNamed(string instanceName)
        {
            if (instanceName == null)
                throw new ArgumentNullException(nameof(instanceName));

            if (_clients.TryGetValue(instanceName, out ILoop54Client client))
                return client;

            throw new ApplicationException($"Loop54 client with instance name '{instanceName}' is not initialized. " +
                $"You must add it to the settings used when initializing.");
        }

        internal ILoop54Client GetSingleOrThrow()
        {
            if (_clients.Count > 1)
                throw new ApplicationException($"Cannot guess a single default client if there are {_clients.Count} registered. " +
                    $"Use the '{nameof(ILoop54ClientProvider)}.{nameof(ILoop54ClientProvider.GetNamed)}' method instead.");

            return _clients.Values.SingleOrDefault() ?? throw new ApplicationException($"No client has been initialized.");
        }
    }
}
