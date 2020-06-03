using System;
using Microsoft.Extensions.DependencyInjection;
using PolygonNET.Network;

namespace PolygonNET {
    public static class PolygonServiceCollection {
        public static IServiceCollection AddPolygonClient(this IServiceCollection services,
                                                    Action<PolygonConfiguration> setupAction) {
            services.Configure(setupAction);
            services.AddNetworkServices();
            services.AddSingleton<PolygonClient>();
            return services;
        }
    }
}