using Core.Domain;
using Core.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Core.UseCases;

public class UploadFileConnectorUseCase : IUploadFileConnectorUseCase
{
    private readonly IConnector _connector;

    public UploadFileConnectorUseCase(IConnector connector)
    {
        _connector = connector ?? throw new ArgumentNullException(nameof(connector));
    }
    public async Task<bool> ExecuteAsync(IFormFile file, string rutaFullCompleta)
    {
        return await _connector.SendFileAsync("http://localhost:3001/uploadfile", file, rutaFullCompleta);
    }
}
