using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;

namespace Shop.Frontend.Application
{
    public class UserService : IUserService
    {
        private class UserInformation
        {
            public Guid Id { get; set; }
            public List<string> Roles { get; set; }

            public UserInformation(Guid id, List<string> roles)
            {
                Id = id;
                Roles = roles;
            }
        }

        private readonly IApplicationStateManager _applicationStateManager;
        private readonly IApiGateway _api;

        private UserInformation User { get; set; }

        public bool IsUserLoggedIn => User != null;
        public bool IsUserAdmin => User?.Roles.Any(role => string.Equals("ADMIN", role, StringComparison.Ordinal)) == true;

        private void NotifyStateChanged() => OnChange?.Invoke();
        public event Action OnChange;

        public UserService(IApplicationStateManager applicationStateManager, IApiGateway api)
        {
            _applicationStateManager = applicationStateManager;
            _api = api;
        }

        public async Task Initialize()
        {
            var currentToken = await _applicationStateManager.GetJwtToken().ConfigureAwait(false);
            if (!string.IsNullOrEmpty(currentToken))
            {
                User = CreateUserFromJwtToken(currentToken);
                NotifyStateChanged();
            }
        }

        public async Task<bool> TryLogin(AuthenticateModel model)
        {
            var response = await _api.Post<UserAuthenticatedModel, AuthenticateModel>(model, "Users", "Authenticate").ConfigureAwait(false);
            if (response == null)
                return false;

            await _applicationStateManager.SetJwtToken(response.Token).ConfigureAwait(false);
            User = CreateUserFromJwtToken(response.Token);

            NotifyStateChanged();
            return true;
        }

        public async Task<bool> TryLogout()
        {
            User = null;
            await _applicationStateManager.SetJwtToken(string.Empty).ConfigureAwait(false);

            NotifyStateChanged();
            return true;
        }

        private UserInformation CreateUserFromJwtToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
                return null;

            var jwt = new JwtSecurityToken(jwtToken);
            var uniqueName = jwt.Claims.FirstOrDefault(x => x.Type == "unique_name");
            if (!Guid.TryParse(uniqueName?.Value, out var userId))
                return null;

            var roles = jwt.Claims.Where(x => x.Type == "role").Select(x => x.Value).ToList();

            return new UserInformation(userId, roles);
        }
    }
}
