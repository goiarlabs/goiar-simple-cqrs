using Goiar.Simple.Cqrs.Commands;
using System;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Commands
{
    public class DeleteHumanCommand : ICommand
    {
        public DeleteHumanCommand(Guid entityId)
        {
            EntityId = entityId;
        }

        public Guid EntityId { get; }
    }
}
