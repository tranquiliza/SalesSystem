using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurity _security;
        private readonly IDateTimeProvider _timeProvider;

        public UserService(IUserRepository userRepository, ISecurity security, IDateTimeProvider timeProvider)
        {
            _userRepository = userRepository;
            _security = security;
            _timeProvider = timeProvider;
        }

        public async Task<User> Authenticate(string email, string password)
        {
            if (email == null)
                return null;

            if (password == null)
                return null;

            var user = await _userRepository.GetByEmail(email).ConfigureAwait(false);
            if (user == null)
                return null;

            if (!_security.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<ICreateUserResult> Create(string email, string password, string roleName = null)
        {
            if (string.IsNullOrEmpty(email))
                return CreateUserResult.Failure("email cannot be empty.");

            if (string.IsNullOrEmpty(password))
                return CreateUserResult.Failure("Cannot create a user without a password.");

            if (await _userRepository.GetByEmail(email).ConfigureAwait(false) != null)
                return CreateUserResult.Failure("A user with that username already exists.");

            if (!_security.TryCreatePasswordHash(password, out var hash, out var salt))
                return CreateUserResult.Failure("Unable to generate password hash and salt.");

            var user = new User(email, hash, salt);
            if (!string.IsNullOrEmpty(roleName))
                user.AddRole(roleName);

            await _userRepository.Save(user).ConfigureAwait(false);

            return CreateUserResult.Succeeded(user);
        }

        public async Task<IResult> Delete(Guid id, IApplicationContext applicationContext)
        {
            var currentUser = await _userRepository.Get(applicationContext.UserId).ConfigureAwait(false);
            if (!currentUser.HasRole(Role.Admin))
                return Result.Failure("Unsufficient permissions");

            await _userRepository.Delete(id).ConfigureAwait(false);

            return Result.Succeeded;
        }

        public async Task<IEnumerable<User>> GetAll(IApplicationContext applicationContext)
        {
            var currentUser = await _userRepository.Get(applicationContext.UserId).ConfigureAwait(false);
            if (!currentUser.HasRole(Role.Admin))
                return Enumerable.Empty<User>();

            return await _userRepository.GetAll().ConfigureAwait(false);
        }

        public async Task<User> GetById(Guid id, IApplicationContext applicationContext)
        {
            var currentUser = await _userRepository.Get(applicationContext.UserId).ConfigureAwait(false);
            if (!currentUser.HasRole(Role.Admin) || currentUser.Id != id)
                return null;

            return await _userRepository.Get(id).ConfigureAwait(false);
        }

        public async Task UpdatePassword(Guid id, string password, string newPassword, IApplicationContext applicationContext)
        {
            var currentUser = await _userRepository.Get(applicationContext.UserId).ConfigureAwait(false);
            if (currentUser.Id != id)
                return;

            var user = await _userRepository.Get(id).ConfigureAwait(false);
            if (!_security.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return;

            if (_security.TryCreatePasswordHash(newPassword, out var hash, out var salt))
                user.UpdatePassword(hash, salt);

            await _userRepository.Save(user).ConfigureAwait(false);
        }

        public async Task<IResult> RestorePassword(Guid id, string newPassword, Guid resetToken)
        {
            var user = await _userRepository.Get(id).ConfigureAwait(false);
            if (!user.ResetTokenMatchesAndIsValid(resetToken, _timeProvider.UtcNow))
                return Result.Failure("Token was invalid"); // Perhabs use same pattern as create user?

            if (!_security.TryCreatePasswordHash(newPassword, out var hash, out var salt))
                return Result.Failure("Unable to generate password hash and salt.");

            user.UpdatePassword(hash, salt);
            user.InvalidateResetToken();
            await _userRepository.Save(user).ConfigureAwait(false);

            return Result.Succeeded;
        }
    }
}
