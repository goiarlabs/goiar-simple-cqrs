using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

using Goiar.Simple.Cqrs.Queries;
using goiar.simple.cqrs.sample.aspnetcore.eventhubs.Storage;
using goiar.simple.cqrs.sample.aspnetcore.eventhubs.Models;

namespace goiar.simple.cqrs.sample.aspnetcore.eventhubs.Queries
{
    public class ProductQueryHandler : 
        IQueryHandler<List<Product>, GetAllProductsQuery>,
        IQueryHandler<Product, GetProductByIdQuery>
    {

        private readonly ProductStore _productStore;

        public ProductQueryHandler(ProductStore productStore)
        {
            _productStore = productStore;
        }

        public Task<List<Product>> Handle(GetAllProductsQuery query)
        {
            return Task.FromResult(_productStore.Products);
        }

        public Task<Product> Handle(GetProductByIdQuery query)
        {
            var product = _productStore.Products.FirstOrDefault(p => p.Id == query.ProductId);

            if (product is null)
            {
                throw new Exception($"Product with id {query.ProductId} was not found");
            }

            return Task.FromResult(product);
        }
    }
}
