using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace SCAPI.WebFront
{
    public class WebApiHelper
    {
        public static async Task<string> GetData(string RequestUrl)
        {
            string responseData = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7047");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                // GET Method
                HttpResponseMessage response = await client.GetAsync(RequestUrl);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return responseData;
        }
        public static async Task<string> PostData<T>(string RequestUrl, T postData)
        {
            string responseData = string.Empty;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri("https://localhost:7047");
                client.DefaultRequestHeaders.Accept.Clear();
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                // Convert the post data to JSON
                var jsonContent = new StringContent(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");

                // POST Method
                HttpResponseMessage response = await client.PostAsync(RequestUrl, jsonContent);
                if (response.IsSuccessStatusCode)
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
                else
                {
                    responseData = await response.Content.ReadAsStringAsync();
                }
            }
            return responseData.Trim('"'); // Remove surrounding quotes from the response string;
        }
    }
}
