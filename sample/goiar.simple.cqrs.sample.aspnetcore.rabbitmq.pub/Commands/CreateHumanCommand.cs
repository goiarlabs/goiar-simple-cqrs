using goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Models;
using Goiar.Simple.Cqrs.Commands;
using System;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Commands
{
    public class CreateHumanCommand : ICommand<Human>
    {
        public CreateHumanCommand() 
        {
            EntityId = Guid.NewGuid();
        }

        public CreateHumanCommand(Guid? entityId, string name)
        {
            EntityId = entityId ?? Guid.NewGuid();
            Name = name;
        }

        public Guid EntityId { get; set; }

        public string Name { get; set; }
    }
}
