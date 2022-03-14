using System;

namespace HHVacancyParser.Core.Models
{
    public class HeadHunterCompany
    {
        public string Name { get; set; } = string.Empty;

        public bool IsVerified { get; set; }
        public bool IsOnline { get; set; }

        public Uri Link { get; set; } = null!;

        public override string ToString()
        {
            return $"[{Name}]: " + Link;
        }
    }
}
