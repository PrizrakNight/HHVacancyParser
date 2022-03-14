using HHVacancyParser.Core.Models.Filters;
using System;

namespace HHVacancyParser.Core.Models.Responses
{
    public class HeadHunterResponse
    {
        public static HeadHunterResponse Empty => new HeadHunterResponse();

        public bool IsEmpty { get; }

        public long UnixTimeSeconds { get; }

        public int TotalVacancies { get; }

        public HeadHunterFilters Filters { get; }

        public HeadHunterVacancy[]? Vacancies { get; }

        public HeadHunterResponse(int totalVacancies, HeadHunterFilters filters, HeadHunterVacancy[]? vacancies)
        {
            UnixTimeSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            TotalVacancies = totalVacancies;
            Vacancies = vacancies;
            Filters = filters ?? new HeadHunterFilters();
            IsEmpty = filters == null && vacancies == null && totalVacancies == 0;
        }

        private HeadHunterResponse()
        {
            Filters = new HeadHunterFilters();
            IsEmpty = true;
        }
    }
}
