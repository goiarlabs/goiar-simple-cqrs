using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Commands
{
    /// <summary>
    /// Handles commands with responses
    /// </summary>
    /// <typeparam name="TResponse">The response of the handler</typeparam>
    /// <typeparam name="TCommand">The Message type that handles</typeparam>
    public interface ICommandHandler<TResponse, TCommand> where TCommand : ICommand<TResponse>
    {
        /// <summary>
        /// Handles the command <typeparamref name="TCommand"/>
        /// </summary>
        /// <param name="command">The command to be handled</param>
        /// <returns></returns>
        Task<TResponse> Handle(TCommand command);
    }

    /// <summary>
    /// Handles Commands with no response
    /// </summary>
    /// <typeparam name="TCommand"></typeparam>
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        /// <summary>
        /// Handles the command <typeparamref name="TCommand"/>
        /// </summary>
        /// <param name="command">The command to handle</param>
        /// <returns></returns>
        Task Handle(TCommand command);
    }
}
