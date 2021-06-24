using System;
using System.Linq;
using System.Threading.Tasks;

using Goiar.Simple.Cqrs.Commands;
using goiar.simple.cqrs.sample.aspnetcore.eventhubs.Storage;
using goiar.simple.cqrs.sample.aspnetcore.eventhubs.Models;

namespace goiar.simple.cqrs.sample.aspnetcore.eventhubs.Commands
{
    public class ProductCommandHandler :
        ICommandHandler<Product, CreateProductCommand>,
        ICommandHandler<DeleteProductCommand>
    {
        private readonly ProductStore _productStore;

        public ProductCommandHandler(ProductStore productStore)
        {
            _productStore = productStore;
        }

        public Task<Product> Handle(CreateProductCommand command)
        {
            var product = new Product(command.Price, command.Name);

            _productStore.Products.Add(product);

            return Task.FromResult(product);
        }

        public Task Handle(DeleteProductCommand command)
        {
            var product = _productStore.Products.FirstOrDefault(p => p.Id == command.EntityId);

            if (product is null)
            {
                throw new Exception($"Product with id {command.EntityId} was not found");
            }

            _productStore.Products.Remove(product);

            return Task.CompletedTask;
        }  
    }
}
