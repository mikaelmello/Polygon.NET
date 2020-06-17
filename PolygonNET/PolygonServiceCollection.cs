using System;
using Microsoft.Extensions.DependencyInjection;
using PolygonNET.Network;

namespace PolygonNET {
    public static class PolygonServiceCollection {
        /// <summary>
        /// Adds singleton services of <see cref="PolygonClient" />, <see cref="PolygonHttpClient" /> and
        /// <see cref="PolygonAuth" />.
        /// </summary>
        /// <param name="services">The service collection.</param>
        /// <param name="setupAction">Setup action to configure the <see cref="PolygonConfiguration" /></param>
        public static IServiceCollection AddPolygonClient(this IServiceCollection services,
                                                          Action<PolygonConfiguration> setupAction) {
            services.Configure(setupAction);
            services.AddHttpServices();
            services.AddSingleton<PolygonClient>();
            return services;
        }
    }
}