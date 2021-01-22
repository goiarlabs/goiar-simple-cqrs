using Goiar.Simple.Cqrs.Commands;
using System;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Commands
{
    public class UpdateHumanCommand : ICommand
    {
        public UpdateHumanCommand(Guid entityId, string name)
        {
            EntityId = entityId;
            Name = name;
        }

        public Guid EntityId { get; }

        public string Name { get; }
    }
}
