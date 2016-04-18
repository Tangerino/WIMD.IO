using System;
using System.Collections.Generic;
using System.Text;

namespace WimdioApiProxy.v2.Rest
{
    internal class RestUriBuilder
    {
        public string BaseUrl { get; private set; }
        public string Resource { get; private set; }
        public IDictionary<string, string> QueryParameters { get; private set; }

        public RestUriBuilder(string baseUrl, string resource, IDictionary<string, string> queryParameters = null)
        {
            if (!IsValidUri(baseUrl))
                throw new ArgumentException($"{nameof(baseUrl)} must be set to a valid Url");

            if (string.IsNullOrEmpty(resource))
                throw new ArgumentException($"{nameof(resource)} method must be set");

            BaseUrl = baseUrl;
            Resource = resource;
            QueryParameters = queryParameters;
        }

        public static bool IsValidUri(string potentialUri)
        {
            Uri uri = null;
            return Uri.TryCreate(potentialUri, UriKind.Absolute, out uri);
        }

        public override string ToString()
        {
            var path = string.Concat(BaseUrl.TrimEnd('/'), "/", Resource.TrimStart('/'));

            var uri = new UriBuilder(path);

            if (QueryParameters != null && QueryParameters.Count > 0)
            {
                var queryString = new StringBuilder();

                foreach (var pair in QueryParameters)
                {
                    if (queryString.Length > 0)
                    {
                        queryString.Append("&");
                    }

                    queryString.Append(pair.Key);
                    queryString.Append("=");
                    queryString.Append(pair.Value);
                }

                uri.Query = queryString.ToString();
            }

            return uri.ToString();
        }

    }
}
