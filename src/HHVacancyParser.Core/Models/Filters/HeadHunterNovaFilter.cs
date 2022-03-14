using System;

namespace HHVacancyParser.Core.Models.Filters
{
    public class HeadHunterNovaFilter
    {
        public string? Title { get; }

        public string FilterName { get; } = string.Empty;
        public string Value { get; } = string.Empty;

        public int NumberOfVacancies { get; }

        public HeadHunterNovaFilter(string filterName, string value)
        {
            FilterName = filterName ?? throw new ArgumentNullException(nameof(filterName));
            Value = value ?? throw new ArgumentNullException(nameof(value));
        }

        public HeadHunterNovaFilter(string? title,
            string filterName,
            string value,
            int numberOfVacancies) : this(filterName, value)
        {
            Title = title;
            NumberOfVacancies = numberOfVacancies;
        }

        public override string ToString()
        {
            return $"[{Title}|{NumberOfVacancies}] {FilterName} - {Value}";
        }
    }
}
