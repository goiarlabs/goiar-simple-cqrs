using Goiar.Simple.Cqrs.Queries;
using System;

namespace goiar.simple.cqrs.sample.aspnetcore.Queries
{
    public class GetWeatherByIdQuery : IQuery
    {
        public GetWeatherByIdQuery(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; }
    }
}