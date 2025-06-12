using System.Net.Http.Json;
using ShareLib;

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
            getlinks();
            return "ooo0";
        }

        public async Task<List<MeetingLink2>> getlinks()
        {
            var client = _httpClientFactory.CreateClient("ApiCalls");
            var response = await client.GetAsync("https://localhost:7044/api/Meeting");
            var result = await response.Content.ReadFromJsonAsync<List<MeetingLink2>>();
            foreach (var x in result)
            {
            Console.WriteLine(x.Link);
            }

            return  result;
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

        // ... existing code ...

        public async Task<string> GetUserByemail(string email)
        {
            try
            {
                var client = _httpClientFactory.CreateClient("ApiCalls");
                // Create a simple object to send the email
                var emailRequest = new { email = email };
                var response = await client.PostAsJsonAsync("https://localhost:7044/api/User/getE", emailRequest);

                if (response.IsSuccessStatusCode)
                {
                    return await response.Content.ReadAsStringAsync();
                }
                return null;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting user: {ex.Message}");
                return null;
            }
        }

    }
}
