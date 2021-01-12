using System;

namespace Goiar.Simple.Cqrs.sample.aspnetcore.Model
{
    public class WheatherRequest
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public string Summary { get; set; }
    }
}
