using HHVacancyParser.Core.Extensions;
using HHVacancyParser.Core.Models;
using HHVacancyParser.Core.Models.Filters;
using HHVacancyParser.Core.Models.Responses;
using HHVacancyParser.Core.Web;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;

namespace HHVacancyParser.Core
{
    public class HtmlAgilityPackParser : IHeadHunterParser
    {
        private readonly string[] _ignoreList = new[] { "Метро", "Опыт", "Исключение", "Соседние", "Уровень", "Ключевые слова", "Постоянная работа" };

        private readonly Dictionary<string, string> _filterNames = new Dictionary<string, string>
        {
            {"Специализации", "professional_role" },
            {"Отрасль компании", "industry" },
        };

        public HeadHunterResponse ParseResponse(string htmlDocument)
        {
            var document = new HtmlDocument();

            document.LoadHtml(htmlDocument);

            var documentNode = document.DocumentNode;
            var totalVacancies = GetTotalVacancies(documentNode);
            var filters = GetFilters(documentNode);
            var vacancies = GetVacancies(documentNode);

            return new HeadHunterResponse(totalVacancies, filters, vacancies);
        }

        private int GetTotalVacancies(HtmlNode documentNode)
        {
            var element = documentNode.SelectSingleNode("//div[contains(@data-qa, 'vacancies-search-header')]");
            var totalVacancies = int.Parse(element.InnerText.Split(' ')[0].RemoveUnicodeSpaces().RemoveMnemonics());

            return totalVacancies;
        }

        private HeadHunterVacancy[]? GetVacancies(HtmlNode documentNode)
        {
            var node = documentNode.SelectSingleNode("//div[contains(@class, 'vacancy-serp-content')]");
            var vacancyElements = node.SelectNodes(".//div[@class = 'vacancy-serp-item']");

            if (vacancyElements == null)
                return null;

            var vacancies = vacancyElements.Select(x =>
            {
                var bodyElement = x.SelectSingleNode(".//div[@class = 'vacancy-serp-item-body__main-info']");

                if (bodyElement == null)
                {
                    return new HeadHunterVacancy
                    {
                        Name = VacancyConstants.NotAVacancy,
                        Description = x.InnerHtml
                    };
                }

                var vacancy = new HeadHunterVacancy();

                // Html elements of a job
                var titleElement = x.SelectSingleNode(".//div[contains(@class, 'vacancy-serp-item-body')]");
                var vacancyLinkElement = titleElement.SelectSingleNode(".//a[contains(@data-qa, 'vacancy-serp__vacancy-title')]");
                var vacancySalaryElement = titleElement.SelectSingleNode(".//span[contains(@data-qa, 'vacancy-serp__vacancy-compensation')]");
                var vacancyPubDateElement = x.SelectSingleNode(".//span[contains(@data-qa, 'vacancy-serp__vacancy-date')]");
                var vacancyWorkingMethod = x.SelectSingleNode(".//div[contains(@data-qa, 'vacancy-serp__vacancy-work-schedule')]");
                var vacancyResponsibilityElement = x.SelectSingleNode(".//div[contains(@data-qa, 'vacancy-serp__vacancy_snippet_responsibility')]");
                var vacancyRequirementElement = x.SelectSingleNode(".//div[contains(@data-qa, 'vacancy-serp__vacancy_snippet_requirement')]");
                var vacancyAddressElement = x.SelectSingleNode(".//div[contains(@data-qa, 'vacancy-serp__vacancy-address')]");
                var companyVerifiedElement = titleElement.SelectSingleNode(".//span[contains(@class, 'vacancy-serp-bage-trusted-employer')]");
                var companyOnlineElement = x.SelectSingleNode(".//div[contains(@class, 'vacancy-serp-item-activity')]");
                var companyNameElement = x.SelectSingleNode(".//a[contains(@data-qa, 'vacancy-serp__vacancy-employer')]");

                vacancy.Company = new HeadHunterCompany
                {
                    IsOnline = companyOnlineElement != null,
                    IsVerified = companyVerifiedElement != null,
                };

                if (companyNameElement == null)
                {
                    var linkElement = x.SelectSingleNode(".//div[contains(@class, 'vacancy-serp-item__meta-info-link')]/a");

                    companyNameElement = x.SelectSingleNode(".//div[contains(@class, 'vacancy-serp-item__meta-info-company')]");

                    vacancy.Company.Name = companyNameElement.InnerText;
                    vacancy.Company.Link = new Uri(HeadHunterEndpoints.Domain + linkElement.Attributes["href"].Value);
                }
                else
                {
                    vacancy.Company.Name = companyNameElement.InnerText;
                    vacancy.Company.Link = new Uri(HeadHunterEndpoints.Domain + companyNameElement.Attributes["href"].Value);
                }

                // Job data
                vacancy.Name = vacancyLinkElement.InnerText;
                vacancy.Link = new Uri(vacancyLinkElement.Attributes["href"].Value);
                vacancy.Salary = ParseSalary(vacancySalaryElement);
                vacancy.RawPublicationDate = vacancyPubDateElement?.InnerText;
                vacancy.WorkingMethod = vacancyWorkingMethod?.InnerText;
                vacancy.Description = $"{vacancyResponsibilityElement?.InnerText} {Environment.NewLine} {vacancyRequirementElement?.InnerText}";
                vacancy.Location = vacancyAddressElement.InnerText;

                if (vacancyPubDateElement != null && DateTime.TryParse(vacancyPubDateElement.InnerText, out var pubDate))
                    vacancy.PublicationDate = pubDate;

                return vacancy;
            });

            return vacancies.ToArray();
        }

