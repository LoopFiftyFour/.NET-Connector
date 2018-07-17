using Loop54.Http;
using Loop54.User;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Loop54.AspNet
{
    /// <summary>
    /// Extensions methods for installing the Loop54 client as a service in ASP.NET Core.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Adds the <see cref="ILoop54Client"/> as a singleton service on the <see cref="IServiceCollection"/>. The Loop54 library depends on <see cref="IHttpContextAccessor"/> and that service needs to be added as a singleton before calling this.
        /// </summary>
        /// <param name="serviceCollection">The serviceCollection the singleton should be added to.</param>
        /// <param name="endpoint">Your Loop54 endpoint. This must be HTTPS by default. If you cannot use HTTPS use <see cref="AddLoop54(IServiceCollection, Loop54Settings)"/> instead.</param>
        /// <returns>The serviceCollection</returns>
        public static IServiceCollection AddLoop54(this IServiceCollection serviceCollection, string endpoint) => serviceCollection.AddLoop54(new Loop54Settings(endpoint));

        /// <summary>
        /// Adds the <see cref="ILoop54Client"/> as a singleton service on the <see cref="IServiceCollection"/>. The Loop54 library depends on <see cref="IHttpContextAccessor"/> and that service needs to be added as a singleton before calling this.
        /// </summary>
        /// <param name="serviceCollection">The serviceCollection the singleton should be added to.</param>
        /// <param name="setting">The settings the loop54 client should use.</param>
        /// <returns>The serviceCollection</returns>
        public static IServiceCollection AddLoop54(this IServiceCollection serviceCollection, Loop54Settings setting)
        {
            serviceCollection.AddLoop54(Loop54SettingsCollection.Create().Add("Default", setting));
            return serviceCollection;
        }

        /// <summary>
        ///  Adds the <see cref="ILoop54ClientProvider"/> as a singleton service on the <see cref="IServiceCollection"/>. The Loop54 library depends on <see cref="IHttpContextAccessor"/> and that service needs to be added as a singleton before calling this.
        /// </summary>
        /// <param name="serviceCollection">The serviceCollection the singleton should be added to.</param>
        /// <param name="settings">A collection of named settings the clients should use. These names will map to the name provided to <see cref="ILoop54ClientProvider.GetNamed(string)"/>.</param>
        /// <returns>The serviceCollection</returns>
        public static IServiceCollection AddLoop54(this IServiceCollection serviceCollection, Loop54SettingsCollection settings)
        {
            if(!settings.Any())
                throw new ApplicationException($"You must supply at least one {nameof(Loop54Settings)} in the settings collection.");

            //Assert IHttpContextAccessor service has been added.
            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(IHttpContextAccessor) && sd.Lifetime == ServiceLifetime.Singleton))
                throw new ApplicationException($"No Singleton {nameof(IHttpContextAccessor)} service has been added to the {nameof(IServiceCollection)}. The Loop54 library depends on it.");
            
            serviceCollection.AddSingleton(settings);
            serviceCollection.AddSingleton<IRemoteClientInfoProvider, HttpContextInfoProvider>();
            serviceCollection.AddSingleton<ILoop54ClientProvider, Loop54ClientProvider>();
            serviceCollection.AddSingleton((serviceProvider) =>
            {
                ILoop54ClientProvider clientProvider = serviceProvider.GetService<ILoop54ClientProvider>();
                if (clientProvider is Loop54ClientProvider loop54ClientProvider)
                    return loop54ClientProvider.GetSingleOrThrow();

                throw new ApplicationException($"Cannot get a single, default {nameof(ILoop54Client)} if not using {nameof(Loop54ClientProvider)} as {nameof(ILoop54ClientProvider)}");
            });

            return serviceCollection;
        }
    }
}
