using HHVacancyParser.Core.Formats;
using System;

namespace HHVacancyParser.Core.Models
{
    public class HeadHunterSalaryFork : IEquatable<HeadHunterSalaryFork>
    {
        public decimal From { get; set; }
        public decimal To { get; set; }

        /// <summary>
        /// Compares salary with the current salary fork.
        /// </summary>
        /// <returns>
        /// -1 if the expected salary is less than the fork.
        /// <para>0 if the expected salary is included in the fork</para>
        /// <para>1 if the expected salary is more than the fork</para>
        /// </returns>
        public int CompareSalary(decimal salary)
        {
            if (salary < From)
                return -1;

            if (salary > To)
                return 1;

            return 0;
        }

        public bool Equals(HeadHunterSalaryFork other)
        {
            if (other == null)
                return false;

            return From == other.From && To == other.To;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(From, To);
        }

        public override string ToString()
        {
            return $"{From.ToString(HeadHunterStringFormat.Money)} - {To.ToString(HeadHunterStringFormat.Money)}";
        }
    }
}
