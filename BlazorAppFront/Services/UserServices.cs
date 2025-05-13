using System.Net.Http.Json;

namespace BlazorAppFront.Services
{
    public class UserServices
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public UserServices(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        public async Task<string> AddUser(UserAddViewModel email)
        {
            var client = _httpClientFactory.CreateClient("ApiCalls");
            var response = await client.PostAsJsonAsync("https://localhost:7044/api/User/AddUser",email);
            var result = await response.Content.ReadAsStringAsync();
            Console.WriteLine(result);
            return "ooo0";
        }

    }
}
