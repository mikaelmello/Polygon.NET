using Microsoft.Extensions.DependencyInjection;

namespace PolygonNET.Network {
    internal static class NetworkServiceCollection {
        public static IServiceCollection AddNetworkServices(this IServiceCollection services) {
            services.AddSingleton<IPolygonAuth, PolygonAuth>();
            services.AddSingleton<IPolygonHttpClient, PolygonHttpClient>();
            return services;
        }
    }
}