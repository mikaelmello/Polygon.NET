using Microsoft.Extensions.DependencyInjection;

namespace PolygonNET.Utils {
    public static class UtilsServiceCollection {
        public static IServiceCollection AddUtilsServices(this IServiceCollection services)
        {
            services.AddSingleton<ICryptoUtils, CryptoUtils>();
            services.AddSingleton<IRandomUtils, RandomUtils>();
            return services;
        }
    }
}