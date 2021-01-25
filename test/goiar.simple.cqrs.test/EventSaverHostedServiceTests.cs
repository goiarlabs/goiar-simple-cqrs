using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.Persistance;
using Goiar.Simple.Cqrs.test.Fakes.Queues;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Goiar.Simple.Cqrs.test
{
    public class EventSaverHostedServiceTests
    {
        #region Fields

        private readonly Mock<IServiceProvider> _serviceProvider;
        private readonly Mock<IServiceProvider> _scopedProvider;
        private readonly EventQueueExposer _eventQueue;
        private readonly Mock<ILogger<EventSaverHostedService>> _logger;

        private readonly EventSaverHostedService _classUnderTest;

        #endregion

        #region Constructor

        public EventSaverHostedServiceTests()
        {
            (_serviceProvider, _scopedProvider) = CreateProvider();
            _eventQueue = new EventQueueExposer();
            _logger = new Mock<ILogger<EventSaverHostedService>>();

            _classUnderTest = new EventSaverHostedService(
                _serviceProvider.Object,
                _eventQueue,
                _logger.Object
            );
        }

        #endregion

        #region Execute Tests

        [Fact]
        public async Task ExcecuteAsync_ShouldLogOperationCanceledException_OnCancellationTokenExcecuted()
        {
            var cts = new CancellationTokenSource();

            var task = _classUnderTest.StartAsync(cts.Token);
            await Task.Run(async () =>
            {
                cts.Cancel();
                await task;
            });

            _logger.Verify(
                a => a.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.IsAny<OperationCanceledException>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)
                ), Times.Once);
        }
        
        [Fact]
        public async Task ExcecuteAsync_ShouldLogCriticalException_WhenNoEventStoreIsConfigured()
        {
            var cts = new CancellationTokenSource();

            var @event = new Event("createdBy", Guid.NewGuid());
            var task = _classUnderTest.StartAsync(cts.Token);

            _eventQueue.Enqueue(@event);

            await task;

            _logger.Verify(
                a => a.Log(
                    LogLevel.Critical,
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => true),
                    It.Is<Exception>(a=>a.Message == "No event store was found"),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)
                ), Times.Once);
        }

        [Fact]
        public async Task ExcecuteAsync_ShouldExcecuteSaveOnEventStore()
        {
            var cts = new CancellationTokenSource();
            var eventStore = new Mock<IEventStore>();
            _scopedProvider
                .Setup(a => a.GetService(typeof(IEventStore)))
                .Returns(eventStore.Object);

            var @event = new Event("createdBy", Guid.NewGuid());
            var task = _classUnderTest.StartAsync(cts.Token);

            _eventQueue.Enqueue(@event);

            await task;

            eventStore.Verify(a => a.Save(@event), Times.Once);
        }

        #endregion

        #region Private methods

        private (Mock<IServiceProvider>, Mock<IServiceProvider>) CreateProvider()
        {
            var serviceProvider = new Mock<IServiceProvider>();
            var scopedProvider = new Mock<IServiceProvider>();
            var scopeFactory = new Mock<IServiceScopeFactory>();
            var serviceScope = new Mock<IServiceScope>();

            serviceProvider.Setup(a => a.GetService(typeof(IServiceScopeFactory)))
                .Returns(scopeFactory.Object);
            scopeFactory.Setup(s => s.CreateScope()).Returns(serviceScope.Object);
            serviceScope.Setup(a => a.ServiceProvider).Returns(scopedProvider.Object);

            return (serviceProvider, scopedProvider);
        }

        #endregion
    }
}
