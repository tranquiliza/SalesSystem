using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public interface IProductManagementService
    {
        Task<IResult<Product>> CreateProduct(string title, string category, int price);
    }
}