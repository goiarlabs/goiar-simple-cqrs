using Goiar.Simple.Cqrs.sample.aspnetcore.Storage;
using Goiar.Simple.Cqrs.Commands;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.sample.aspnetcore.Commands
{
    public class WeatherCommandHandler : ICommandHandler<Weather, CreateWeatherCommand>
    {
        private readonly InmemoryDb _db;

        public WeatherCommandHandler(InmemoryDb db)
        {
            _db = db;
        }

        public Task<Weather> Handle(CreateWeatherCommand command)
        {
            var weather = new Weather(command.EntityId, command.Date, command.TemperatureC, command.Summary);

            _db.Weathers.Add(weather);

            return Task.FromResult(weather);
        }
    }
}
