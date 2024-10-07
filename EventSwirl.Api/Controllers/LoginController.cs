using EventSwirl.Application.Data.DTOs;
using EventSwirl.Application.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace EventSwirl.Api.Controllers
{
    public class LoginController : Controller
    {
        private readonly IConfiguration _config;
        private readonly IUserService _userService;

        public LoginController(IConfiguration config, IUserService userService)
        {
            _config = config;
            _userService = userService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] UserLoginDTO dto)
        {
            IActionResult response;
            try
            {
                await _userService.Login(dto.Login, dto.Password).ConfigureAwait(false);

                var tokenString = GenerateJSONWebToken();
                response = Ok(new { token = tokenString });
            }
            catch
            {
                response = Unauthorized();
            }

            return response;
        }

        private string GenerateJSONWebToken()
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JwtOptions:SigningKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenExpiration = double.Parse(_config["JwtOptions:ExpirationSeconds"]);

            var token = new JwtSecurityToken(_config["JwtOptions:Issuer"],
              _config["JwtOptions:Audience"],
              null,
              expires: DateTime.Now.AddSeconds(tokenExpiration),
              signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
