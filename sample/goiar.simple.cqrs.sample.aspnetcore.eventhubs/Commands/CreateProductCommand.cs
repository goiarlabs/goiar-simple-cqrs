using System;

using Goiar.Simple.Cqrs.Commands;
using goiar.simple.cqrs.sample.aspnetcore.eventhubs.Models;

namespace goiar.simple.cqrs.sample.aspnetcore.eventhubs.Commands
{
    public class CreateProductCommand : ICommand<Product>
    {
        public CreateProductCommand(int price, string name)
        {
            Price = price;
            Name = name;
        }

        public Guid EntityId { get; }

        public int Price { get; set; }

        public string Name { get; set; }
    }
}