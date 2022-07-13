using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace TAM.Talent.Jobs.CalculateTrainings
{
    class Program
    {
        static HttpClient client = new HttpClient();

        static async Task MainTask()
        {
            // Prevents SSL error due to self-signed certificate OR intranet connection.
            //var handler = new WebRequestHandler();
            //handler.ServerCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;

            //B-Sys Server
            //var url = "http://94.237.76.37:889";
            //Local
            //var url = "http://localhost:45810";
            //var apiKey = ConfigurationManager.AppSettings["ApiKey"];
            //prod
            //var url = "http://13.76.212.218";
            var url = "https://italent.toyota.astra.co.id";

            var client = new HttpClient();
            client.DefaultRequestHeaders.ConnectionClose = true;
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("tissuememberopt!ionassessmentcou!ncilwinespeedstai!rcase");
            //client.DefaultRequestHeaders.Add("ApiKey", apiKey);

            var rangeDate = 3;

            //var request = client.PostAsync(url + "/api/v1/userside-taskanswer/calculate-bulk/" + rangeDate, null);
            var request = client.PostAsync(url + "/api/v1/userside-taskanswer/calculate-bulk-final-score" , null);

            var response = await request;
            var body = await response.Content.ReadAsStringAsync();

            Console.WriteLine("Server Response Certificate:");
            Console.WriteLine((int)response.StatusCode);
            Console.WriteLine(response.StatusCode.ToString());
            Console.WriteLine(body);

            // request = client.PostAsync(url + "/api/v1/userside-taskanswer/calculate-bulk-final-score-hierarki/" + rangeDate, null);

            // response = await request;
            // body = await response.Content.ReadAsStringAsync();
            // Console.WriteLine("Server Response Certificate Hierarki:");
            // Console.WriteLine((int)response.StatusCode);
            // Console.WriteLine(response.StatusCode.ToString());
            // Console.WriteLine(body);

            Thread.Sleep(10000);
        }
        static void Main(string[] args)
        {
            MainTask().GetAwaiter().GetResult();
        }
    }
}
