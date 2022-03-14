using HHVacancyParser.Core.Models.Requests;
using HHVacancyParser.Core.Models.Responses;
using System.Threading;
using System.Threading.Tasks;

namespace HHVacancyParser.Core.Web
{
    public interface IHeadHunterClient
    {
        event HeadHunterParsedPageEventHandler? ParsedPage;

        Task<HeadHunterResponse> SendAsync(HeadHunterRequest request,
            CancellationToken cancellationToken = default);

        Task<HeadHunterPagesTraversalResponse> GoThroughPagesAsync(HeadHunterPagesTraversalRequest request,
            CancellationToken cancellationToken = default);
    }
}
