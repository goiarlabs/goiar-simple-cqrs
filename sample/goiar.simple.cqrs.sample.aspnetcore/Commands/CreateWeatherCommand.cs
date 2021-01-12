using Goiar.Simple.Cqrs.Commands;
using System;

namespace Goiar.Simple.Cqrs.sample.aspnetcore.Commands
{
    public class CreateWeatherCommand : ICommand<Weather>
    {
        public CreateWeatherCommand(DateTime date, string summary, int temperatureC)
        {
            EntityId = Guid.NewGuid();
            Date = date;
            Summary = summary;
            TemperatureC = temperatureC;
        }

        public Guid EntityId { get; }
        public DateTime Date { get; }
        public string Summary { get; }
        public int TemperatureC { get; }
    }
}
