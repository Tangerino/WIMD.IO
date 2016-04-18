using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using WimdioApiProxy.v2.DataTransferObjects;
using WimdioApiProxy.v2.Helpers;

namespace WimdioApiProxy.v2.Rest
{
    internal class BaseClient
    {
        protected readonly ILog log = LogManager.GetLogger(typeof(BaseClient));

        private int? RequestTimeoutInMilliseconds { get; set; }

        protected string BaseUrl { get; set; }
        protected IDictionary<HttpRequestHeader, string> Headers { get; set; }
        protected IDictionary<string, string> CustomHeaders { get; set; }
        protected string ContentType { get; set; }
        protected ISerializer Serializer { get; set; }
        protected IRequestDataWriter RequestWriter { get; set; }
        protected IResponseDataReader ResponseReader { get; set; }

        public BaseClient(string baseUrl, ISerializer dataSerializer, IRequestDataWriter requestDataWriter, IResponseDataReader responseDataReader, int? requestTimeoutInSeconds = null)
        {
            if (!RestUriBuilder.IsValidUri(baseUrl))
                throw new ArgumentException($"{nameof(baseUrl)} must be set to a valid Url");

            if (dataSerializer == null)
                throw new ArgumentException($"{nameof(dataSerializer)} can not be Null");

            if (requestDataWriter == null)
                throw new ArgumentException($"{nameof(requestDataWriter)} can not be Null");

            if (responseDataReader == null)
                throw new ArgumentException($"{nameof(responseDataReader)} can not be Null");

            BaseUrl = baseUrl;
            Serializer = dataSerializer;
            RequestWriter = requestDataWriter;
            ResponseReader = responseDataReader;

            Headers = new Dictionary<HttpRequestHeader, string>();
            CustomHeaders = new Dictionary<string, string>();

            if (requestTimeoutInSeconds.HasValue)
                RequestTimeoutInMilliseconds = (int)Math.Truncate(TimeSpan.FromSeconds(requestTimeoutInSeconds.Value).TotalMilliseconds);
        }

        public async Task<T> Get<T>(string method, IDictionary<string, string> queryStringParameters = null) where T : class
        {
            return await ExecuteCall<T>(method, HttpVerbs.Get, queryStringParameters);
        }

        public async Task<T> Post<T>(string method, object parameter, IDictionary<string, string> queryStringParameters = null) where T : class
        {
            return await ExecuteCall<T>(method, HttpVerbs.Post, parameter, queryStringParameters);
        }

        public async Task<T> Put<T>(string method, object parameter, IDictionary<string, string> queryStringParameters = null) where T : class
        {
            return await ExecuteCall<T>(method, HttpVerbs.Put, parameter, queryStringParameters);
        }

        public async Task<T> Delete<T>(string method, IDictionary<string, string> queryStringParameters = null) where T : class
        {
            return await ExecuteCall<T>(method, HttpVerbs.Delete, queryStringParameters);
        }

        public async Task<T> ExecuteCall<T>(string method, HttpVerbs verb, object bodyParameter = null, IDictionary<string, string> queryStringParameters = null) where T : class
        {
            if (string.IsNullOrEmpty(method))
                throw new ArgumentException($"{nameof(method)} must be set");

            T result = null;

            var uri = new RestUriBuilder(BaseUrl, method, queryStringParameters);
            var request = GetRequest(uri.ToString(), verb);

            string serializedObject = null;
            if (bodyParameter != null)
            {
                serializedObject = Serializer.Serialize(bodyParameter);

                await RequestWriter.WriteAsync(request, serializedObject);
            }

            string responseString = null;
            try
            {
                var response = (HttpWebResponse)await request.GetResponseAsync();

                using (response)
                {
                    responseString = await ResponseReader.ReadAsync(response);

                    if (!string.IsNullOrWhiteSpace(responseString))
                        result = Serializer.Deserialize<T>(responseString);
                }
            }
            catch (WebException we)
            {
                request?.Abort();

                var errorResponse = ((HttpWebResponse)we.Response);

                using (errorResponse)
                {
                    responseString = await ResponseReader.ReadAsync(errorResponse);

                    if (!string.IsNullOrWhiteSpace(responseString))
                    {
                        var errorResult = errorResponse.ContentType.EndsWith("json")
                                        ? Serializer.Deserialize<ErrorResponse>(responseString)
                                        : null;

                        log.Error($"RESTful '{method}' {verb.ToString().ToUpper()} failed with code: '{(int)errorResponse.StatusCode}', message: '{errorResult?.Error ?? errorResult?.Status ?? "NULL"}'", we);
                        throw;
                    }
                }
            }
            catch(Exception ex)
            {
                request?.Abort();

                log.Error(ex.Message, ex);
                throw;
            }

            return result;
        }

        protected virtual HttpWebRequest GetRequest(string uri, HttpVerbs verb)
        {
            HttpWebRequest request = null;
            try
            {
                request = (HttpWebRequest)WebRequest.Create(uri);

                if (RequestTimeoutInMilliseconds.HasValue)
                    request.Timeout = RequestTimeoutInMilliseconds.Value;

                request.Headers.Clear();
                Headers?.ToList().ForEach(h => request.Headers.Add(h.Key, h.Value));
                CustomHeaders?.ToList().ForEach(h => request.Headers.Add(h.Key, h.Value));

                request.Referer = BaseUrl;
                request.Method = verb.ToString().ToUpper();

                request.ContentType = ContentType;
                request.Accept = ContentType;

                return request;
            }
            catch (Exception ex)
            {
                request?.Abort();

                log.Error(ex.Message, ex);
                throw;
            }
        }
    }
}
