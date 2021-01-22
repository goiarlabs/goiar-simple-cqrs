using Goiar.Simple.Cqrs.Queries;
using System;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Queries
{
    public class GetHumanByIdQuery : IQuery
    {
        public GetHumanByIdQuery(Guid humanId)
        {
            HumanId = humanId;
        }

        public Guid HumanId { get; }
    }
}
