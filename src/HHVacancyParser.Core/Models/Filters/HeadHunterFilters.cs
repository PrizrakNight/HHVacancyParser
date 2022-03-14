using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace HHVacancyParser.Core.Models.Filters
{
    public sealed class HeadHunterFilters : IEnumerable<HeadHunterNovaFilter>
    {
        public const string Region = "area";
        public const string Neighbour = "neighbours";
        public const string Specializations = "professional_role";
        public const string Schedule = "schedule";
        public const string EmploymentType = "employment";
        public const string CompanySector = "industry";

        private readonly IReadOnlyCollection<HeadHunterNovaFilter> _filters;

        public HeadHunterFilters(IEnumerable<HeadHunterNovaFilter> filters)
        {
            _filters = new ReadOnlyCollection<HeadHunterNovaFilter>(filters.ToList());
        }

        public HeadHunterFilters()
        {
            _filters = new ReadOnlyCollection<HeadHunterNovaFilter>(new List<HeadHunterNovaFilter>());
        }

        public IEnumerable<HeadHunterNovaFilter> GetFiltersByName(string filterName)
        {
            return _filters.Where(x => x.FilterName == filterName);
        }

        public IEnumerator<HeadHunterNovaFilter> GetEnumerator()
        {
            return _filters.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
