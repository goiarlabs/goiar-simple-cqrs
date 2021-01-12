using System.Collections.Generic;

namespace goiar.simple.cqrs.sample.aspnetcore.Storage
{
    public class InmemoryDb
    {
        public InmemoryDb()
        {
            Weathers = new List<Weather>();
        }

        public IList<Weather> Weathers { get; }

    }
}
