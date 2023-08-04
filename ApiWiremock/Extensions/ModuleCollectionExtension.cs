using Core.Interfaces;
using Core.UseCases;
using Infrastructure;
using Infrastructure.Interfaces;
using RestSharp;

namespace ApiWireMock.Extensions
{
    public static class ModuleCollectionExtension
    {
        public static IServiceCollection AddCoreModules(this IServiceCollection services)
        {
            // Use Cases
            services.AddSingleton<IGetDateTimeProductApiUseCase, GetDateTimeProductApiUseCase>();

            return services;
        }

        public static IServiceCollection AddInfrastructureModules(this IServiceCollection services)
        {
            services.AddSingleton<IRestClient, RestClient>();
            services.AddSingleton<IConnector, Connector>();
            
            return services;
        }
    }
}
