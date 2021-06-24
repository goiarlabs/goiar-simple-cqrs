using System;

using Goiar.Simple.Cqrs.Commands;

namespace goiar.simple.cqrs.sample.aspnetcore.eventhubs.Commands
{
    public class DeleteProductCommand : ICommand
    {
        public DeleteProductCommand(Guid entityId)
        {
            EntityId = entityId;
        }

        public Guid EntityId { get; }
    }
}