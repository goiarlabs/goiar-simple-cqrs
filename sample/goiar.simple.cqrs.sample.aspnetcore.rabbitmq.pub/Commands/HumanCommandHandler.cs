using goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Models;
using Goiar.Simple.Cqrs.Commands;
using System.Threading.Tasks;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Commands
{
    public class HumanCommandHandler :
        ICommandHandler<Human, CreateHumanCommand>,
        ICommandHandler<UpdateHumanCommand>,
        ICommandHandler<DeleteHumanCommand>
    {
        private readonly HumanStore _humanStore;

        public HumanCommandHandler(HumanStore humanStore)
        {
            _humanStore = humanStore;
        }

        public Task<Human> Handle(CreateHumanCommand command)
        {
            var human = new Human(command.EntityId, command.Name);
            _humanStore.AddHuman(human);

            return Task.FromResult(human);
        }

        public Task Handle(UpdateHumanCommand command)
        {
            var human = new Human(command.EntityId, command.Name);
            _humanStore.UpdateHuman(human);

            return Task.CompletedTask;
        }

        public Task Handle(DeleteHumanCommand command)
        {
            _humanStore.RemoveHuman(command.EntityId);
            return Task.CompletedTask;
        }
    }
}
