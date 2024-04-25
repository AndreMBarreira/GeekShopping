using System.Net.Http.Headers;
using System.Text.Json;

namespace GeekShopping.Web.Utils
{
    public static class HttpClientExtensions
    {
        private static MediaTypeHeaderValue contentType 
            = new MediaTypeHeaderValue("application/json");

        //desserialização JSON (unmarshalling)
        public static async Task<T> ReadContentAs<T>(
            this HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode) throw new ApplicationException(
                    $"Something went wrong calling the API: " +
                    $"{response.ReasonPhrase}");
            var dataAsString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                return JsonSerializer.Deserialize<T>(dataAsString, 
                    new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        //SerializaçãoJSON (marshalling)
        public static Task<HttpResponseMessage> PostAsJson<T>(
            this HttpClient httpCLient, 
            string url,
            T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = contentType;
            return httpCLient.PostAsync(url, content);
        }

        public static Task<HttpResponseMessage> PutAsJson<T>(
            this HttpClient httpCLient,
            string url,
            T data)
        {
            var dataAsString = JsonSerializer.Serialize(data);
            var content = new StringContent(dataAsString);
            content.Headers.ContentType = contentType;
            return httpCLient.PutAsync(url, content);
        }
    }
}
