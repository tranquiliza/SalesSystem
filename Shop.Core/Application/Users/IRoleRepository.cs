﻿using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IRoleRepository
    {
        Task<Role> Get(string roleName);
    }
}