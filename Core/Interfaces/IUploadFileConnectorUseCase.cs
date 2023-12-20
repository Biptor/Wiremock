using Microsoft.AspNetCore.Http;

namespace Core.Interfaces;

public interface IUploadFileConnectorUseCase
{
    Task<bool> ExecuteAsync(IFormFile file, string rutaFullCompleta);
}
