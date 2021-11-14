using Microsoft.Azure.WebJobs.Host;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Web;

namespace UOP.AzureFunctions.StudentRetention.Helpers
{
    class WebAPIHelper
    {


        private TraceWriter _logger;

        public WebAPIHelper(TraceWriter logger)
        {
            _logger = logger;
        }

        public T Get<T>(string uriBaseAddress, string uriPath, Dictionary<string, string> parameters, Dictionary<string, List<string>> parametersLists, Dictionary<string, string> headers, string mediaType = "application/json")
        {
            try
            {
                using (var client = new HttpClient())
                {
                    _logger.Info($"ReadyTo: send new GET request to api, uriBaseAddress = '{ uriBaseAddress }', uriPath = '{ uriPath }'");

                    var uriBuilder = new UriBuilder(uriBaseAddress);
                    uriBuilder.Path = uriPath;

                    client.BaseAddress = new Uri(uriBaseAddress);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(mediaType));

                    // setting headers 
                    if (headers != null && headers.Count() > 0)
                    {
                        foreach (var item in headers)
                            client.DefaultRequestHeaders.Add(item.Key, item.Value);
                    }

                    // the list of query parameters list that will be appended to the URI
                    var queryParams = new List<string>();

                    // adding query parameters to the list
                    if (parameters != null && parameters.Count() > 0)
                    {
                        foreach (var item in parameters)
                            queryParams.Add($"{ item.Key }={ HttpUtility.UrlEncode(item.Value) }");
                    }

                    // adding query parameter lists to the list
                    if (parametersLists != null && parametersLists.Count() > 0)
                    {
                        foreach (var item in parametersLists)
                            foreach (var subItem in item.Value)
                                queryParams.Add($"{ item.Key }={ HttpUtility.UrlEncode(subItem) }");
                    }

                    // appending the list of query parameters to the URI
                    if (queryParams.Count > 0)
                        uriBuilder.Query = String.Join("&", queryParams);

                    // sending the request
                    var response = client.GetAsync(uriBuilder.Uri).Result;

                    // check results if success
                    if (response.IsSuccessStatusCode)
                    {
                        _logger.Info($"Api response with IsSuccess_Code = 'TRUE': Getting response from API and ready to extract content data from response, url = { uriBuilder.Uri }");

                        var jsonResult = response.Content.ReadAsStringAsync().Result;

                        var resultData = JsonConvert.DeserializeObject<T>(jsonResult);

                        if (resultData != null)
                        {
                            _logger.Info($"Success: Extract content data from api json response, url = { uriBuilder.Uri }");
                            return resultData;
                        }

                        else
                            throw new Exception($"Failed: Extract content data from api json response, Content data = 'NULL', url = { uriBuilder.Uri }");
                    }

                    else
                    {
                        var responseBody = response.Content.ReadAsStringAsync().Result;

                        throw new Exception($"api response with IsSuccess_Code = 'FALSE', Reason-Phrase = '{ response.ReasonPhrase }', Status-Code = '{ response.StatusCode.ToString() }'," +
                                            $" url = { uriBuilder.Uri } , response body = {responseBody}");
                    }


                }
            }
            catch (Exception ex)
            {
               // log.Error($"Error while calling GET request to API, uriBaseAddress = '{ uriBaseAddress }', uriPath = '{ uriPath }', Details = { ex.ToString() }");

                throw new Exception($"Error while calling GET request to API, uriBaseAddress = '{ uriBaseAddress }', uriPath = '{ uriPath }', Details = { ex.ToString() }");
            }
        }

        public Dictionary<string, List<string>> GetQueryParameters(IEnumerable<KeyValuePair<string, string>> keyValuePairs)
        {
            var parameters = new Dictionary<string, List<string>>();

            if (keyValuePairs != null && keyValuePairs.Count() > 0)
            {
                foreach (var p in keyValuePairs)
                {
                    List<string> values = new List<string>();
                    if (!parameters.ContainsKey(p.Key))
                    {
                        if (p.Value != string.Empty)
                            values.Add(p.Value);
                        parameters.Add(p.Key, values);
                    }
                    else
                    {
                        if (p.Value != string.Empty)
                            parameters[p.Key].Add(p.Value);
                    }
                }
            }

            return parameters;
        }


    }
}
