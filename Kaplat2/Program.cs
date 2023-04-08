using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

namespace Kaplat2
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            string getResponse = Get();
            string postResponse = Post(getResponse);
          
            Console.WriteLine(postResponse);
            Console.Read(); 
        }

        public static string Get()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8989/test_get_method?id=319041166&year=1998");
            var response = client.SendAsync(request).Result;
            
            return response.Content.ReadAsStringAsync().Result;
        }

        public static string CreateJsonObject(string i_GetResponse)
        {
            var jsonObject = new JObject { 
                { "id", 319041166 },
                { "year", 1998 },
                { "requestId",  i_GetResponse }
            };
           
            return jsonObject.ToString();
        }

    public static string Post(string i_GetResponse)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8989/test_post_method");
            var content = new StringContent(CreateJsonObject(i_GetResponse), null, "application/json");
            request.Content = content;
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }
    }
}
