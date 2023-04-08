using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text.Json;
using static System.Net.WebRequestMethods;

namespace Kaplat2
{
    internal class Program
    {
        const int k_ID = 319041166;
        const int k_YEAR = 1998;

        public static void Main(string[] args)
        {
            string getResponse = Get();
            string postResponse = Post(getResponse);
            string putResponse = Put(postResponse);
            Delete(putResponse);
        }

        public static string Get()
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Get, "http://localhost:8989/test_get_method?id=319041166&year=1998");
            var response = client.SendAsync(request).Result;

            return response.Content.ReadAsStringAsync().Result;
        }

        public static string CreatePostJsonObject(string i_GetResponse)
        {
            var jsonObject = new JObject {
                { "id", k_ID },
                { "year", k_YEAR },
                { "requestId",  i_GetResponse }
            };

            return jsonObject.ToString();
        }

        public static string CreatePutJsonObject()
        {
            int id = (k_ID - 294234) % 34;
            int year = (k_YEAR + 94) % 13;

            var jsonObject = new JObject {
                { "id", id },
                { "year", year },
            };

            return jsonObject.ToString();
        }

        public static string Post(string i_GetResponse)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Post, "http://localhost:8989/test_post_method");
            var content = new StringContent(CreatePostJsonObject(i_GetResponse), null, "application/json");
            request.Content = content;
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();

            return response.Content.ReadAsStringAsync().Result;
        }

        public static string MakePutUrl(string i_PostResponse)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(i_PostResponse);
            JsonElement root = jsonDocument.RootElement;
            string message = root.GetProperty("message").GetString();

            return "http://localhost:8989/test_put_method?id=" + message;
        }

        public static string MakeDeleteUrl(string i_PutResponse)
        {
            JsonDocument jsonDocument = JsonDocument.Parse(i_PutResponse);
            JsonElement root = jsonDocument.RootElement;
            string message = root.GetProperty("message").GetString();

            return "http://localhost:8989/test_delete_method?id=" + message;
        }

        public static string Put(string i_PostResponse)
        {
            var client = new HttpClient();
            string url = MakePutUrl(i_PostResponse);
            var request = new HttpRequestMessage(HttpMethod.Put, url);
            var content = new StringContent(CreatePutJsonObject(), null, "application/json");
            request.Content = content;
            var response = client.SendAsync(request).Result;
            response.EnsureSuccessStatusCode();
            
            return response.Content.ReadAsStringAsync().Result;
        }

        public static void Delete(string i_PutResponse)
        {
            var client = new HttpClient();
            var request = new HttpRequestMessage(HttpMethod.Delete, MakeDeleteUrl(i_PutResponse));
            var response = client.SendAsync(request).Result;
        }
    }
}
