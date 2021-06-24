using System;

namespace goiar.simple.cqrs.sample.aspnetcore.eventhubs.Models
{
    public class Product
    {
        public Product(int price, string name)
        {
            Id = Guid.NewGuid();
            CreatedOn = DateTime.UtcNow;
            Price = price;
            Name = name;
        }

        public Guid Id { get; set; }

        public DateTime CreatedOn { get; set; }

        public int Price { get; set; }

        public string Name { get; set; }
    }
}
