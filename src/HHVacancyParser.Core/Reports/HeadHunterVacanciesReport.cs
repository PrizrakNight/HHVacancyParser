using System;

namespace HHVacancyParser.Core.Reports
{
    [Serializable]
    public class HeadHunterVacanciesReport
    {
        public string? SearchText { get; set; }

        public DateTime ReportGenerationDate { get; set; }

        // Vacancies
        public int NumberOfVacancies { get; set; }

        // Companies
        public int NumberOfVerifiedCompanies { get; set; }
        public int NumberOfUnverifiedCompanies { get; set; }
        public int NumberOfCompaniesOnline { get; set; }
        public int NumberOfCompaniesOffline { get; set; }

        // Salaries
        public int NumberOfSalaries { get; set; }
        public int QuantitySalaryForks { get; set; }
        public int QuantityWithoutSalary { get; set; }
        public int QuantityWithoutSalaryForks { get; set; }

        public HeadHunterAverageSalaryReport[] SalaryReports { get; set; } = null!;
    }
}
