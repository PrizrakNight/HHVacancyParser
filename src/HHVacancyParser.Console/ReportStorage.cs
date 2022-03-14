using HHVacancyParser.Core.Reports;
using System.Text.Json;

namespace HHVacancyParser.Console
{
    public static class ReportStorage
    {
        public static string StoragePath = "Reports.json";

        private static bool _loaded;

        private static List<HeadHunterVacanciesReport> _reports = new List<HeadHunterVacanciesReport>();

        public static void SaveReport(HeadHunterVacanciesReport report)
        {
            if (!_loaded)
                LoadReports();

            _reports!.Add(report);

            SaveReportsToFile();
        }

        private static void SaveReportsToFile()
        {
            var jsonString = JsonSerializer.Serialize(_reports);

            File.WriteAllText(StoragePath, jsonString);
        }

        private static void LoadReports()
        {
            if (!File.Exists(StoragePath))
                return;

            var jsonString = File.ReadAllText(StoragePath);
            var reports = JsonSerializer.Deserialize<List<HeadHunterVacanciesReport>>(jsonString);

            _reports = reports!.ToList();
            _loaded = true;
        }
    }
}
