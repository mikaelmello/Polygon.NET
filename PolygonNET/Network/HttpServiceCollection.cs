using Microsoft.Extensions.DependencyInjection;

namespace PolygonNET.Network {
    internal static class HttpServiceCollection {
        /// <summary>
        /// Adds services related to http calls to the API: PolygonAuth and PolygonHttpClient.
        /// </summary>
        public static IServiceCollection AddHttpServices(this IServiceCollection services) {
            services.AddSingleton<IPolygonAuth, PolygonAuth>();
            services.AddSingleton<IPolygonHttpClient, PolygonHttpClient>();
            return services;
        }
    }
}