        private HeadHunterSalary? ParseSalary(HtmlNode vacancySalaryElement)
        {
            if (vacancySalaryElement == null)
                return null;

            var salary = new HeadHunterSalary();
            var innerText = vacancySalaryElement.InnerText;
            var salarySegments = innerText.Split(' ');

            salary.RawValue = innerText;
            salary.Currency = salarySegments.Last().RemoveMnemonics();

            if (salarySegments.Length == 4)
            {
                var fromString = salarySegments[0].RemoveUnicodeSpaces().RemoveMnemonics();
                var toString = salarySegments[2].RemoveUnicodeSpaces().RemoveMnemonics();

                var salaryFork = new HeadHunterSalaryFork
                {
                    From = decimal.Parse(fromString),
                    To = decimal.Parse(toString)
                };

                salary.Fork = salaryFork;

                return salary;
            }

            salary.Value = decimal.Parse(salarySegments[1].RemoveUnicodeSpaces().RemoveMnemonics());

            return salary;
        }

        private HeadHunterFilters GetFilters(HtmlNode documentNode)
        {
            var novaFiltersElements = documentNode.SelectNodes("//div[contains(@class, 'novafilters-group-wrapper')]");

            var novaFilters = novaFiltersElements
                .Select(x => ParseNovaFilters(x))
                .SelectMany(x => x);

            return new HeadHunterFilters(novaFilters);
        }

        private IEnumerable<HeadHunterNovaFilter> ParseNovaFilters(HtmlNode novaFilterNode)
        {
            var filterElementTitle = novaFilterNode.SelectSingleNode("./div")?.InnerText.Trim();

            if (string.IsNullOrEmpty(filterElementTitle))
                return Array.Empty<HeadHunterNovaFilter>();

            if (_ignoreList.Any(x => x.Contains(filterElementTitle, StringComparison.InvariantCultureIgnoreCase)))
                return Array.Empty<HeadHunterNovaFilter>();

            var innerFilters = novaFilterNode.SelectNodes("./div/ul//li");

            if (innerFilters == null)
                return Array.Empty<HeadHunterNovaFilter>();

            var headHunterFilters = innerFilters.Select(x =>
            {
                var input = x.SelectSingleNode("./label/input");

                var filterName = input.Attributes["name"]?.Value;

                if (string.IsNullOrEmpty(filterName))
                    filterName = _filterNames.First(x => x.Key.Contains(filterElementTitle, StringComparison.InvariantCultureIgnoreCase)).Value;

                var filterValue = input.Attributes["value"].Value;
                var filterTitle = input.ParentNode.SelectSingleNode("./span/span").InnerText;
                var filterCounts = 0;
                var filterCountsElement = input.ParentNode.SelectSingleNode("./span/span/span");

                if (!string.IsNullOrWhiteSpace(filterCountsElement?.InnerText))
                    filterCounts = int.Parse(filterCountsElement.InnerText.RemoveUnicodeSpaces().RemoveMnemonics());

                return new HeadHunterNovaFilter(filterTitle, filterName, filterValue, filterCounts);
            });

            return headHunterFilters;
        }
    }
}
