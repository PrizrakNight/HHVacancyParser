using System;

namespace HHVacancyParser.Core.Models
{
    public class HeadHunterVacancy : IEquatable<HeadHunterVacancy>
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;

        public HeadHunterCompany Company { get; set; } = null!;

        public Uri Link { get; set; } = null!;

        public DateTime? PublicationDate { get; set; }

        public string? RawPublicationDate { get; set; }
        public string? WorkingMethod { get; set; }

        public HeadHunterSalary? Salary { get; set; }

        public bool Equals(HeadHunterVacancy other)
        {
            if (other == null)
                return false;

            if (Salary == null ^ other.Salary == null)
                return false;

            var result = Name == other.Name;

            result &= Company.Name == other.Company.Name;
            result &= Location == other.Location;

            if (Salary != null)
                result &= Salary.Equals(other.Salary!);

            return result;
        }

        public override int GetHashCode()
        {
            var hash = HashCode.Combine(Name, Location, Company.Name);

            if (Salary != null)
                hash = HashCode.Combine(hash, Salary!.GetHashCode());

            return hash;
        }

        public string ToShortString()
        {
            return $"{Name} - {Company.Name}*{Location} {Salary}";
        }

        public override string ToString()
        {
            var verifiedCompany = Company.IsVerified ? "✓" : "✖";
            var publicationDate = PublicationDate == null ? RawPublicationDate : PublicationDate.Value.ToShortDateString();

            if (Company.IsOnline)
                publicationDate = "Online";

            return $"({publicationDate }) [{verifiedCompany}] {Name} {Salary?.ToString()}";
        }
    }
}
