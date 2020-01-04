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
        private readonly IApplicationStateManager _applicationStateManager;
        private readonly IApiGateway _api;
        public bool IsUserLoggedIn { get; private set; }

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
                IsUserLoggedIn = true;
                NotifyStateChanged();
            }
        }

        public async Task<bool> TryLogin(AuthenticateModel model)
        {
            var response = await _api.Post<UserAuthenticatedModel, AuthenticateModel>(model, "Users", "Authenticate").ConfigureAwait(false);
            if (response == null)
                return false;

            await _applicationStateManager.SetJwtToken(response.Token).ConfigureAwait(false);
            IsUserLoggedIn = true;

            NotifyStateChanged();
            return true;
        }

        public async Task<bool> TryLogout()
        {
            IsUserLoggedIn = false;

            return true;
        }

        public async Task<Guid> GetCurrentUserId()
        {
            var currentToken = await _applicationStateManager.GetJwtToken().ConfigureAwait(false);
            if (string.IsNullOrEmpty(currentToken))
                return default;

            var jwt = new JwtSecurityToken(currentToken);
            var uniqueName = jwt.Claims.FirstOrDefault(x => x.Type == "unique_name");
            if (Guid.TryParse(uniqueName?.Value, out var result))
                return result;

            return default;
        }

        public async Task<List<string>> GetCurrentUserRoles()
        {
            var currentToken = await _applicationStateManager.GetJwtToken().ConfigureAwait(false);
            if (string.IsNullOrEmpty(currentToken))
                return default;

            var jwt = new JwtSecurityToken(currentToken);
            return jwt.Claims.Where(x => x.Type == "role").Select(x => x.Value).ToList();
        }
    }
}
