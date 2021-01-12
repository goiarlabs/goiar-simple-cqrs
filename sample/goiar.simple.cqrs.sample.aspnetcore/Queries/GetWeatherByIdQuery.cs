using Goiar.Simple.Cqrs.Queries;
using System;

namespace Goiar.Simple.Cqrs.sample.aspnetcore.Queries
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