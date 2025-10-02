using Library_Management_Sys.Helpers.Enums;
using Library_Management_Sys.Repositories.Interfaces;

namespace Library_Management_Sys.Services
{
    public class CheckService : BackgroundService  
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly TimeSpan Check_Duration = TimeSpan.FromDays(1);
        public CheckService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }
        // Update status of overdue transactions everyday 
        public async Task CheckOverdue_Update()
        {
            DateTime today = DateTime.UtcNow;
            using (var scope = _serviceProvider.CreateScope())
            {
                var borrowTransactionRepository = scope.ServiceProvider.GetRequiredService<IBorrowTransactionRepository>();
                var overdueTransactions = await borrowTransactionRepository.GetAllAsync(t => t.Status == BorrowTransStatus.Borrowed && t.ReturnDate < today);
                foreach (var transaction in overdueTransactions)
                {
                    transaction.Status = BorrowTransStatus.Overdue;
                    await borrowTransactionRepository.UpdateAsync(transaction);
                }
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                await CheckOverdue_Update();
                await Task.Delay(Check_Duration, stoppingToken);
            }

        }
    }
}
