using HHVacancyParser.Core.Models.Requests;
using HHVacancyParser.Core.Models.Responses;
using System;

namespace HHVacancyParser.Core.Web
{
    public class HeadHunterParsedPageEventArgs : EventArgs
    {
        public readonly HeadHunterPagesTraversalRequest? TraversalRequest;
        public readonly HeadHunterRequest Request;
        public readonly HeadHunterResponse Response;

        public HeadHunterParsedPageEventArgs(HeadHunterPagesTraversalRequest? traversalRequest,
            HeadHunterRequest request,
            HeadHunterResponse response)
        {
            TraversalRequest = traversalRequest;
            Request = request;
            Response = response;
        }
    }

    public delegate void HeadHunterParsedPageEventHandler(HeadHunterParsedPageEventArgs args);
}
