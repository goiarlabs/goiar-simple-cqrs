using System;

namespace Goiar.Simple.Cqrs.Commands
{
    /// <summary>
    /// Marker interface to represent a command with a void response
    /// </summary>
    public interface ICommand : ICommand<WeirdVoid> { }

    /// <summary>
    /// Marker interface to represent a command with an actual response
    /// </summary>
    public interface ICommand<out TResponse>
    {
        /// <summary>
        /// Identifier of the entity that needs to be processed on this command
        /// </summary>
        Guid EntityId { get; }
    }
}
