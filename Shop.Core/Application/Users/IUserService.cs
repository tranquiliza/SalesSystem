using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IUserService
    {
        Task<User> Authenticate(string email, string password);
        Task<IResult<IEnumerable<User>>> GetAll(IApplicationContext applicationContext);
        Task<IResult<User>> GetById(Guid id, IApplicationContext applicationContext);
        Task<ICreateUserResult> Create(string email, string password, string role = null);
        Task UpdatePassword(Guid id, string password, string newPassword, IApplicationContext applicationContext);
        Task<IResult> Delete(Guid id, IApplicationContext applicationContext);
        Task<IResult> RestorePassword(Guid id, string newPassword, Guid resetToken);
    }
}
