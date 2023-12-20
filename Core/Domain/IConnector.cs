using Microsoft.AspNetCore.Http;

namespace Core.Domain
{
    public interface IConnector
    {
        Task<string?> SendAsync(string endpointUrl);
        Task<bool> SendFileAsync(string endpointUrl, IFormFile file, string rutaFullCompleta);
    }
}
