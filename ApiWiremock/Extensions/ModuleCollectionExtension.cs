using Core.Domain;
using Core.Interfaces;
using Core.UseCases;
using Infrastructure;
using RestSharp;

namespace ApiWireMock.Extensions
{
    public static class ModuleCollectionExtension
    {
        public static IServiceCollection AddCoreModules(this IServiceCollection services)
        {
            // Use Cases
            services.AddSingleton<IGetDateTimeProductApiUseCase, GetDateTimeProductApiUseCase>();
            services.AddSingleton<IUploadFileConnectorUseCase, UploadFileConnectorUseCase>();

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
