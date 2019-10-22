using System;
using System.Collections.Generic;
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
        private readonly IRoleRepository _roleRepository;

        public UserService(IUserRepository userRepository, ISecurity security, IDateTimeProvider timeProvider, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _security = security;
            _timeProvider = timeProvider;
            _roleRepository = roleRepository;
        }

        public async Task<User> Authenticate(string username, string password)
        {
            if (username == null)
                return null;

            if (password == null)
                return null;

            var user = await _userRepository.GetByUsername(username).ConfigureAwait(false);
            if (user == null)
                return null;

            if (!_security.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return null;

            return user;
        }

        public async Task<ICreateUserResult> Create(string username, string password, string roleName = null)
        {
            if (string.IsNullOrEmpty(username))
                return CreateUserResult.Failure("Username cannot be empty.");

            if (string.IsNullOrEmpty(password))
                return CreateUserResult.Failure("Cannot create a user without a password.");

            if (await _userRepository.GetByUsername(username).ConfigureAwait(false) != null)
                return CreateUserResult.Failure("A user with that username already exists.");

            if (!_security.TryCreatePasswordHash(password, out var hash, out var salt))
                return CreateUserResult.Failure("Unable to generate password hash and salt.");

            var user = new User(username, hash, salt);

            if (roleName != null)
            {
                var role = await _roleRepository.Get(roleName).ConfigureAwait(false);
                if (role != null)
                    user.AddRole(role);
            }

            await _userRepository.Save(user).ConfigureAwait(false);

            return CreateUserResult.Succeeded(user);
        }

        public async Task Delete(Guid id)
        {
            await _userRepository.Delete(id).ConfigureAwait(false);
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            // Check if allowed 

            return await _userRepository.GetAll().ConfigureAwait(false);
        }

        public async Task<User> GetById(Guid id)
        {
            // Check if allowed

            return await _userRepository.GetById(id).ConfigureAwait(false);
        }

        public async Task UpdatePassword(Guid id, string password, string newPassword)
        {
            // Check if allowed
            var user = await _userRepository.GetById(id).ConfigureAwait(false);
            if (!_security.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return;

            if (_security.TryCreatePasswordHash(newPassword, out var hash, out var salt))
                user.UpdatePassword(hash, salt);

            await _userRepository.Save(user).ConfigureAwait(false);
        }

        public async Task RestorePassword(Guid id, string newPassword, Guid resetToken)
        {
            var user = await _userRepository.GetById(id).ConfigureAwait(false);
            if (!user.ResetTokenMatchesAndIsValid(resetToken, _timeProvider.UtcNow))
                return; // Perhabs use same pattern as create user?

            if (!_security.TryCreatePasswordHash(newPassword, out var hash, out var salt))
                return;

            user.UpdatePassword(hash, salt);
            user.InvalidateResetToken();
            await _userRepository.Save(user).ConfigureAwait(false);
        }
    }
}
