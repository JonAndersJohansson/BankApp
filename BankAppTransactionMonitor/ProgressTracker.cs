using System.Text.Json;

namespace BankAppTransactionMonitor
{
    public class ProgressTracker
    {
        private const string ProgressFolder = "Data";
        private const string ProgressFileName = "progress.json";
        private readonly string ProgressFilePath;

        public ProgressTracker()
        {
            var rootPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", ".."));
            var dataDirPath = Path.Combine(rootPath, ProgressFolder);

            Directory.CreateDirectory(dataDirPath);

            ProgressFilePath = Path.Combine(dataDirPath, ProgressFileName);
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
            var json = JsonSerializer.Serialize(progress, new JsonSerializerOptions { WriteIndented = true });
            File.WriteAllText(ProgressFilePath, json);
            Console.WriteLine("Saving progress file...");
        }

        public void InitIfMissing(IEnumerable<string> countryCodes, int startingTransactionId)
        {
            if (File.Exists(ProgressFilePath))
                return;

            Console.WriteLine("No progress file found – initializing from baseline...");

            var initialProgress = countryCodes.ToDictionary(code => code, code => startingTransactionId);
            Save(initialProgress);
        }
    }
}
