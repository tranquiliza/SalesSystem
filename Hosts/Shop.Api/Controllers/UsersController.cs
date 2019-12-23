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
    public class UsersController : BaseController
    {
        private readonly IUserService _userService;
        private readonly Core.IApplicationConfigurationProvider _configurationProvider;

        public UsersController(IUserService userService, Core.IApplicationConfigurationProvider configurationProvider)
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

            var roleClaims = user.UserRoles.Select(role => new Claim(ClaimTypes.Role, role));
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
            var result = await _userService.Create(registerUserModel.Email, registerUserModel.Password, Role.Admin).ConfigureAwait(false);
            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Map());
        }

        [HttpGet]
        public async Task<IActionResult> GetUser([FromQuery]Guid userId)
        {
            if (userId == Guid.Empty)
            {
                var allUsersResult = await _userService.GetAll(ApplicationContext).ConfigureAwait(false);
                if (!allUsersResult.Success)
                    return BadRequest(allUsersResult.FailureReason);

                return Ok(allUsersResult.Data.Select(x => x.Map()));
            }

            var result = await _userService.GetById(userId, ApplicationContext).ConfigureAwait(false);
            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok(result.Data.Map());
        }

        [HttpGet("Confirm/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail([FromRoute]Guid userId, [FromQuery]Guid emailConfirmationToken)
        {
            var result = await _userService.ConfirmEmail(userId, emailConfirmationToken).ConfigureAwait(false);
            if (!result.Success)
                return BadRequest(result.FailureReason);

            return Ok();
        }

        [HttpDelete]
        [Authorize(Roles = Role.Admin)]
        public async Task<IActionResult> Delete([FromQuery]Guid userId)
        {
            if (userId == Guid.Empty)
                return BadRequest("Please provide an Id");

            var result = await _userService.Delete(userId, ApplicationContext).ConfigureAwait(false);
            if (!result.Success)
                return Unauthorized(result.FailureReason);

            return Ok();
        }
    }
}
