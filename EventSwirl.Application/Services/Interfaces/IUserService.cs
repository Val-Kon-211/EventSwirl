using EventSwirl.Application.Data.DTOs;

namespace EventSwirl.Application.Services.Interfaces
{
    public interface IUserService
    {
        public Task CreateUser(UserDTO user);

        public Task<UserDTO> GetUserById(int id);

        public Task UpdateUser(UserDTO user);

        public Task DeleteUserById(int id);

        public Task Login(string login, string password);
    }
}
