using System.Text.Json;

namespace BankAppTransactionMonitor
{
    using System.Text.Json;

    public class ProgressTracker
    {
        private const string ProgressFolder = "Data";
        private const string ProgressFileName = "progress.json";
        private readonly string ProgressFilePath;

        public ProgressTracker()
        {
            ProgressFilePath = Path.Combine(Directory.GetCurrentDirectory(), ProgressFolder, ProgressFileName);
        }

        public Dictionary<string, int> Load()
        {
            if (!File.Exists(ProgressFilePath))
                throw new FileNotFoundException("Progress file not found. It should have been created in InitIfMissing().");

            var json = File.ReadAllText(ProgressFilePath);
            return JsonSerializer.Deserialize<Dictionary<string, int>>(json)
                   ?? new Dictionary<string, int>();
        }

        public void Save(Dictionary<string, int> progress)
        {
            Directory.CreateDirectory(Path.Combine(Directory.GetCurrentDirectory(), ProgressFolder));

            var json = JsonSerializer.Serialize(progress, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ProgressFilePath, json);

            Console.WriteLine("Saving progress file...");
        }

        public void InitIfMissing(IEnumerable<string> countryCodes, int startingTransactionId)
        {
            var progressPath = Path.Combine(Directory.GetCurrentDirectory(), "Data", "progress.json");

            if (File.Exists(progressPath))
                return;

            Console.WriteLine("No progress file found – initializing from baseline...");

            var initialProgress = countryCodes.ToDictionary(code => code, code => startingTransactionId);

            Save(initialProgress);
        }

    }

}
