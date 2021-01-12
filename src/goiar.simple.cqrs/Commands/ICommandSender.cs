using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs.Commands
{
    /// <summary>
    /// Sender class used on controllers
    /// </summary>
    public interface ICommandSender
    {
        /// <summary>
        /// Sends a command and returns a response
        /// </summary>
        /// <typeparam name="TResponse">A class to be returned</typeparam>
        /// <typeparam name="TCommand">The command sended</typeparam>
        /// <param name="message">Message to send</param>
        /// <returns>the response needed</returns>
        Task<TResponse> Send<TResponse>(ICommand<TResponse> message)
            where TResponse : class;

        /// <summary>
        /// Sends a command to his handler
        /// </summary>
        /// <typeparam name="TCommand">The command to send</typeparam>
        /// <param name="message">The instance of the command</param>
        /// <returns>Task</returns>
        Task Send<TCommand>(TCommand message)
            where TCommand : ICommand;
    }
}
