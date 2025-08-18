
namespace WebSQLCRUD.Helpers
{
    public class BackgroundHelper : IHostedService
    {
        private readonly ILogger<BackgroundHelper> _logger;
        public BackgroundHelper(ILogger<BackgroundHelper> logger)
        {
            _logger = logger;
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background service is starting.");
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Background service is stopping.");
        }
    }
}
