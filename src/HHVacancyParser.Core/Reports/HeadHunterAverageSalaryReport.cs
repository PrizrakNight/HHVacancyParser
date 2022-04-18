using HHVacancyParser.Core.Formats;
using HHVacancyParser.Core.Models;
using System;

namespace HHVacancyParser.Core.Reports
{
    [Serializable]
    public class HeadHunterAverageSalaryReport
    {
        public string Currency { get; set; } = string.Empty;

        public int NumberOfSalaries { get; set; }

        public HeadHunterSalary[] Salaries { get; set; } = null!;

        public decimal AverageWithoutForks { get; set; }
        public decimal TotalAverage { get; set; }
        public decimal Median { get; set; }

        public double CoefficientOfVariation { get; set; }

        public HeadHunterSalaryFork AverageSalaryFork { get; set; } = new HeadHunterSalaryFork();

        public override string ToString()
        {
            return $"({Currency} | {NumberOfSalaries}) Total: {TotalAverage.ToString(HeadHunterStringFormat.Money)}," +
                $" Average: {AverageWithoutForks.ToString(HeadHunterStringFormat.Money)}" +
                $" CoefficientOfVariation: {CoefficientOfVariation.ToString(HeadHunterStringFormat.Percent)}" +
                $" [Min: {AverageSalaryFork.From.ToString(HeadHunterStringFormat.Money)}," +
                $" Max: {AverageSalaryFork.To.ToString(HeadHunterStringFormat.Money)}]";
        }
    }
}
