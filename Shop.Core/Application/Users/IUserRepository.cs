using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IUserRepository
    {
        Task<User> GetById(Guid id);
        Task<User> GetByUsername(string username);
        Task<IEnumerable<User>> GetAll();
        Task Save(User user);
        Task Delete(Guid id);
    }
}
