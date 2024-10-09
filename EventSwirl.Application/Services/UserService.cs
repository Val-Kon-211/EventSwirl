using AutoMapper;
using EwentSwirl.RabbitMQ.Commands;
using EventSwirl.Application.Data.DTOs;
using EventSwirl.Application.Services.Interfaces;
using EventSwirl.Domain.Entities;
using EwentSwirl.RabbitMQ;

namespace EventSwirl.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly ISecurityService _securityService;
        private readonly IProducer _producer;

        public UserService(IMapper mapper, ISecurityService securityService, IProducer producer)
        {
            _mapper = mapper;
            _securityService = securityService;
            _producer = producer;
        }

        public async Task CreateUser(UserDTO userDTO)
        {
            var existingUserResponse = await _producer
                .SendCommandAsync<GetUserByLoginCommand, GetUserByLoginResponse>(new GetUserByLoginCommand() { Login = userDTO.Login })
                .ConfigureAwait(false);

            var existingUser = existingUserResponse.User;

            if (existingUser != null)
                throw new Exception($"User with login <<{userDTO.Login}>> is already exist. " +
                    $"Please, choose another login.");

            var user = _mapper.Map<User>(userDTO);

            var (salt, passwordHash) = await _securityService.Encoder(userDTO.Password);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = salt;

            await _producer
                .SendCommandAsync<CreateUserCommand, CreateUserResponse>(new CreateUserCommand() { User = user })
                .ConfigureAwait(false);
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var userResponse = await _producer
                .SendCommandAsync<GetUserByIdCommand, GetUserByIdResponse>(new GetUserByIdCommand() { UserId = id })
                .ConfigureAwait(false);

            var user = userResponse.User;

            return user == null 
                ? throw new ArgumentNullException() 
                : _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateUser(UserDTO user) =>
            await _producer
                .SendCommandAsync<UpdateUserCommand, UpdateUserResponse>(new UpdateUserCommand() { User = _mapper.Map<User>(user) })
                .ConfigureAwait(false);

        public async Task DeleteUserById(int id)
        {
            var userResponse = await _producer
                .SendCommandAsync<GetUserByIdCommand, GetUserByIdResponse>(new GetUserByIdCommand() { UserId = id })
                .ConfigureAwait(false);

            var user = userResponse.User;

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await _producer
                .SendCommandAsync<DeleteUserCommand, DeleteUserResponse>(new DeleteUserCommand() { User = user })
                .ConfigureAwait(false);
        }

        public async Task Login(string login, string password)
        {
            var userResponse = await _producer
                .SendCommandAsync<GetUserByLoginCommand, GetUserByLoginResponse>(new GetUserByLoginCommand() { Login = login })
                .ConfigureAwait(false);

            var user = userResponse.User;

            if (user == null)
                throw new Exception($"User with login <<{login}>> doesn't exist.");

            if (!await VerifyPassword(user, password))
                throw new Exception("Password verification failed: wrong password");
        }

        public async Task Logout()
        {
            return;
        }

        private async Task<bool> VerifyPassword(User user, string password)
        {
            var (_, passwordHash) = await _securityService.Encoder(password, user.PasswordSalt);
            return passwordHash == user.PasswordHash;
        }
    }
}
