using Domain.DTOs.Requests;
using Domain.Exceptions;
using Domain.Interfaces;

namespace Application.Services
{
    public class UserService
    {
        private readonly IJsonPlaceholderClient _client;

        public UserService(IJsonPlaceholderClient client)
        {
            _client = client;
        }

        public async Task<List<GetUser>> GetUsers()
        {
            
            return await _client.GetUsers();
        }

        public async Task<GetUser> GetUserById(int id)
        {
            var user = await _client.GetUserById(id);
            if (!user.IsSuccessful)
            {
                throw new UserNotFoundException();
            }    
         
            return user.Data;
        }

        public async Task<CreateUser> CreateUser (CreateUser user)
        {
            CreateUser createdUser = await _client.CreateUser(user);
            return createdUser; 
        }
    }
}
