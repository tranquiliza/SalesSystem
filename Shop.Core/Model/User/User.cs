using System;
using System.Collections.Generic;
using System.Linq;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.Core.Model
{
    public class User : DomainEntityBase
    {
        private class Data
        {
            public Guid Id { get; set; }
            public string Username { get; set; }
            public string Email { get; set; }
            public byte[] PasswordHash { get; set; }
            public byte[] PasswordSalt { get; set; }
            public List<string> Roles { get; set; } = new List<string>();
            public bool EmailConfirmed { get; set; }
            public Guid EmailConfirmationToken { get; set; }
            public Guid ResetToken { get; set; }
            public DateTime ResetTokenExpiration { get; set; }
        }

        private Data UserData { get; }
        public IReadOnlyList<string> Roles => UserData.Roles.AsReadOnly();

        public Guid Id => UserData.Id;
        public byte[] PasswordHash => UserData.PasswordHash;
        public byte[] PasswordSalt => UserData.PasswordSalt;
        public string Username => UserData.Username;
        public string Email => UserData.Email;

        [Obsolete("Serialization Only", true)]
        public User() { }

        private User(Data userData)
        {
            UserData = userData;
        }

        private User(string email, byte[] passwordHash, byte[] passwordSalt)
        {
            UserData = new Data
            {
                Id = Guid.NewGuid(),
                Email = email,
                Username = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt,
                EmailConfirmationToken = Guid.NewGuid()
            };

            AddEvent(new UserCreatedEvent(UserData.EmailConfirmationToken, Email));
        }

        internal void AddRole(string role)
        {
            if (!UserData.Roles.Contains(role))
                UserData.Roles.Add(role);
        }

        internal bool HasRole(string role)
        {
            return UserData.Roles.Any(r => r == role);
        }

        internal void UpdatePassword(byte[] passwordHash, byte[] passwordSalt)
        {
            UserData.PasswordHash = passwordHash;
            UserData.PasswordSalt = passwordSalt;
        }

        internal Guid GenerateResetToken(DateTime tokenExpirationTime)
        {
            UserData.ResetToken = Guid.NewGuid();
            UserData.ResetTokenExpiration = tokenExpirationTime;

            return UserData.ResetToken;
        }

        internal bool ResetTokenMatchesAndIsValid(Guid resetToken, DateTime now) => resetToken == UserData.ResetToken && now < UserData.ResetTokenExpiration;

        internal void InvalidateResetToken()
        {
            UserData.ResetToken = Guid.Empty;
            UserData.ResetTokenExpiration = default;
        }

        internal bool ConfirmEmail(Guid confirmationToken)
        {
            if (confirmationToken != UserData.EmailConfirmationToken)
                return false;

            UserData.EmailConfirmed = true;
            AddEvent(new UserEmailConfirmedEvent(Email));
            return true;
        }

        internal static User CreateNewUser(string email, byte[] passwordHash, byte[] passwordSalt) => new User(email, passwordHash, passwordSalt);
        public static User CreateUserFromData(string userData) => new User(Serialization.Deserialize<Data>(userData));
        public string Serialize() => Serialization.Serialize(UserData);
    }
}