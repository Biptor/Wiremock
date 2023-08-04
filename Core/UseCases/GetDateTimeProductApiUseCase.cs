using Core.Interfaces;
using Infrastructure.Interfaces;

namespace Core.UseCases
{
    public class GetDateTimeProductApiUseCase : IGetDateTimeProductApiUseCase
    {
        private readonly IConnector _connector;

        public GetDateTimeProductApiUseCase(IConnector connector)
        {
            _connector = connector;
        }

        public async Task<string?> ExecuteAsync()
        {
            return await _connector.SendAsync("http://localhost:62737/products", RestSharp.Method.Get) ?? string.Empty;
        }
    }
}
