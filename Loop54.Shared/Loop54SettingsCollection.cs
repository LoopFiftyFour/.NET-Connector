using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;

namespace Loop54
{
    /// <summary>
    /// Class containing a named settings for the Loop54 search engine library.
    /// </summary>
    public class Loop54SettingsCollection : IEnumerable<KeyValuePair<string, Loop54Settings>>
    {
        private readonly Dictionary<string, Loop54Settings> _settings = new Dictionary<string, Loop54Settings>();
        
        private Loop54SettingsCollection()
        {
        }
        
        /// <summary>
        /// Will create and return a new, empty instance of <see cref="Loop54SettingsCollection"/>.
        /// </summary>
        /// <returns>New instance of <see cref="Loop54SettingsCollection"/>.</returns>
        public static Loop54SettingsCollection Create() => new Loop54SettingsCollection();

        /// <summary>
        /// Adds a named endpoint setting to the collection.
        /// </summary>
        /// <param name="instanceName">Name of the setting instance. For example 'swedish', 'english' or 'content'.</param>
        /// <param name="endpoint">The endpoint to affiliate with the instance.</param>
        /// <returns>The Loop54SettingsCollection instance. For chaining.</returns>
        public Loop54SettingsCollection Add(string instanceName, string endpoint) 
            => Add(instanceName, new Loop54Settings(endpoint ?? throw new ArgumentNullException(nameof(endpoint))));

        /// <summary>
        /// Adds a named setting to the collection.
        /// </summary>
        /// <param name="instanceName">Name of the setting instance. For example 'swedish', 'english' or 'content'.</param>
        /// <param name="settings">The settings to affiliate with the instance.</param>
        /// <returns>The Loop54SettingsCollection instance. For chaining.</returns>
        public Loop54SettingsCollection Add(string instanceName, Loop54Settings settings)
        {
            if (settings == null)
                throw new ArgumentNullException(nameof(settings));

            if (instanceName == null)
                throw new ArgumentNullException(nameof(instanceName));

            if (_settings.ContainsKey(instanceName))
                throw new ApplicationException($"There's already a '{instanceName}' in the collection. Cannot add it again.");

            _settings.Add(instanceName, settings);
            return this;
        }
        
        /// <summary>
        /// Implementation of <see cref="IEnumerable{T}.GetEnumerator"/>.
        /// </summary>
        /// <returns>Enumerator</returns>
        public IEnumerator<KeyValuePair<string, Loop54Settings>> GetEnumerator()
        {
            return _settings.GetEnumerator();
        }

        /// <summary>
        /// Implementation of <see cref="IEnumerable.GetEnumerator"/>.
        /// </summary>
        /// <returns>Enumerator</returns>
        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
