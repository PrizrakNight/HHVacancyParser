using HHVacancyParser.Core.Extensions;
using HHVacancyParser.Core.Maths;
using HHVacancyParser.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HHVacancyParser.Core.Reports
{
    public static class HeadHunterVacancyEx
    {
        public static HeadHunterVacanciesReport CreateVacanciesReport(this IEnumerable<HeadHunterVacancy> vacancies)
        {
            if (vacancies == null)
                throw new ArgumentNullException(nameof(vacancies));

            var report = new HeadHunterVacanciesReport
            {
                ReportGenerationDate = DateTime.Now,

                NumberOfVacancies = vacancies.Count(),

                NumberOfCompaniesOnline = vacancies.Count(x => x.Company.IsOnline),
                NumberOfCompaniesOffline = vacancies.Count(x => x.Company.IsOnline == false),
                NumberOfVerifiedCompanies = vacancies.Count(x => x.Company.IsVerified),
                NumberOfUnverifiedCompanies = vacancies.Count(x => x.Company.IsVerified == false),

                NumberOfSalaries = vacancies.Count(x => x.Salary != null),
                QuantitySalaryForks = vacancies.Count(x => x.Salary != null && x.Salary.Fork != null),
                QuantityWithoutSalary = vacancies.Count(x => x.Salary == null),
                QuantityWithoutSalaryForks = vacancies.Count(x => x.Salary == null || x.Salary.Fork == null),

                SalaryReports = vacancies.CreateSalaryReports()
            };

            return report;
        }

        public static HeadHunterAverageSalaryReport[] CreateSalaryReports(this IEnumerable<HeadHunterVacancy> vacancies)
        {
            if (vacancies == null)
                throw new ArgumentNullException(nameof(vacancies));

            vacancies = vacancies.WithSalary();

            if (vacancies.Any() == false)
                return Array.Empty<HeadHunterAverageSalaryReport>();

            var currencyGroups = vacancies.GroupByCurrency();

            var reports = currencyGroups.Select(x =>
            {
                var numberOfSalaries = x.Count();

                var report = new HeadHunterAverageSalaryReport
                {
                    Currency = x.Key,
                    NumberOfSalaries = numberOfSalaries,
                    Salaries = x.Select(x => x.Salary!).ToArray(),
                    CoefficientOfVariation = x.Select(x => x.Salary!.Value).GetCoefficientOfVariation()
                };

                // Median Calculation
                var sortedSalaries = x.Select(x => x.Salary!.Value).OrderBy(x => x);
                var midSequence = numberOfSalaries / 2;

                if (numberOfSalaries % 2 == 0)
                {
                    var median = (sortedSalaries.ElementAt(midSequence - 1) + sortedSalaries.ElementAt(midSequence)) / 2;

                    report.Median = median;
                }
                else report.Median = sortedSalaries.ElementAt(midSequence);

                // Average fork calculation
                var forks = x.Where(x => x.Salary!.Fork != null).Select(x => x.Salary!.Fork);

                if (forks.Any())
                {
                    var forkAverageMin = forks.Average(x => x!.From);
                    var forkAverageMax = forks.Average(x => x!.To);

                    report.AverageSalaryFork = new HeadHunterSalaryFork
                    {
                        From = forkAverageMin,
                        To = forkAverageMax
                    };
                }

                // Calculation of the average minimum edge of forks and without forks
                report.TotalAverage = x.Average(x => x.Salary!.Value);

                // Calculation of the average without a fork
                var withoutForks = x.Where(x => x.Salary!.Fork == null);

                if (withoutForks.Any())
                    report.AverageWithoutForks = withoutForks.Average(x => x.Salary!.Value);

                return report;
            });

            return reports.ToArray();
        }
    }
}
