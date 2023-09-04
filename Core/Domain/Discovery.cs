namespace Core.Domain
{
    public interface Discovery
    {
        Task Load(DiscoveryRecord record, Integration integration);
    }
}
