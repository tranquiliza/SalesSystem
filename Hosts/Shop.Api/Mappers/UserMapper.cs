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
            return new UserModel
            {
                Email = user.Email,
                Username = user.Username,
                Id = user.Id
            };
        }

        public static UserAuthenticatedModel Map(this Core.Model.User user, string token)
        {
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
