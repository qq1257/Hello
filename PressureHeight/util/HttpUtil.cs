using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Networking.Connectivity;
using Windows.Web.Http;

namespace PressureHeight.util
{
    public class HttpUtil
    {
        public async Task<string> get(Uri requestUri)
        {
            if (NetworkInformation.GetInternetConnectionProfile() == null)
                throw new Exception("No network");
            //Create an HTTP client object
            HttpClient httpClient = new HttpClient();
            //Add a user-agent header to the GET request. 
            var headers = httpClient.DefaultRequestHeaders;
            //The safe way to add a header value is to use the TryParseAdd method and verify the return value is true,
            //especially if the header value is coming from user input.
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }
            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }
            //Uri requestUri = new Uri("http://api.map.baidu.com/geocoder/v2/?ak=LUmQKZ3QBGRy8qG6H7Yg5d8G5PaN46Oe&output=json&pois=0&coordtype=wgs84ll&location=" + latitude + "," + longitude);

            //Send the GET request asynchronously and retrieve the response as a string.
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";
            try
            {
                //Send the GET request
                httpResponse = await httpClient.GetAsync(requestUri);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
            }
            return httpResponseBody;
        }

        public async Task<string> post(Uri requestUri, IEnumerable<KeyValuePair<string, string>> contentK=null)
        {
            if (NetworkInformation.GetInternetConnectionProfile() == null)
                throw new Exception("No network");
            HttpClient httpClient = new HttpClient();
            var headers = httpClient.DefaultRequestHeaders;
            string header = "ie";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }
            header = "Mozilla/5.0 (compatible; MSIE 10.0; Windows NT 6.2; WOW64; Trident/6.0)";
            if (!headers.UserAgent.TryParseAdd(header))
            {
                throw new Exception("Invalid header value: " + header);
            }
            //Uri requestUri = new Uri("http://10.11.112.71:44000/plugins/interfaceSet");
            HttpResponseMessage httpResponse = new HttpResponseMessage();
            string httpResponseBody = "";
            try
            {
                //IEnumerable<KeyValuePair<string, string>> contentK = new[] { new KeyValuePair<string, string>("type", "cluster"), new KeyValuePair<string, string>("11", "111"), new KeyValuePair<string, string>("11", "111") };
                IHttpContent content = new HttpFormUrlEncodedContent(contentK);
                httpResponse = await httpClient.PostAsync(requestUri, content);
                httpResponse.EnsureSuccessStatusCode();
                httpResponseBody = await httpResponse.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error: " + ex.HResult.ToString("X") + " Message: " + ex.Message);
            }
            return httpResponseBody;
        }
    }
}
