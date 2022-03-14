using System.Collections.Generic;

namespace HHVacancyParser.Core.Models.Responses
{
    public class HeadHunterPagesTraversalResponse
    {
        public long UnixStartSeconds { get; set; }
        public long UnixEndSeconds { get; set; }

        public IReadOnlyCollection<HeadHunterResponse> Responses { get; set; } = null!;
    }
}
