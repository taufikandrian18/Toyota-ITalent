using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TAM.Talent.Jobs.MDM
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static async Task MainTask()
        {
            // Prevents SSL error due to self-signed certificate OR intranet connection.
            //var handler = new WebRequestHandler();
            //handler.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            //var url = "http://13.76.212.218";
            var url = "https://italent.toyota.astra.co.id";
            //var apiKey = ConfigurationManager.AppSettings["ApiKey"];

            var client = new HttpClient();
            client.DefaultRequestHeaders.ConnectionClose = true;
            //client.DefaultRequestHeaders.Add("ApiKey", apiKey);

            var request = client.GetAsync(url + "/api/v1/staging/all");
            var response = await request;
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Server Response:");
            Console.WriteLine((int)response.StatusCode);
            Console.WriteLine(response.StatusCode.ToString());
            Console.WriteLine(body);

            Thread.Sleep(10000);
        }
        static void Main(string[] args)
        {
            MainTask().GetAwaiter().GetResult();
        }
    }
}
