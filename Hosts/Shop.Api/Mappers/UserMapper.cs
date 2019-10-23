using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;

namespace Tranquiliza.Shop.Api
{
    public static class UserMapper
    {
        public static UserModel Map(this Core.Model.User user)
        {
            if (user == null)
                return null;

            return new UserModel
            {
                Email = user.Email,
                Username = user.Username,
                Id = user.Id
            };
        }

        public static UserAuthenticatedModel Map(this Core.Model.User user, string token)
        {
            if (user == null)
                return null;

            return new UserAuthenticatedModel
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email,
                Token = token
            };
        }
    }
}
