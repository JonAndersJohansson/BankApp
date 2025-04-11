using System.Text;

namespace BankAppTransactionMonitor
{
    public class ReportWriter
    {
        private const string ReportFolder = "Reports";

        public void WriteReport(string countryCode, List<SuspiciousTransaction> transactions)
        {
            if (!transactions.Any())
                return;

            var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
            var reportDirPath = Path.Combine(Directory.GetCurrentDirectory(), ReportFolder);

            Directory.CreateDirectory(reportDirPath);

            var filename = Path.Combine(reportDirPath, $"Suspicious_{countryCode}_{timestamp}.txt");

            var sb = new StringBuilder();

            sb.AppendLine($"Suspicious transactions – {countryCode} – {timestamp}");
            sb.AppendLine("==========================================");

            var grouped = transactions.GroupBy(t => t.Rule);

            foreach (var group in grouped)
            {
                sb.AppendLine();
                sb.AppendLine($"~ {group.Key} ~");

                foreach (var t in group)
                {
                    sb.AppendLine($"CustomerId: {t.CustomerId}, AccountId: {t.AccountId}, TransactionId: {t.TransactionId}, Amount: {t.Amount}");
                }
            }

            sb.AppendLine("------------------------------------------");
            sb.AppendLine($"Total suspicious transactions: {transactions.Count}");
            sb.AppendLine("------------------------------------------");

            File.WriteAllText(filename, sb.ToString());

            Console.WriteLine($"Report created: {filename}");
        }
    }
}
