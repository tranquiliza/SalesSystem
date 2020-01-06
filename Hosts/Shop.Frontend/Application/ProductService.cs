using Shop.Frontend.Infrastructure;
using System;
using System.Collections.Generic;
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

        public async Task CreateProduct(CreateProductModel model)
        {
            var newProduct = await _api.Post<ExtendedProductModel, CreateProductModel>(model, "Products").ConfigureAwait(false);
            if (newProduct != null)
            {
                Products.Add(newProduct);
                NotifyStateChanged();
            }
        }
    }
}
