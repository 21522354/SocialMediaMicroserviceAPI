using ChatService.DataLayer.DTO;
using System.Net.Http;
using System.Text.Json;

namespace ChatService.SyncDataService
{
    public class HttpUserDataClient : IUserDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public HttpUserDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }
        public async Task<UserReadDTO> GetUserById(Guid id)
        {
            var response = await _httpClient.GetAsync($"{_configuration["UserServiceEndpoint"]}/{id}");

            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var user = JsonSerializer.Deserialize<UserReadDTO>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return user;
            }
            else
            {
                throw new HttpRequestException($"Failed to get user by ID: {response.StatusCode}");
            }
        }
    }
}
