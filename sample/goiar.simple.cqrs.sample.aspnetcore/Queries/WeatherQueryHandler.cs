using Goiar.Simple.Cqrs.sample.aspnetcore.Storage;
using Goiar.Simple.Cqrs.Queries;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.sample.aspnetcore.Queries
{
    public class WeatherQueryHandler :
        IQueryHandler<IList<Weather>, GetAllWeatherQuery>,
        IQueryHandler<Weather, GetWeatherByIdQuery>
    {
        private readonly InmemoryDb _db;

        public WeatherQueryHandler(InmemoryDb db)
        {
            _db = db;
        }

        public Task<IList<Weather>> Handle(GetAllWeatherQuery query) =>
            Task.FromResult(_db.Weathers);

        public Task<Weather> Handle(GetWeatherByIdQuery query) =>
            Task.FromResult(_db.Weathers.FirstOrDefault(a => a.Id == query.Id));
    }
}
