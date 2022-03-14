using HHVacancyParser.Core.Models.Requests;
using System;
using System.Net;

namespace HHVacancyParser.Core.Exceptions
{
    public class HeadHunterException : Exception
    {
        public HeadHunterRequest Request { get; }
        public HttpWebResponse? Response { get; }

        public HeadHunterException(HeadHunterRequest request, HttpWebResponse? response, Exception exception)
            : base("An error occurred while requesting the HeadHunter site.", exception)
        {
            Request = request;
            Response = response;
        }
    }
}
