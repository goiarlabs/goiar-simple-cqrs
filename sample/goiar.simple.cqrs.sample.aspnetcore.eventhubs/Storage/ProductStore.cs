using System;
using System.Collections.Generic;
using System.Linq;

using goiar.simple.cqrs.sample.aspnetcore.eventhubs.Models;

namespace goiar.simple.cqrs.sample.aspnetcore.eventhubs.Storage
{
    public class ProductStore
    {
        public ProductStore()
        {
            Products = SeedsProducts();
        }

        public List<Product> Products { get; init; }
        
        public List<Product> SeedsProducts()
        {
            var names = new[]
            {
                "Laptop", "XBox", "TV 4k", "Gaming Chair", "Pixel 5", "Monitor 4k Ultrawide ", "Tenkeyless Keyboard"
            };

            var random = new Random();

            return Enumerable
                .Range(1, 5)
                .Select(index =>
                    new Product(
                        random.Next(1, 55),
                        names[random.Next(names.Length)]))
                .ToList();
        }
    }
}
