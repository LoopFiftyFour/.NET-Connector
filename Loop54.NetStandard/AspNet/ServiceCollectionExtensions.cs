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
        /// Adds the Loop54 client as a singleton service on the <see cref="IServiceCollection"/>. The Loop54 library depends on <see cref="IHttpContextAccessor"/> and that service needs to be added as a singleton before calling this.
        /// </summary>
        /// <param name="serviceCollection">The serviceCollection the singleton should be added to.</param>
        /// <param name="endpoint">Your Loop54 endpoint. This must be HTTPS by default. If you cannot use HTTPS use <see cref="AddLoop54(IServiceCollection, Loop54Settings)"/> instead.</param>
        /// <returns>The serviceCollection</returns>
        public static IServiceCollection AddLoop54(this IServiceCollection serviceCollection, string endpoint) => serviceCollection.AddLoop54(new Loop54Settings(endpoint));

        /// <summary>
        /// Adds the Loop54 client as a singleton service on the <see cref="IServiceCollection"/>. The Loop54 library depends on <see cref="IHttpContextAccessor"/> and that service needs to be added as a singleton before calling this.
        /// </summary>
        /// <param name="serviceCollection">The serviceCollection the singleton should be added to.</param>
        /// <param name="setting">The settings the loop54 client should use.</param>
        /// <returns>The serviceCollection</returns>
        public static IServiceCollection AddLoop54(this IServiceCollection serviceCollection, Loop54Settings setting)
        {
            //Assert IHttpContextAccessor service has been added.
            if (!serviceCollection.Any(sd => sd.ServiceType == typeof(IHttpContextAccessor) && sd.Lifetime == ServiceLifetime.Singleton))
                throw new ApplicationException($"No Singleton {nameof(IHttpContextAccessor)} service has been added to the {nameof(IServiceCollection)}. The Loop54 library depends on it.");

            serviceCollection.AddSingleton(setting);
            serviceCollection.AddSingleton<IRequestManager, RequestManager>();
            serviceCollection.AddSingleton<IRemoteClientInfoProvider, HttpContextInfoProvider>();
            serviceCollection.AddSingleton<ILoop54Client, Loop54Client>();

            return serviceCollection;
        }
    }
}
