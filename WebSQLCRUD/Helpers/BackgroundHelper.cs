
namespace WebSQLCRUD.Helpers
{
    public class BackgroundHelper : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            Console.WriteLine("Started Async Background.");
            await Task.Delay(TimeSpan.FromSeconds(10));
            int times = 1;

            DatabaseHelper dbHelper = new DatabaseHelper(); 

            while (!stoppingToken.IsCancellationRequested)
            {
                DatabaseHelper.CreateToDatabase();

                Console.WriteLine("Background Authors and Books updated! " + times);
                times++;

                await Task.Delay(TimeSpan.FromSeconds(6), stoppingToken);
            }
        }
    }
}
