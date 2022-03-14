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

        public decimal AverageSalaryWithoutForks { get; set; }
        public decimal TotalAverageSalary { get; set; }

        public double CoefficientOfVariation { get; set; }

        public HeadHunterSalaryFork AverageSalaryFork { get; set; } = new HeadHunterSalaryFork();

        public override string ToString()
        {
            return $"({Currency} | {NumberOfSalaries}) Total: {TotalAverageSalary.ToString(HeadHunterStringFormat.Money)}," +
                $" Average: {AverageSalaryWithoutForks.ToString(HeadHunterStringFormat.Money)}" +
                $" CoefficientOfVariation: {CoefficientOfVariation.ToString(HeadHunterStringFormat.Percent)}" +
                $" [Min: {AverageSalaryFork.From.ToString(HeadHunterStringFormat.Money)}," +
                $" Max: {AverageSalaryFork.To.ToString(HeadHunterStringFormat.Money)}]";
        }
    }
}
