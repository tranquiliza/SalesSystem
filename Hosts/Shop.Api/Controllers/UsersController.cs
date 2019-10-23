using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;
using Tranquiliza.Shop.Core.Application;
using Tranquiliza.Shop.Core.Model;

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

        [AllowAnonymous]
        [HttpPost("Authenticate")]
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

            var roleClaims = user.Roles.Select(role => new Claim(ClaimTypes.Role, role));
            if (roleClaims != null)
                tokenDescriptor.Subject.AddClaims(roleClaims);
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var bearerToken = tokenHandler.WriteToken(token);

            return Ok(user.Map(bearerToken));
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RegisterUser([FromBody]RegisterUserModel registerUserModel)
        {
            var result = await _userService.Create(registerUserModel.Username, registerUserModel.Password).ConfigureAwait(false);
            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.User.Map());
        }

        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery]Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest("Please provide an Id");

            var user = await _userService.GetById(userId, ApplicationContext.Create(Guid.Parse(User.Identity.Name))).ConfigureAwait(false);
            return Ok(user.Map());
        }

        [HttpDelete]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete([FromQuery]Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest("Please provide an Id");

            var result = await _userService.Delete(userId, ApplicationContext.Create(Guid.Parse(User.Identity.Name))).ConfigureAwait(false);
            if (!result.Success)
                return Unauthorized(result.FailureReason);

            return Ok();
        }
    }
}
