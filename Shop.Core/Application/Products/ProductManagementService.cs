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
        private readonly IUserRepository _userRepository;

        public ProductManagementService(IProductRepository productRepository, IUserRepository userRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
        }

        public async Task<IResult<Product>> CreateProduct(string title, string category, int price, IApplicationContext context)
        {
            var currentUser =  await _userRepository.Get(context.UserId).ConfigureAwait(false);
            if (!currentUser.HasRole(Role.Admin))
                return Result<Product>.Failure("Insufficient permissions");

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
