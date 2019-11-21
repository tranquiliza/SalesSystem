using System;
using System.Collections;
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
        private readonly IImageRepository _imageRepository;

        public ProductManagementService(IProductRepository productRepository, IUserRepository userRepository, IImageRepository imageRepository)
        {
            _productRepository = productRepository;
            _userRepository = userRepository;
            _imageRepository = imageRepository;
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

        public async Task<IResult> AttachImageToProduct(Guid productId, byte[] imageData, string imageType)
        {
            var product = await _productRepository.Get(productId).ConfigureAwait(false);
            if (product == null)
                return Result.Failure("Product does not exist");

            var imageId = product.AddImage(imageType);
            await _imageRepository.Save(imageData, imageType, imageId).ConfigureAwait(false);
            await _productRepository.Save(product).ConfigureAwait(false);

            return Result.Succeeded;
        }

        public async Task<IResult<IEnumerable<string>>> GetCategories()
        {
            var categories = await _productRepository.GetCategories().ConfigureAwait(false);
            return Result<IEnumerable<string>>.Succeeded(categories);
        }
    }
}
