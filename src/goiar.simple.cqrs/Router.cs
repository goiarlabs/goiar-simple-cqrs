﻿using Goiar.Simple.Cqrs.Attributes;
using Goiar.Simple.Cqrs.Commands;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.Queries;
using Goiar.Simple.Cqrs.UserIdentities;
using Microsoft.Extensions.DependencyInjection;
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
        private readonly IEventQueue _eventQueue;
        private readonly IUserIdentityHolder _userIdentityHolder;

        #endregion

        #region Constructor

        /// <summary>
        /// Creates a new instance of <see cref="Router"/>
        /// Implements <see cref="ICommandSender"/>
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="eventQueue"></param>
        /// <param name="userIdentityHolder"></param>
        /// <param name="correlationId"></param>
        public Router(IServiceProvider serviceProvider, IEventQueue eventQueue, IUserIdentityHolder userIdentityHolder, Guid? correlationId = null)
        {
            _serviceProvider = serviceProvider;
            _userIdentityHolder = userIdentityHolder;
            _correlationId = correlationId ?? Guid.NewGuid();
            _eventQueue = eventQueue;
        }

        #endregion

        #region Command implementations

        /// <inheritdoc />
        public async Task<TResponse> Send<TResponse, TCommand>(TCommand message)
            where TResponse : class
            where TCommand : ICommand<TResponse>
        {
            var @event = new Event(
                _userIdentityHolder.UserId ?? "NoId",
                _correlationId);
            @event.SetCommand<TResponse, TCommand>(message);

            try
            {
                var handler = _serviceProvider.GetService(typeof(ICommandHandler<TResponse, TCommand>)) as ICommandHandler<TResponse, TCommand>;

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
                if (ShouldEnque(typeof(TCommand)))
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
                var handler = GetHandler<ICommandHandler<TCommand>>(message.GetType().Name);

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
        public async Task<TResponse> Query<TResponse, TQuery>(TQuery query) where TQuery : IQuery
        {
            var @event = new Event(
                _userIdentityHolder.UserId ?? "NoId",
                _correlationId);

            @event.SetQuery<TResponse, TQuery>(query);

            try
            {
                var handler = GetHandler<IQueryHandler<TResponse, TQuery>>(query.GetType().Name);

                var response = await handler.Handle(query);

                @event.Success(response);
                return response;
            }
            catch (Exception ex)
            {
                @event.Failed(ex);
                throw;
            }
            finally
            {
                if (ShouldEnque(typeof(TQuery)))
                {
                    _eventQueue.Enqueue(@event);
                }
            }
        }

        #endregion

        #region Private methods

        private T GetHandler<T>(string eventTypeName)
        {
            var handler = _serviceProvider.GetService<T>();

            if (handler is null)
            {
                throw new InvalidOperationException($"there's no handler registered for {eventTypeName}");
            }

            return handler;
        }

        private static bool ShouldEnque(Type type)
        {
            var attributes = type.GetCustomAttributes(typeof(NoEnqueueEventAttribute), false);

            return attributes == null || !attributes.Any();
        }

        #endregion
    }
}
