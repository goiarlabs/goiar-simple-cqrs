using System;

namespace goiar.simple.cqrs.sample.aspnetcore.Model
{
    public class WheatherRequest
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}
