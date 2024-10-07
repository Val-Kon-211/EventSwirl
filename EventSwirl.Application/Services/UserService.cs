using AutoMapper;
using EventSwirl.Application.Data.DTOs;
using EventSwirl.Application.Services.Interfaces;
using EventSwirl.DataAccess.Interfaces;
using EventSwirl.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace EventSwirl.Application.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly ISecurityService _securityService;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, ISecurityService securityService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _securityService = securityService;
        }

        public async Task CreateUser(UserDTO userDTO)
        {
            var existingUser = await _unitOfWork.UserRepository
                .Query()
                .Where(u => u.Login == userDTO.Login)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

            if (existingUser != null)
                throw new Exception(
                    $"User with login <<{userDTO.Login}>> is already exist. " +
                    $"Please, choose another login."
                    );

            var user = _mapper.Map<User>(userDTO);

            var (salt, passwordHash) = await _securityService.Encoder(userDTO.Password);
            user.PasswordHash = passwordHash;
            user.PasswordSalt = salt;

            await _unitOfWork.UserRepository.Insert(user).ConfigureAwait(false);
            _unitOfWork.Save();
        }

        public async Task<UserDTO> GetUserById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id).ConfigureAwait(false);

            return user == null 
                ? throw new ArgumentNullException() 
                : _mapper.Map<UserDTO>(user);
        }

        public async Task UpdateUser(UserDTO user)
        {
            await _unitOfWork.UserRepository.Update(_mapper.Map<User>(user)).ConfigureAwait(false);
            _unitOfWork.Save();
        }

        public async Task DeleteUserById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetById(id).ConfigureAwait(false);

            if (user == null)
                throw new ArgumentNullException(nameof(user));

            await _unitOfWork.UserRepository.Delete(user).ConfigureAwait(false);
            _unitOfWork.Save();
        }

        public async Task Login(string login, string password)
        {
            var user = await _unitOfWork.UserRepository.Query()
                .Where(u => u.Login ==  login)
                .FirstOrDefaultAsync()
                .ConfigureAwait(false);

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
