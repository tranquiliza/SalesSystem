using System;
using System.Collections.Generic;
using System.Text;

namespace Tranquiliza.Shop.Core.Application
{
    public class ProductManagementService : IProductManagementService
    {
        private readonly IProductRepository productRepository;

        public ProductManagementService(IProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }
    }
}
