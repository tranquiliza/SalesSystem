using System;
using System.Collections.Generic;
using System.Linq;
using Tranquiliza.Shop.Core.Application;

namespace Tranquiliza.Shop.Core.Model
{
    public class User
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

        public User(string email, byte[] passwordHash, byte[] passwordSalt)
        {
            UserData = new Data
            {
                Id = Guid.NewGuid(),
                Email = email,
                Username = email,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };
        }

        public void AddRole(string role)
        {
            if (!UserData.Roles.Contains(role))
                UserData.Roles.Add(role);
        }

        public bool HasRole(string role)
        {
            return UserData.Roles.Any(r => r == role);
        }

        public void UpdatePassword(byte[] passwordHash, byte[] passwordSalt)
        {
            UserData.PasswordHash = passwordHash;
            UserData.PasswordSalt = passwordSalt;
        }

        public Guid GenerateResetToken(DateTime tokenExpirationTime)
        {
            UserData.ResetToken = Guid.NewGuid();
            UserData.ResetTokenExpiration = tokenExpirationTime;

            return UserData.ResetToken;
        }

        public bool ResetTokenMatchesAndIsValid(Guid resetToken, DateTime now) => resetToken == UserData.ResetToken && now < UserData.ResetTokenExpiration;

        public void InvalidateResetToken()
        {
            UserData.ResetToken = Guid.Empty;
            UserData.ResetTokenExpiration = default;
        }

        public bool ConfirmEmail(Guid confirmationToken)
        {
            if (confirmationToken != UserData.EmailConfirmationToken)
                return false;

            UserData.EmailConfirmed = true;
            return true;
        }

        public static User CreateNewUser(string email, byte[] passwordHash, byte[] passwordSalt, out Guid emailConfirmationToken)
        {
            emailConfirmationToken = Guid.NewGuid();
            return new User(email, passwordHash, passwordSalt);
        }

        public static User CreateUserFromData(string userData) => new User(Serialization.Deserialize<Data>(userData));
        public string SerializeUser() => Serialization.Serialize(UserData);
    }
}