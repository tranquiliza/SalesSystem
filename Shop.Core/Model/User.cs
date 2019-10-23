using System;
using System.Collections.Generic;
using System.Linq;

namespace Tranquiliza.Shop.Core.Model
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public string Email { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public IReadOnlyList<string> Roles => _roles.AsReadOnly();
        private readonly List<string> _roles = new List<string>();
        public bool EmailConfirmed { get; private set; }
        private Guid EmailConfirmationToken { get; set; }

        private Guid ResetToken { get; set; }
        private DateTime ResetTokenExpiration { get; set; }

        [Obsolete("Serialization Only", true)]
        public User() { }

        public User(string email, byte[] passwordHash, byte[] passwordSalt)
        {
            Id = Guid.NewGuid();
            Email = email;
            Username = email;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public void AddRole(string role)
        {
            if (!_roles.Contains(role))
                _roles.Add(role);
        }

        public bool HasRole(string role)
        {
            return _roles.Any(r => r == role);
        }

        public void UpdatePassword(byte[] passwordHash, byte[] passwordSalt)
        {
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public Guid GenerateResetToken(DateTime tokenExpirationTime)
        {
            ResetToken = Guid.NewGuid();
            ResetTokenExpiration = tokenExpirationTime;

            return ResetToken;
        }

        public bool ResetTokenMatchesAndIsValid(Guid resetToken, DateTime now)
            => resetToken == ResetToken && now < ResetTokenExpiration;

        public void InvalidateResetToken()
        {
            ResetToken = Guid.Empty;
            ResetTokenExpiration = default;
        }

        public bool ConfirmEmail(Guid confirmationToken)
        {
            if (confirmationToken != EmailConfirmationToken)
                return false;

            EmailConfirmed = true;
            return true;
        }

        public static User CreateNewUser(string email, byte[] passwordHash, byte[] passwordSalt, out Guid emailConfirmationToken)
        {
            emailConfirmationToken = Guid.NewGuid();
            return new User(email, passwordHash, passwordSalt);
        }
    }
}