using Domain.DTOs;
using Domain.DTOs.Requests;

namespace Domain.Interfaces
{
    public interface IJsonPlaceholderClient
    {
        Task<JsonPlaceholderResult<GetUser>> GetUserById(int id);
        Task<List<GetUser>> GetUsers();
        Task<CreateUser> CreateUser(CreateUser user);
    }
}