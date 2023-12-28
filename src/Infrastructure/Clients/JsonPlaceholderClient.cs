using Domain.DTOs;
using Domain.DTOs.Requests;
using Domain.Interfaces;
using Newtonsoft.Json;
using System.Text;

namespace Infrastructure.Clients
{
    public class JsonPlaceholderClient : IJsonPlaceholderClient
    {
        private HttpClient _httpClient;

        public JsonPlaceholderClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<GetUser>> GetUsers()
        {
            var response = await _httpClient.GetAsync("https://jsonplaceholder.typicode.com/users");
            var users = await response.Content.ReadAsAsync<List<GetUser>>();

            return users;
        }

        public async Task<JsonPlaceholderResult<GetUser>> GetUserById(int id)
        {
            var response = await _httpClient.GetAsync($"https://jsonplaceholder.typicode.com/users/{id}");
            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadAsAsync<GetUser>();

                return new JsonPlaceholderResult<GetUser>
                {
                    Data = data,
                    IsSuccessful = true,
                    ErrorMessage = ""
                };
            }
            else
            {
                return new JsonPlaceholderResult<GetUser>
                {
                    IsSuccessful = false,
                    ErrorMessage = response.StatusCode.ToString()
                };
            }
        }

        public async Task<CreateUser> CreateUser (CreateUser user)
        {
            var jsonContent = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync("https://jsonplaceholder.typicode.com/users", jsonContent);
            var createdUser = await response.Content.ReadAsAsync<CreateUser>();
            
            return createdUser;
        }



    }
}
