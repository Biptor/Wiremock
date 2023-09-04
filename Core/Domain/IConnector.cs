namespace Core.Domain
{
    public interface IConnector
    {
        Task<string?> SendAsync(string param);
    }
}
