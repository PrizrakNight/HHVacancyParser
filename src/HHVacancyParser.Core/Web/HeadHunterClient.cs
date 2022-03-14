using HHVacancyParser.Core.Exceptions;
using HHVacancyParser.Core.Models.Requests;
using HHVacancyParser.Core.Models.Responses;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HHVacancyParser.Core.Web
{
    public class HeadHunterClient : IHeadHunterClient
    {
        public event HeadHunterParsedPageEventHandler? ParsedPage;

        public const int PageOffset = 1;

        private readonly IWebProxy _webProxy;
        private readonly IHeadHunterParser _headHunterParser;

        public HeadHunterClient(IHeadHunterParser headHunterParser,
            IWebProxy? webProxy)
        {
            _headHunterParser = headHunterParser;
            _webProxy = webProxy ?? WebRequest.GetSystemWebProxy();
        }

        public HeadHunterClient(IWebProxy? proxy = null) : this(new HtmlAgilityPackParser(), proxy)
        {
        }

        public async Task<HeadHunterPagesTraversalResponse> GoThroughPagesAsync(HeadHunterPagesTraversalRequest request,
            CancellationToken cancellationToken = default)
        {
            var responses = new List<HeadHunterResponse>();
            var response = new HeadHunterPagesTraversalResponse();

            response.UnixStartSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();

            foreach (var page in request.Pages)
            {
                request.Request.Page = page - PageOffset;

                var requestResponse = await SendRequestInternalAsync(request.Request, false, cancellationToken);

                ParsedPage?.Invoke(new HeadHunterParsedPageEventArgs(request, request.Request, requestResponse));

                if (requestResponse.IsEmpty == false)
                    responses.Add(requestResponse);

                await Task.Delay(request.DelayOptions.GetDelay());
            }

            response.UnixEndSeconds = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
            response.Responses = new ReadOnlyCollection<HeadHunterResponse>(responses);

            return response;
        }

        public async Task<HeadHunterResponse> SendAsync(HeadHunterRequest request,
            CancellationToken cancellationToken = default)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            if (string.IsNullOrWhiteSpace(request.SearchText))
                throw new InvalidOperationException("Search text must not be empty or null.");

            return await SendRequestInternalAsync(request, true, cancellationToken);
        }

        private async Task<HeadHunterResponse> SendRequestInternalAsync(HeadHunterRequest request,
            bool generateEvent,
            CancellationToken cancellationToken = default)
        {
            try
            {
                var url = GetUrl(request);
                var httpRequest = CreateWebRequest(url);

                var response = (HttpWebResponse)await httpRequest.GetResponseAsync();

                using (var streamReader = new StreamReader(response.GetResponseStream(), Encoding.UTF8))
                {
                    var htmlDocument = await streamReader.ReadToEndAsync();
                    var headHunterResponse = _headHunterParser.ParseResponse(htmlDocument);

                    if (generateEvent)
                        ParsedPage?.Invoke(new HeadHunterParsedPageEventArgs(null, request, headHunterResponse));

                    return headHunterResponse;
                }
            }
            catch (WebException webEx)
            {
                var response = (HttpWebResponse)webEx.Response;

                if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NotFound)
                    throw new HeadHunterException(request, response, webEx);

                return HeadHunterResponse.Empty;
            }
            catch (Exception)
            {
                throw;
            }
        }

        private HttpWebRequest CreateWebRequest(string url)
        {
            var httpRequest = WebRequest.CreateHttp(url);

            httpRequest.Proxy = _webProxy;

            httpRequest.Headers.Add("user-agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/99.0.4844.51 Safari/537.36");
            httpRequest.Headers.Add("sec-ch-ua", "\"Not A;Brand\"; v = \"99\", \"Chromium\"; v = \"99\", \"Google Chrome\"; v = \"99\"");
            //httpRequest.Headers.Add("accept-encoding", "gzip, deflate, br");

            return httpRequest;
        }

        private static string GetUrl(HeadHunterRequest request)
        {
            var queryParams = HeadHunterQueryParams.Default;

            if (request.Page > 0)
                queryParams.Add("page", request.Page.ToString());

            if (request.Filters != null)
                queryParams.AddFilters(request.Filters);

            queryParams.Add("text", WebUtility.UrlEncode(request.SearchText));

            var queryString = queryParams.ToQueryString();

            return $"{HeadHunterEndpoints.Vacancy}{queryString}";
        }
    }
}
