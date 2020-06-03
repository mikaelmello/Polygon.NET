using Microsoft.Extensions.DependencyInjection;

namespace PolygonNET.Utils {
    internal static class UtilsServiceCollection {
        public static IServiceCollection AddUtilsServices(this IServiceCollection services)
        {
            services.AddSingleton<ICryptoUtils, CryptoUtils>();
            services.AddSingleton<IRandomUtils, RandomUtils>();
            return services;
        }
    }
}