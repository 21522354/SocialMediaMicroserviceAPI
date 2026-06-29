using ChatService.DataLayer.DTO;
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
        public async Task<UserReadDTO> GetUserById(int id)
        {
            var response = await _httpClient.GetAsync($"{_configuration["UserServiceEndpoint"]}/{id}");
            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestException($"Failed to get user by ID: {response.StatusCode}");
            }

            var content = await response.Content.ReadAsStringAsync();
            return ReadResponseData<UserReadDTO>(content);
        }

        private static T ReadResponseData<T>(string content)
        {
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            var response = JsonSerializer.Deserialize<ServiceResponse<T>>(content, options);
            if (response == null || response.data == null)
            {
                throw new HttpRequestException("User service returned empty data.");
            }

            return response.data;
        }

        private class ServiceResponse<T>
        {
            public int result { get; set; }
            public T data { get; set; }
        }
    }
}
