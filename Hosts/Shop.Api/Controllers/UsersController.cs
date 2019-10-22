using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.Api.Controllers
{
    [ApiController]
    [Authorize]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IConfigurationProvider _configurationProvider;

        public UsersController(IUserService userService, IConfigurationProvider configurationProvider)
        {
            _userService = userService;
            _configurationProvider = configurationProvider;
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody]AuthenticateModel authenticateModel)
        {
            var user = await _userService.Authenticate(authenticateModel.Username, authenticateModel.Password).ConfigureAwait(false);
            if (user == null)
                return BadRequest(new { message = "Username or password is incorrect" });

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configurationProvider.SecurityKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id.ToString()),
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var roleClaims = user.Roles.Select(r => new Claim(ClaimTypes.Role, r.Name));
            if (roleClaims != null)
                tokenDescriptor.Subject.AddClaims(roleClaims);
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new User
            {
                Id = user.Id,
                Username = user.Username,
                Token = tokenHandler.WriteToken(token)
            });
        }
    }
}
