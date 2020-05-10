using Shop.Frontend.Infrastructure;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;

namespace Shop.Frontend.Application
{
    public class UserService : IUserService
    {
        public class UserInformation
        {
            public Guid Id { get; private set; }
            public List<string> Roles { get; private set; }
            public DateTime TokenExpires { get; private set; }

            public UserInformation(Guid id, List<string> roles, DateTime tokenExpires)
            {
                Id = id;
                Roles = roles;
                TokenExpires = tokenExpires;
            }
        }

        private readonly IApplicationStateManager _applicationStateManager;
        private readonly IApiGateway _api;

        public UserInformation User { get; private set; }

        public bool IsUserLoggedIn => User != null;
        public bool IsUserAdmin => User?.Roles.Any(role => string.Equals("ADMIN", role, StringComparison.Ordinal)) == true;

        public IReadOnlyList<InquiryModel> Inquiries { get; private set; }

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
                User = await CreateUserFromJwtToken(currentToken).ConfigureAwait(false);
                NotifyStateChanged();
            }
        }

        public async Task LoadUserInquiries()
        {
            if (IsUserLoggedIn)
                Inquiries = await _api.Get<List<InquiryModel>>("users/inquiries").ConfigureAwait(false);
            else
                UnloadInquiries();
        }

        private void UnloadInquiries()
        {
            Inquiries = null;
        }

        public async Task<bool> TryLogin(AuthenticateModel model)
        {
            var response = await _api.Post<UserAuthenticatedModel, AuthenticateModel>(model, "Users", "Authenticate").ConfigureAwait(false);
            if (response == null)
                return false;

            await _applicationStateManager.SetJwtToken(response.Token).ConfigureAwait(false);
            User = await CreateUserFromJwtToken(response.Token).ConfigureAwait(false);
            await LoadUserInquiries().ConfigureAwait(false);
            NotifyStateChanged();
            return true;
        }

        public async Task<bool> TryLogout()
        {
            User = null;
            UnloadInquiries();
            await _applicationStateManager.SetJwtToken(string.Empty).ConfigureAwait(false);

            NotifyStateChanged();
            return true;
        }

        private async Task<UserInformation> CreateUserFromJwtToken(string jwtToken)
        {
            if (string.IsNullOrEmpty(jwtToken))
                return null;

            var jwt = new JwtSecurityToken(jwtToken);
            var uniqueName = jwt.Claims.FirstOrDefault(x => x.Type == "unique_name");
            if (!Guid.TryParse(uniqueName?.Value, out var userId))
                return null;

            var expires = jwt.ValidTo;
            if (expires <= DateTime.UtcNow)
            {
                await TryLogout().ConfigureAwait(false);
                UnloadInquiries();
                return null;
            }

            var roles = jwt.Claims.Where(x => x.Type == "role").Select(x => x.Value).ToList();

            return new UserInformation(userId, roles, expires);
        }

        public async Task CreateAccount(RegisterUserModel model)
        {
            await _api.Post(model, "Users").ConfigureAwait(false);
        }
    }
}
