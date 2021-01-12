using Goiar.Simple.Cqrs.Attributes;
using Goiar.Simple.Cqrs.Commands;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.Queries;
using Goiar.Simple.Cqrs.UserIdentities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Goiar.Simple.Cqrs
{
    /// <summary>
    /// Router class in charge to know where it has to send the commands
    /// </summary>
    public class Router : ICommandSender, IQueryRequester
    {
        #region Fields

        private readonly Guid _correlationId;
        private readonly IServiceProvider _serviceProvider;
        private readonly EventQueue _eventQueue;
        private readonly IUserIdentityHolder _userIdentityHolder;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="Router"/>
        /// Implements <see cref="ICommandSender"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="dbContext"></param>
        /// <param name="userIdentityHolder"></param>
        /// <param name="correlationId"></param>
        public Router(IServiceProvider serviceProvider, EventQueue eventQueue, IUserIdentityHolder userIdentityHolder, Guid? correlationId = null)
        {
            _serviceProvider = serviceProvider;
            _userIdentityHolder = userIdentityHolder;
            _correlationId = correlationId ?? Guid.NewGuid();
            _eventQueue = eventQueue;
        }

        #endregion

        #region Command implementations

        /// <inheritdoc />
        public async Task<TResponse> Send<TResponse>(ICommand<TResponse> message)
            where TResponse : class
        {
            var @event = new Event<TResponse>(
                _userIdentityHolder.UserId ?? "NoId",
                _correlationId);
            @event.SetCommand<TResponse, ICommand<TResponse>>(message);

            try
            {
                var handler = _serviceProvider.GetService(typeof(ICommandHandler<TResponse, ICommand<TResponse>>)) as ICommandHandler<TResponse, ICommand<TResponse>>;

                if (handler is null)
                {
                    throw new InvalidOperationException($"there's no command handler regitered for {message.GetType().Name}");
                }

                var res = await handler.Handle(message);
                @event.Success(res);
                return res;
            }
            catch (Exception ex)
            {
                @event.Failed(ex);
                throw;
            }
            finally
            {
                if (ShouldEnque(typeof(ICommand<TResponse>)))
                {
                    _eventQueue.Enqueue(@event);
                }
            }
        }

        /// <inheritdoc />
        public async Task Send<TCommand>(TCommand message) where TCommand : ICommand
        {
            var @event = new Event(
                _userIdentityHolder.UserId ?? "NoId",
                _correlationId);
            @event.SetCommand<WeirdVoid, ICommand<WeirdVoid>>(message);

            try
            {
                var handler = _serviceProvider.GetService(typeof(ICommandHandler<TCommand>)) as ICommandHandler<TCommand>;

                if (handler is null)
                {
                    throw new InvalidOperationException($"there's no command handler regitered for {message.GetType().Name}");
                }

                await handler.Handle(message);

                @event.Success(WeirdVoid.Value);
            }
            catch (Exception ex)
            {
                @event.Failed(ex);
                throw;
            }
            finally
            {
                if (ShouldEnque(typeof(TCommand)))
                {
                    _eventQueue.Enqueue(@event);
                }
            }
        }

        #endregion

        #region Query implementations

        /// <inheritdoc />
        public Task<TResponse> Query<TResponse, TQuery>(TQuery query) where TQuery : IQuery
        {
            var handler = _serviceProvider.GetService(typeof(IQueryHandler<TResponse, TQuery>)) as IQueryHandler<TResponse, TQuery>;

            if (handler is null)
            {
                throw new InvalidOperationException($"there's no query handler regitered for {query.GetType().Name}");
            }

            return handler.Handle(query);
        }

        #endregion

        #region Private methods

        private static bool ShouldEnque(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(NoEnqueueEventAttribute), false);

            return attributes == null || !attributes.Any();
        }

        #endregion
    }
}
