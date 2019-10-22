using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IUserService
    {
        Task<User> Authenticate(string username, string password);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(Guid id);
        Task<ICreateUserResult> Create(string username, string password, string role = null);
        Task UpdatePassword(Guid id, string password, string newPassword);
        Task Delete(Guid id);
        Task RestorePassword(Guid id, string newPassword, Guid resetToken);
    }
}
