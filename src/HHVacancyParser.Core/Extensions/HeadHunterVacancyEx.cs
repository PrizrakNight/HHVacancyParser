using HHVacancyParser.Core.EqualityComparers;
using HHVacancyParser.Core.Models;
using System.Collections.Generic;
using System.Linq;

namespace HHVacancyParser.Core.Extensions
{
    public static class HeadHunterVacancyEx
    {
        public static IEnumerable<HeadHunterVacancy> ExcludeNonVacancies(this IEnumerable<HeadHunterVacancy> vacancies)
        {
            return vacancies.Where(x => x.Name != VacancyConstants.NotAVacancy);
        }

        public static IEnumerable<HeadHunterVacancy> UniqueVacancies(this IEnumerable<HeadHunterVacancy> vacancies)
        {
            return vacancies.Distinct(new HeadHunterVacancyEqualityComparer());
        }

        public static IEnumerable<HeadHunterVacancy> WithSalary(this IEnumerable<HeadHunterVacancy> vacancies)
        {
            return vacancies.Where(x => x.Salary != null);
        }

        public static IEnumerable<IGrouping<string, HeadHunterVacancy>> GroupByCurrency(this IEnumerable<HeadHunterVacancy> vacancies)
        {
            return vacancies.GroupBy(x => x.Salary == null ? string.Empty : x.Salary.Currency);
        }
    }
}
