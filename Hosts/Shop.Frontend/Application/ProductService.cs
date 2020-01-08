using Shop.Frontend.Infrastructure;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Tranquiliza.Shop.Contract.Models;

namespace Shop.Frontend.Application
{
    public class ProductService : IProductService
    {
        private readonly IApiGateway _api;

        public ProductService(IApiGateway api)
        {
            _api = api;
        }

        public List<ExtendedProductModel> Products { get; private set; } = new List<ExtendedProductModel>();

        private void NotifyStateChanged() => OnChange?.Invoke();
        public event Action OnChange;

        public async Task Initialize()
        {
            var onlyActive = new QueryParam("onlyActive", "false");
            var extendedProperties = new QueryParam("extended", "true");
            Products = await _api.Get<List<ExtendedProductModel>>("Products", queryParams: new QueryParam[] { onlyActive, extendedProperties }).ConfigureAwait(false);
            NotifyStateChanged();
        }

        public async Task DeleteProduct(Guid productId)
        {
            var itemToRemove = Products.Find(x => x.Id == productId);
            if (itemToRemove != null)
            {
                await _api.Delete("Products", routeValues: new string[] { itemToRemove.Id.ToString() }).ConfigureAwait(false);
                Products.Remove(itemToRemove);
                NotifyStateChanged();
            }
        }

        public async Task EditProduct(Guid productId, EditProductModel model)
        {
            var updatedProduct = await _api.Post<ExtendedProductModel, EditProductModel>(model, "Products", routeValues: new string[] { productId.ToString() }).ConfigureAwait(false);
            if (updatedProduct != null)
            {
                var productToReplace = Products.Find(x => x.Id == productId);
                if (productToReplace != null)
                {
                    Products.Remove(productToReplace);
                    Products.Add(updatedProduct);
                }
            }

            NotifyStateChanged();
        }

        public async Task UploadImage(Guid productId, MemoryStream memoryStream, string fileName)
        {
            await _api.PostImage(memoryStream, fileName, "Products", routeValues: new string[] { productId.ToString(), "Image" }).ConfigureAwait(false);

            // Fetch product again and replace in Products?

            var extendedProperties = new QueryParam("extended", "true");
            var product = await _api.Get<ExtendedProductModel>("Products", routeValues: new string[] { productId.ToString() }, queryParams: extendedProperties).ConfigureAwait(false);
            if (product == null)
            {
                // Shit
            }

            var productToRemove = Products.Find(x => x.Id == productId);
            Products.Remove(productToRemove);
            Products.Add(product);

            NotifyStateChanged();
        }

        public async Task CreateProduct(CreateProductModel model)
        {
            var newProduct = await _api.Post<ExtendedProductModel, CreateProductModel>(model, "Products").ConfigureAwait(false);
            if (newProduct != null)
            {
                Products.Add(newProduct);
                NotifyStateChanged();
            }
        }

        public async Task MakePrimaryImage(Guid productId, string imageName)
        {
            var model = new UpdateMainImageModel { ImageName = imageName };
            var result = await _api.Post<ExtendedProductModel, UpdateMainImageModel>(model, "Products", routeValues: new string[] { productId.ToString(), "MainImage" }).ConfigureAwait(false);
            if (result != null)
            {
                var productToRemove = Products.Find(x => x.Id == productId);
                Products.Remove(productToRemove);
                Products.Add(result);

                NotifyStateChanged();
            }
        }

        public async Task DeleteImage(Guid productId, string imageName)
        {
            var model = new DeleteImageModel { ImageName = imageName };
            var result = await _api.Delete<ExtendedProductModel, DeleteImageModel>(model, "Products", routeValues: new string[] { productId.ToString(), "Image", imageName }).ConfigureAwait(false);
            if (result != null)
            {
                var productToRemove = Products.Find(x => x.Id == productId);
                Products.Remove(productToRemove);
                Products.Add(result);
                NotifyStateChanged();
            }
        }
    }
}
