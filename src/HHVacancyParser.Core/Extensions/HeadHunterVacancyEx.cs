using HHVacancyParser.Core.EqualityComparers;
using HHVacancyParser.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace HHVacancyParser.Core.Extensions
{
    public static class HeadHunterVacancyEx
    {
        public static IEnumerable<HeadHunterVacancy> UniqueVacancies(this IEnumerable<HeadHunterVacancy> vacancies)
        {
            return vacancies.Distinct(new HeadHunterVacancyEqualityComparer());
        }
    }
}
