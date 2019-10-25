using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly ISecurity _security;
        private readonly IDateTimeProvider _timeProvider;
        private readonly IEventDispatcher _eventDispatcher;

        public UserService(IUserRepository userRepository, ISecurity security, IDateTimeProvider timeProvider, IEventDispatcher eventDispatcher)
        {
            _userRepository = userRepository;
            _security = security;
            _timeProvider = timeProvider;
            _eventDispatcher = eventDispatcher;
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
            if (!EmailIsValid())
                return CreateUserResult.Failure("Invalid Email");

            if (await _userRepository.GetByEmail(email).ConfigureAwait(false) != null)
                return CreateUserResult.Failure("A user with that username already exists.");

            if (!PasswordIsValid(out var failureReason))
                return CreateUserResult.Failure(failureReason);

            if (!_security.TryCreatePasswordHash(password, out var hash, out var salt))
                return CreateUserResult.Failure("Unable to generate password hash and salt.");

            var user = User.CreateNewUser(email, hash, salt);
            if (!string.IsNullOrEmpty(roleName))
                user.AddRole(roleName);

            await _userRepository.Save(user).ConfigureAwait(false);
            await _eventDispatcher.DispatchEvents(user).ConfigureAwait(false);

            return CreateUserResult.Succeeded(user);

            bool EmailIsValid()
            {
                try
                {
                    _ = new MailAddress(email);
                    return true;
                }
                catch
                {
                    return false;
                }
            }

            bool PasswordIsValid(out string failureReason)
            {
                if (string.IsNullOrEmpty(password))
                {
                    failureReason = "Password cannot be empty";
                    return false;
                }

                var hasMinimum8Chars = new Regex(".{8,}");
                if (!hasMinimum8Chars.Match(password).Success)
                {
                    failureReason = "Password must contain atleast 8 characters";
                    return false;
                }

                var hasNumber = new Regex("[0-9]+");
                if (!hasNumber.Match(password).Success)
                {
                    failureReason = "Password must contain atleast one number";
                    return false;
                }

                var hasUpperChar = new Regex("[A-Z]+");
                if (!hasUpperChar.Match(password).Success)
                {
                    failureReason = "Password must contain atleast one uppercase character";
                    return false;
                }

                var hasLowerChar = new Regex("[a-z]+");
                if (!hasLowerChar.Match(password).Success)
                {
                    failureReason = "Password must contain atleast one lowercase character";
                    return false;
                }

                failureReason = string.Empty;
                return true;
            }
        }

        public async Task<IResult> Delete(Guid id, IApplicationContext applicationContext)
        {
            var currentUser = await _userRepository.Get(applicationContext.UserId).ConfigureAwait(false);
            if (currentUser?.HasRole(Role.Admin) == false)
                return Result.Failure("Unsufficient permissions");

            await _userRepository.Delete(id).ConfigureAwait(false);

            return Result.Succeeded;
        }

        public async Task<IResult<IEnumerable<User>>> GetAll(IApplicationContext applicationContext)
        {
            var currentUser = await _userRepository.Get(applicationContext.UserId).ConfigureAwait(false);
            if (currentUser?.HasRole(Role.Admin) == false)
                return Result<IEnumerable<User>>.Failure("Unauthorized");

            var result = await _userRepository.GetAll().ConfigureAwait(false);
            if (result == null)
                return Result<IEnumerable<User>>.Failure("No users found");

            return Result<IEnumerable<User>>.Succeeded(result);
        }

        public async Task<IResult<User>> GetById(Guid id, IApplicationContext applicationContext)
        {
            var currentUser = await _userRepository.Get(applicationContext.UserId).ConfigureAwait(false);
            if (currentUser?.Id != id && currentUser?.HasRole(Role.Admin) == false)
                return Result<User>.Failure("Unauthorized");

            var result = await _userRepository.Get(id).ConfigureAwait(false);
            if (result == null)
                return Result<User>.Failure("User not found");

            return Result<User>.Succeeded(result);
        }

        public async Task UpdatePassword(Guid id, string password, string newPassword, IApplicationContext applicationContext)
        {
            var currentUser = await _userRepository.Get(applicationContext.UserId).ConfigureAwait(false);
            if (currentUser?.Id != id)
                return;

            var user = await _userRepository.Get(id).ConfigureAwait(false);
            if (!_security.VerifyPasswordHash(password, user.PasswordHash, user.PasswordSalt))
                return;

            if (_security.TryCreatePasswordHash(newPassword, out var hash, out var salt))
                user.UpdatePassword(hash, salt);

            await _userRepository.Save(user).ConfigureAwait(false);
            await _eventDispatcher.DispatchEvents(user).ConfigureAwait(false);
        }

        public async Task<IResult> RestorePassword(Guid id, string newPassword, Guid resetToken)
        {
            var user = await _userRepository.Get(id).ConfigureAwait(false);
            if (user?.ResetTokenMatchesAndIsValid(resetToken, _timeProvider.UtcNow) == false)
                return Result.Failure("Token was invalid");

            if (!_security.TryCreatePasswordHash(newPassword, out var hash, out var salt))
                return Result.Failure("Unable to generate password hash and salt.");

            user.UpdatePassword(hash, salt);
            user.InvalidateResetToken();
            await _userRepository.Save(user).ConfigureAwait(false);
            await _eventDispatcher.DispatchEvents(user).ConfigureAwait(false);

            return Result.Succeeded;
        }
    }
}
