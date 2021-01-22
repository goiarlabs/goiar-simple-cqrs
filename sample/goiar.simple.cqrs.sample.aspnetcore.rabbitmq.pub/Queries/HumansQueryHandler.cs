using goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Models;
using Goiar.Simple.Cqrs.Queries;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Queries
{
    public class HumansQueryHandler :
        IQueryHandler<List<Human>, GetAllHumansQuery>,
        IQueryHandler<Human, GetHumanByIdQuery>
    {
        private readonly HumanStore _humanStore;

        public HumansQueryHandler(HumanStore humanStore)
        {
            _humanStore = humanStore;
        }

        public Task<List<Human>> Handle(GetAllHumansQuery query) =>
            Task.FromResult(_humanStore.GetAllHumans());

        public Task<Human> Handle(GetHumanByIdQuery query) =>
            Task.FromResult(_humanStore.GetHuman(query.HumanId));
    }
}
