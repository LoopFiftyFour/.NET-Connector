using System;
using System.Collections.Generic;
using System.Text;

namespace Loop54
{
    /// <summary>
    /// Interface for handling multiple instances of <see cref="ILoop54Client"/> within the same application.
    /// </summary>
    public interface ILoop54ClientProvider
    {
        /// <summary>
        /// Get a named instance of ILoop54Client. Will throw if an instance with the same name is not found.
        /// </summary>
        /// <param name="instanceName">Name of a instance. For example 'swedish', 'english' or 'content'.</param>
        /// <returns>The named instance.</returns>
        ILoop54Client GetNamed(string instanceName);
    }
}
