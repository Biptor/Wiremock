using Core.Interfaces;
using Core.Domain;

namespace Core.UseCases
{
    public class GetDateTimeProductApiUseCase : IGetDateTimeProductApiUseCase
    {

        private readonly IConnector _connector;

        public GetDateTimeProductApiUseCase(IConnector connector)
        {
            _connector = connector;
        }

        public async Task<string?> ExecuteAsync(string param)
        {
            return await _connector.SendAsync(param) ?? string.Empty;
        }
    }
}