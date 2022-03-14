using HHVacancyParser.Core.Models.Responses;

namespace HHVacancyParser.Core
{
    public interface IHeadHunterParser
    {
        HeadHunterResponse ParseResponse(string htmlDocument);
    }
}
