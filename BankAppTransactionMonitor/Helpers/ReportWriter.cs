using BankAppTransactionMonitor.Models;
using System.Text;

namespace BankAppTransactionMonitor.Helpers
{
    public class ReportWriter
    {
        private const string ReportFolder = "Reports";

        public void WriteReport(string countryCode, List<SuspiciousTransaction> transactions)
        {
            if (!transactions.Any())
            {
                Console.WriteLine($"No suspicious transactions found for {countryCode}.");
                Console.WriteLine("No report created.");
                return;
            }

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var reportDirPath = Path.Combine(rootPath, ReportFolder);

            Directory.CreateDirectory(reportDirPath);

            var filename = Path.Combine(reportDirPath, $"Suspicious_{countryCode}_{timestamp}.txt");

            var sb = new StringBuilder();

            sb.AppendLine($"Suspicious transactions – {countryCode} – {timestamp}");

            var grouped = transactions.GroupBy(t => t.Rule);

            foreach (var group in grouped)
            {
                sb.AppendLine("-------------------------------------------------------------------------");
                sb.AppendLine($"* {group.Key}:");
                sb.AppendLine("");

                foreach (var t in group)
                {
                    sb.AppendLine($"  CustomerId: {t.CustomerId}, AccountId: {t.AccountId}, TransactionId: {t.TransactionId}, Amount: {t.Amount}");
                }
            }

            sb.AppendLine("-------------------------------------------------------------------------");
            sb.AppendLine($"Total suspicious transactions: {transactions.Count}");
            sb.AppendLine("-------------------------------------------------------------------------");

            File.WriteAllText(filename, sb.ToString());

            Console.WriteLine($"{transactions.Count} suspicious transactions detected!");
            Console.WriteLine($"Report created. Find it at:\n{filename}");
        }
    }
}
