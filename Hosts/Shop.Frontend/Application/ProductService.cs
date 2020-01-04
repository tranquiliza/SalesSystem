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

        public List<ProductModel> Products { get; private set; } = new List<ProductModel>();

        private void NotifyStateChanged() => OnChange?.Invoke();
        public event Action OnChange;

        public async Task Initialize()
        {
            Products = await _api.Get<List<ProductModel>>("Products").ConfigureAwait(false);
            NotifyStateChanged();
        }

        public async Task DeleteProduct(Guid productId)
        {
            var itemToRemove = Products.Find(x => x.Id == productId);
            if (itemToRemove != null)
            {
                Products.Remove(itemToRemove);
                NotifyStateChanged();
            }
        }
    }
}
