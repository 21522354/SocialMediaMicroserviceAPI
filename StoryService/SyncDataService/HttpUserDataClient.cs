using StoryService.Data_Layer.DTOs;
using System.Text.Json;

namespace StoryService.SyncDataService
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

        public async Task<IEnumerable<UserReadDTO>> GetUsersFollowing(Guid userId)
        {
            var response = await _httpClient.GetAsync($"{_configuration["UserServiceEndpoint"]}/following/{userId}");
            if (response.IsSuccessStatusCode)
            {
                var content = await response.Content.ReadAsStringAsync();
                var users = JsonSerializer.Deserialize<IEnumerable<UserReadDTO>>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                return users;
            }
            else
            {
                throw new HttpRequestException($"Failed to get list user following by ID: {response.StatusCode}");
            }
        }
    }
}
