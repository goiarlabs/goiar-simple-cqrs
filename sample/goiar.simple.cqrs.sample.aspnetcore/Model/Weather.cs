using System;

namespace Goiar.Simple.Cqrs.sample.aspnetcore
{
    public class Weather
    {
        public Weather(Guid id, DateTime date, int temperatureC, string summary)
        {
            Id = id;
            Date = date;
            TemperatureC = temperatureC;
            Summary = summary;
        }

        public Guid Id { get; }

        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}
