using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IUserRepository
    {
        Task<User> Get(Guid id);
        Task<User> GetByEmail(string email);
        Task<IEnumerable<User>> GetAll();
        Task Save(User user);
        Task Delete(Guid id);
    }
}
