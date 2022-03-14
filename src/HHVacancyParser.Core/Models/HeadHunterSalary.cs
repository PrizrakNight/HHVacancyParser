using HHVacancyParser.Core.Formats;
using System;

namespace HHVacancyParser.Core.Models
{
    public class HeadHunterSalary : IEquatable<HeadHunterSalary>
    {
        public string RawValue { get; set; } = string.Empty;
        public string Currency { get; set; } = string.Empty;

        /// <summary>
        /// Returns the value of the current salary.
        /// <para>If the salary has a fork, then the value "<see cref="HeadHunterSalaryFork.From"/>" will be returned.</para>
        /// <para><see cref="NotSupportedException"/>Thrown when trying to set a value when there is a salary fork.</para>
        /// </summary>
        public decimal Value
        {
            get
            {
                if (Fork != null)
                    return Fork.From;

                return _value;
            }

            set
            {
                if (Fork != null)
                    throw new NotSupportedException("You cannot set the value if there is a fork in the salary.");

                _value = value;
            }
        }

        public HeadHunterSalaryFork? Fork { get; set; }

        private decimal _value;

        public bool Equals(HeadHunterSalary other)
        {
            if (other == null)
                return false;

            if (other.Fork == null ^ Fork == null)
                return false;

            if (Fork != null && Fork.Equals(other.Fork!))
                return true;

            return _value == other._value;
        }

        public override int GetHashCode()
        {
            if (Fork != null)
                return HashCode.Combine(Currency, Fork!.GetHashCode());

            return HashCode.Combine(Currency, Value);
        }

        public override string ToString()
        {
            if (Fork != null)
                return $"{Fork} {Currency}";

            return $"{Value.ToString(HeadHunterStringFormat.Money)} {Currency}";
        }
    }
}
