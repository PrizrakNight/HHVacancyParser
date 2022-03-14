using HHVacancyParser.Core.Formats;
using HHVacancyParser.Core.Reports;

namespace HHVacancyParser.Console.Extensions
{
    public static class HeadHunterVacanciesReportEx
    {
        public static void PrintReport(this HeadHunterVacanciesReport report)
        {
            System.Console.WriteLine();
            System.Console.WriteLine("Отчет от: " + report.ReportGenerationDate.ToString("dd.MM.yyyy"));
            System.Console.WriteLine("Проанализировано вакансий: " + report.NumberOfVacancies);
            System.Console.WriteLine("Проанализировано зарплат: " + report.NumberOfSalaries);
            System.Console.WriteLine("Кол-во вакансий без указанных зарплат: "+ report.QuantityWithoutSalary);
            System.Console.WriteLine("Валюты: " + string.Join(", ", report.SalaryReports.Select(x => x.Currency)));

            System.Console.WriteLine();
            PrintSalaryReports(report.SalaryReports);
        }

        private static void PrintSalaryReports(IEnumerable<HeadHunterAverageSalaryReport> reports)
        {
            System.Console.WriteLine("Средние зарплаты в валютах: ");
            System.Console.WriteLine();

            foreach (var report in reports)
            {
                System.Console.Write($"[Кол-во зарплат: {report.NumberOfSalaries}] ");
                System.Console.Write($"Общая средняя: {report.TotalAverageSalary.ToString(HeadHunterStringFormat.Money)} {report.Currency} ");
                System.Console.Write($"Коэффициент вариации: {report.CoefficientOfVariation.ToString(HeadHunterStringFormat.Percent)} ");
                System.Console.Write($"Среняя вилка:" +
                    $" от {report.AverageSalaryFork.From.ToString(HeadHunterStringFormat.Money)} {report.Currency}" +
                    $" до {report.AverageSalaryFork.To.ToString(HeadHunterStringFormat.Money)} {report.Currency}");

                System.Console.WriteLine();
            }
        }
    }
}
