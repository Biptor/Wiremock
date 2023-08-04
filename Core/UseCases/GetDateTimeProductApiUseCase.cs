using Core.Interfaces;
using Infrastructure.Interfaces;

namespace Core.UseCases
{
    public class GetDateTimeProductApiUseCase : IGetDateTimeProductApiUseCase
    {
        private const string PRODUCT_API_URL = "http://localhost:62737/products";
        private readonly IConnector _connector;

        public GetDateTimeProductApiUseCase(IConnector connector)
        {
            _connector = connector;
        }

        public async Task<string?> ExecuteAsync()
        {
            return await _connector.SendAsync(PRODUCT_API_URL, RestSharp.Method.Get) ?? string.Empty;
        }
    }
}