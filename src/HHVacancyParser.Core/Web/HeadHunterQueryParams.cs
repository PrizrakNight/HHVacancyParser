using HHVacancyParser.Core.Models.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace HHVacancyParser.Core.Web
{
    public class HeadHunterQueryParams : IEnumerable<KeyValuePair<string, string>>
    {
        public static HeadHunterQueryParams Default
        {
            get
            {
                var queryParams = new HeadHunterQueryParams
                {
                    _params = new List<KeyValuePair<string, string>>
                    {
                        new KeyValuePair<string, string>("search_field", "name"),
                        new KeyValuePair<string, string>("search_field", "company_name"),
                        new KeyValuePair<string, string>("search_field", "description"),
                        new KeyValuePair<string, string>("clusters", "true"),
                        new KeyValuePair<string, string>("ored_clusters", "true"),
                        new KeyValuePair<string, string>("enable_snippets", "true")
                    }
                };

                return queryParams;
            }
        }

        private List<KeyValuePair<string, string>> _params = new List<KeyValuePair<string, string>>();

        public void Add(string key, string value)
        {
            if (key == null)
                throw new ArgumentNullException(nameof(key));

            if (value == null)
                throw new ArgumentNullException(nameof(value));

            _params.Add(new KeyValuePair<string, string>(key, value));
        }

        public void AddFilter(HeadHunterNovaFilter filter)
        {
            if (filter == null)
                throw new ArgumentNullException(nameof(filter));

            _params.Add(new KeyValuePair<string, string>(filter.FilterName, filter.Value));
        }

        public void AddFilters(HeadHunterFilters filters)
        {
            if (filters == null)
                throw new ArgumentNullException(nameof(filters));

            _params.AddRange(ToKeyValuePairs(filters));
        }

        public bool RemoveByKey(string key)
        {
            var index = _params.FindIndex(x => x.Key == key);

            if (index == -1)
                return false;

            _params.RemoveAt(index);

            return true;
        }

        public int RemoveAllKeys(string key)
        {
            return _params.RemoveAll(x => x.Key == key);
        }

        public void Clear() => _params.Clear();

        public string ToQueryString()
        {
            if (_params.Count == 0)
                return string.Empty;

            var queryString = "?" + string.Join("&", _params.Select(x => $"{x.Key}={x.Value}"));

            return queryString;
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _params.GetEnumerator();
        }

        private static IEnumerable<KeyValuePair<string, string>> ToKeyValuePairs(IEnumerable<HeadHunterNovaFilter> filters)
        {
            return filters.Select(x => new KeyValuePair<string, string>(x.FilterName, x.Value));
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
