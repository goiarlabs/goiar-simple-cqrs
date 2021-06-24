using System;

using Goiar.Simple.Cqrs.Queries;

namespace goiar.simple.cqrs.sample.aspnetcore.eventhubs.Queries
{
    public class GetProductByIdQuery : IQuery
    {
        public GetProductByIdQuery(Guid productId)
        {
            ProductId = productId;
        }

        public Guid ProductId { get; set; }
    }
}
