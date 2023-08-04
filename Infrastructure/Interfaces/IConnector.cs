using RestSharp;

namespace Infrastructure.Interfaces
{
    public interface IConnector
    {
        Task<string?> SendAsync(string endpointUrl, Method method);
    }
}
