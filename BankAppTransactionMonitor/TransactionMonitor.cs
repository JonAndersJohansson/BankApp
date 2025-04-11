using Services;

namespace BankAppTransactionMonitor
{
    public class TransactionMonitor
    {
        private readonly ProgressTracker _progressTracker;
        private readonly ICustomerService _customerService;
        private readonly ReportWriter _reportWriter;
        private readonly ITransactionService _transactionService;

        public TransactionMonitor(ProgressTracker progressTracker, ICustomerService customerService, ReportWriter reportWriter, ITransactionService transactionService)
        {
            _progressTracker = progressTracker;
            _customerService = customerService;
            _reportWriter = reportWriter;
            _transactionService = transactionService;
        }
        public async Task StartAsync()
        {
            Console.WriteLine("Start scanning...");

            var countries = await _customerService.GetAllCountryCodesAsync();

            Console.WriteLine("Loading progress file...");
            _progressTracker.InitIfMissing(countries, 1056321); // Create and skip
            var progress = _progressTracker.Load(); // Load progress

            foreach (var countryCode in countries)
            {
                Console.WriteLine($"Scanning country: {countryCode}");

                var suspiciousTransactions = new List<SuspiciousTransaction>();
                var lastCheckedId = progress.ContainsKey(countryCode) ? progress[countryCode] : 0;

                // NY: Hämtar bara transaktioner för det landets konton som är nya
                var transactions = await _transactionService.GetRecentTransactionsByCountryAsync(countryCode, lastCheckedId);

                foreach (var transaction in transactions)
                {
                    var account = transaction.AccountNavigation;
                    var customer = account?.Dispositions?.FirstOrDefault()?.Customer;

                    if (account == null || customer == null)
                        continue;

                    // Rule 1
                    if (transaction.Amount > 15000)
                    {
                        suspiciousTransactions.Add(new SuspiciousTransaction
                        {
                            Rule = "Single transaction > 15000!",
                            CustomerId = customer.CustomerId,
                            AccountId = account.AccountId,
                            TransactionId = transaction.TransactionId,
                            Amount = transaction.Amount,
                            CountryCode = countryCode
                        });
                    }
                }

                // Rule 2: Grupp per AccountId och kolla totalsumma senaste 72 timmarna
                var grouped = transactions
                    .Where(t => t.Date.ToDateTime(TimeOnly.MinValue) > DateTime.Now.AddHours(-72))
                    .GroupBy(t => t.AccountId);

                foreach (var group in grouped)
                {
                    var recentTotal = group.Sum(t => t.Amount);
                    if (recentTotal > 23000)
                    {
                        foreach (var t in group)
                        {
                            var account = t.AccountNavigation;
                            var customer = account?.Dispositions?.FirstOrDefault()?.Customer;

                            if (account == null || customer == null)
                                continue;

                            suspiciousTransactions.Add(new SuspiciousTransaction
                            {
                                Rule = "72h-sum > 23000!",
                                CustomerId = customer.CustomerId,
                                AccountId = account.AccountId,
                                TransactionId = t.TransactionId,
                                Amount = t.Amount,
                                CountryCode = countryCode
                            });
                        }
                    }
                }

                _reportWriter.WriteReport(countryCode, suspiciousTransactions);

                var highestTransactionId = transactions
                    .Where(t => t.TransactionId > lastCheckedId)
                    .Max(t => (int?)t.TransactionId) ?? lastCheckedId;

                //if (highestTransactionId == lastCheckedId)
                //{
                //    Console.WriteLine($"No new transactions found for {countryCode}. Skipping update.");
                //    continue;
                //}

                progress[countryCode] = highestTransactionId;
                _progressTracker.Save(progress);

                Console.WriteLine($"{countryCode} done.");
            }

            Console.WriteLine("Scanning done!");
        }
    }
}
