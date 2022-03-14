using HHVacancyParser.Core.Models.Filters;

namespace HHVacancyParser.Core.Models.Requests
{
    public class HeadHunterRequest
    {
        public string SearchText { get; set; } = string.Empty;

        public int Page { get; set; }

        public HeadHunterFilters? Filters { get; set; }
    }
}
