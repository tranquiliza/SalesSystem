using System;
using System.Collections.Generic;
using System.Linq;

namespace Tranquiliza.Shop.Core.Model
{
    public class User
    {
        public Guid Id { get; private set; }
        public string Username { get; private set; }
        public byte[] PasswordHash { get; private set; }
        public byte[] PasswordSalt { get; private set; }
        public IReadOnlyList<Role> Roles => _roles.AsReadOnly();
        private readonly List<Role> _roles = new List<Role>();

        private Guid ResetToken { get; set; }
        private DateTime ResetTokenExpiration { get; set; }

        [Obsolete("Serialization Only", true)]
        public User() { }

        public User(string username, byte[] passwordHash, byte[] passwordSalt)
        {
            Id = Guid.NewGuid();
            Username = username;
            PasswordHash = passwordHash;
            PasswordSalt = passwordSalt;
        }

        public void AddRole(Role role)
        {
            if (!_roles.Contains(role))
                _roles.Add(role);
        }

        public bool HasRole(string role)
        {
            return _roles.Any(r => r.Name == role);
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
    }
}