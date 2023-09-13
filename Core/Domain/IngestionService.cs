namespace Core.Domain
{
    public class IngestionService
    {

        private readonly TicketService _ticketService;
        private readonly Discovery _discovery;

        public IngestionService(TicketService ticketService, Discovery discovery)
        {
            _ticketService = ticketService;
            _discovery = discovery;
        }


        public async Task Ingest(Integration integration)
        {
            // Extract
            var ticket = await _ticketService.GetTicket(integration);

            // Transform
            var discoveryRecord = new DiscoveryRecord
            {
                Id = ticket?.Id,
                Name = ticket?.Name + "_Any Transformation Rule_" + integration.Name,
                Description = ticket?.Description,
            };

            //Load
            await _discovery.Load(discoveryRecord, integration);

        }
    }
}
