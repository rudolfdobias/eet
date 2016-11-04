using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Mews.Eet.Communication
{
    public class SoapHttpClient
    {
        public SoapHttpClient(string endpointUrl)
        {
            EndpointUrl = endpointUrl;
            HttpClient = new HttpClient();
        }

        private string EndpointUrl { get; }

        private HttpClient HttpClient { get; }

        public Task<string> Send(string body, string operation)
        {
            HttpClient.DefaultRequestHeaders.Remove("SOAPAction");
            HttpClient.DefaultRequestHeaders.Add("SOAPAction", operation);
            var task = HttpClient.PostAsync(
                new Uri(EndpointUrl),
                new StringContent(body, Encoding.UTF8, "application/x-www-form-urlencoded")
            );
            return AsyncHelpers.SafeContinuationAction(task, response =>
            {
                // TODO: Is there a better way how to handle a nested task?
                var readTask = response.Content.ReadAsStringAsync();
                readTask.Wait();
                return readTask.Result;
            });
        }
    }
}