using EventSwirl.Application.Data.DTOs;
using EventSwirl.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EventSwirl.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        [Route("create")]
        public async Task CreateUser([FromBody] UserDTO user)
        {
            await _userService.CreateUser(user).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("edit")]
        public async Task EditUser([FromBody] UserDTO user)
        {
            await _userService.UpdateUser(user).ConfigureAwait(false);
        }

        [HttpGet]
        [Route("get/{id}")]
        public async Task<UserDTO> GetUserById(int id)
        {
            return await _userService.GetUserById(id).ConfigureAwait(false);
        }

        [HttpPost]
        [Route("delete/{id}")]
        public async Task Delete(int id)
        {
            await _userService.DeleteUserById(id).ConfigureAwait(false);
        }
    }
}
