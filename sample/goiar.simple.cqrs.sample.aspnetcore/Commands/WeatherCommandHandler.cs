using Goiar.Simple.Cqrs.sample.aspnetcore.Storage;
using Goiar.Simple.Cqrs.Commands;
using System.Threading.Tasks;
using System.Linq;

namespace Goiar.Simple.Cqrs.sample.aspnetcore.Commands
{
    public class WeatherCommandHandler : 
        ICommandHandler<Weather, CreateWeatherCommand>,
        ICommandHandler<UpdateWeatherCommand>
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

        public Task Handle(UpdateWeatherCommand command)
        {
            var weather = _db.Weathers.FirstOrDefault(a => a.Id == command.EntityId);

            weather.Date = command.Date;
            weather.Summary = command.Summary;
            weather.TemperatureC = command.TemperatureC;

            return Task.CompletedTask;
        }
    }
}
