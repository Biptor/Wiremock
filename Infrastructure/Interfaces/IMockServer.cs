using Microsoft.AspNetCore.Http;

namespace Infrastructure.Interfaces
{
    public interface IMockServer
    {
        void Start();
        void Stop();
        void LoadResponse(string endpointPath, string httpMethod, string responseData);
        void LoadFileResponse(string endpointUrl, IFormFile file);
    }
}
