using HHVacancyParser.Core.Models;
using System.Collections.Generic;

namespace HHVacancyParser.Core.EqualityComparers
{
    public class HeadHunterVacancyEqualityComparer : IEqualityComparer<HeadHunterVacancy>
    {
        public bool Equals(HeadHunterVacancy x, HeadHunterVacancy y)
        {
            if ((x == null ^ y == null) || (x == null && y == null))
                return false;

            return x!.Equals(y!);
        }

        public int GetHashCode(HeadHunterVacancy obj)
        {
            return obj.GetHashCode();
        }
    }
}
