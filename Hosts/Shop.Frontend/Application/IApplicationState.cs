using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Frontend.Application
{
    public interface IApplicationState
    {
        Task<T> GetItem<T>(string key);
        Task SetItem<T>(string key, T value);
    }
}
