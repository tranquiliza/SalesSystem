using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranquiliza.Shop.Core.Model;

namespace Tranquiliza.Shop.Core.Application
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository _productRepository;

        public ProductManagementService(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<IResult<Product>> CreateProduct(string title, string category, int price)
        {
            try
            {
                var product = Product.Create(title, category, price);
                var persisted = await _productRepository.Save(product).ConfigureAwait(false);
                if (!persisted)
                    return Result<Product>.Failure("Unable to save product");

                return Result<Product>.Succeeded(product);
            }
            catch (DomainException ex)
            {
                return Result<Product>.Failure(ex.Message);
            }
        }
    }
}
