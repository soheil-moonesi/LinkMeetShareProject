using ShareLib;
using System.Net.Http.Json;
using System.Net.Http;
using BlazorAppFront.Model;

namespace BlazorAppFront.Services
{
    public class AuthServices
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public AuthServices(IHttpClientFactory httpClientFactory)
        {
                _httpClientFactory = httpClientFactory;
        }

        public async Task<LoginResult> loginAct(LoginDto user)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiCalls");
                var response = await client.PostAsJsonAsync("https://localhost:7044/api/Auth/login", user);

                if (response.IsSuccessStatusCode)
                {
                    var authResponse = await response.Content.ReadFromJsonAsync<AuthResponseDto>();
                    return new LoginResult
                    {
                        success = true,
                        email = user.email,
                        jwtBearer = authResponse.Token,
                        message = "Login successful"
                    };
                }
                else
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    return new LoginResult
                    {
                        success = false,
                        message = $"Login failed: {response.StatusCode} - {errorMessage}"
                    };
                }
            }
            catch (Exception ex)
            {
                return new LoginResult
                {
                    success = false,
                    message = $"Error: {ex.Message}"
                };
            }
        }


        public async Task<string> register(ApiUserDto user2)
        {
            var client = _httpClientFactory.CreateClient("ApiCalls");
            var response = await client.PostAsJsonAsync("https://localhost:7044/api/Auth/register", user2);
            return "ok";
        }

    }
}
