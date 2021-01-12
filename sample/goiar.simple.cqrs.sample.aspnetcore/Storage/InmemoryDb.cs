using System.Collections.Generic;

namespace Goiar.Simple.Cqrs.sample.aspnetcore.Storage
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
