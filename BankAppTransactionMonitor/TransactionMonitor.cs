using Services;

namespace BankAppTransactionMonitor
{
    public class TransactionMonitor
    {
        private readonly ProgressTracker _progressTracker;
        private readonly ICustomerService _customerService;
        private readonly ReportWriter _reportWriter;

        public TransactionMonitor(ProgressTracker progressTracker, ICustomerService customerService, ReportWriter reportWriter)
        {
            _progressTracker = progressTracker;
            _customerService = customerService;
            _reportWriter = reportWriter;
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
                var customers = await _customerService.GetCustomersByCountryAsync(countryCode);

                foreach (var customer in customers)
                {
                    foreach (var account in customer.Dispositions.Select(d => d.Account))
                    {
                        var transactions = account.Transactions
                            .Where(t => t.TransactionId > lastCheckedId)
                            .ToList();

                        // Rule 1
                        foreach (var transaction in transactions)
                        {
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

                        // Rule 2
                        var recentTransactions = transactions
                            .Where(t => t.Date.ToDateTime(TimeOnly.MinValue) > DateTime.Now.AddHours(-72))
                            .ToList();

                        var recentTotal = recentTransactions.Sum(t => t.Amount);

                        if (recentTotal > 23000)
                        {
                            foreach (var t in recentTransactions)
                            {
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
                }

                

                _reportWriter.WriteReport(countryCode, suspiciousTransactions);

                var highestTransactionId = customers
                    .SelectMany(c => c.Dispositions.Select(d => d.Account))
                    .SelectMany(a => a.Transactions)
                    .Where(t => t.TransactionId > lastCheckedId)
                    .Max(t => (int?)t.TransactionId) ?? lastCheckedId;

                progress[countryCode] = highestTransactionId;

                _progressTracker.Save(progress);

                Console.WriteLine($"{countryCode} done.");
            }
        }
    }
}
