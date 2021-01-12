using Moq;
using Goiar.Simple.Cqrs.Commands;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.Queries;
using Goiar.Simple.Cqrs.Tests.Fakes;
using Goiar.Simple.Cqrs.Tests.Fakes.Commands;
using Goiar.Simple.Cqrs.UserIdentities;
using System;
using System.Threading.Tasks;
using Xunit;
using Goiar.Simple.Cqrs.test.Fakes.Queues;

namespace Goiar.Simple.Cqrs.Tests
{
    public class RouterTests
    {
        #region Fields

        private readonly Mock<IServiceProvider> _serviceProvider;
        private readonly EventQueueExposer _eventQueue;
        private readonly Mock<IUserIdentityHolder> _userIdentityHolder;
        private readonly Router _classUnderTest;

        #endregion

        #region Constructor

        public RouterTests()
        {
            _serviceProvider = new Mock<IServiceProvider>();
            _userIdentityHolder = new Mock<IUserIdentityHolder>();

            _eventQueue = new EventQueueExposer();

            _classUnderTest = new Router(_serviceProvider.Object, _eventQueue, _userIdentityHolder.Object);
        }

        #endregion

        #region Send Tests

        [Fact]
        public async Task Send_ShouldUpdateMessageOnFakeHandler()
        {
            var command = new FakeSimpleCommand("I Have a message!");
            var commandHandler = new FakeSimpleCommandHandler();

            RegisterTypeOnProvider<ICommandHandler<FakeSimpleCommand>>(commandHandler);

            await _classUnderTest.Send(command);

            Assert.Equal(command.Message, commandHandler.Message);
        }

        [Fact]
        public async Task Send_ShouldThrowInvalidOperationException_WhenTheHandlerWasNotRegistered()
        {
            var command = new FakeSimpleCommand("I Have a message!");
            var commandHandler = new FakeSimpleCommandHandler();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _classUnderTest.Send(command)
            );

            Assert.Equal("there's no command handler regitered for FakeSimpleCommand", exception.Message);
        }

        [Fact]
        public async Task Send_ShouldSaveTheCommandIntoEventStoreWithNoId_WhenIdentityIsNull()
        {
            var command = new FakeSimpleCommand("I Have a message!");
            var commandHandler = new FakeSimpleCommandHandler();

            RegisterTypeOnProvider<ICommandHandler<FakeSimpleCommand>>(commandHandler);

            _userIdentityHolder.Setup(a => a.UserId).Returns<string>(null);

            await _classUnderTest.Send(command);

            Assert.Contains(_eventQueue.InternalQueue,
                e => e.CommandName == "Fake Simple Command"
                    && e.CreatedBy == "NoId"
                    && (e.Content as FakeSimpleCommand).Message == command.Message
                    && (e.Result as string) == "Success");
        }

        [Fact]
        public async Task Send_ShouldSaveTheCommandIntoEventStoreWithId_WhenIdentityExist()
        {
            var command = new FakeSimpleCommand("I Have a message!");
            var commandHandler = new FakeSimpleCommandHandler();

            var createdBy = "createdBy";

            RegisterTypeOnProvider<ICommandHandler<FakeSimpleCommand>>(commandHandler);

            _userIdentityHolder.Setup(a => a.UserId).Returns(createdBy);

            await _classUnderTest.Send(command);

            Assert.Contains(_eventQueue.InternalQueue,
                e => e.CommandName == "Fake Simple Command"
                    && e.CreatedBy == createdBy
                    && (e.Content as FakeSimpleCommand).Message == command.Message
                    && (e.Result as string) == "Success");
        }

        [Fact]
        public async Task Send_ShouldLogTheFailureAndThrowItAgain_WhenExceptionWasThrown()
        {
            var command = new FakeSimpleCommand("I Have a message!");
            var commandHandler = new Mock<ICommandHandler<FakeSimpleCommand>>();
            var responseMessage = "weee i have failed =D";

            var exception = new Exception(responseMessage);

            commandHandler.Setup(a => a.Handle(It.Is<FakeSimpleCommand>(s => s == command)))
                .Throws(exception);

            RegisterTypeOnProvider(commandHandler.Object);

            var exceptionRes = await Assert.ThrowsAsync<Exception>(async () => await _classUnderTest.Send(command));

            Assert.Equal(exception, exceptionRes);

            Assert.Contains(_eventQueue.InternalQueue,
                e => e.CommandName == "Fake Simple Command"
                    && (e.Content as FakeSimpleCommand).Message == command.Message
                    && (e.Result as FakeCommandResponse).Message == responseMessage);
        }

        [Fact]
        public async Task Send_ShouldNotEnqueueMessage_WhenAttributeIsAdded()
        {
            var command = new FakeSimpleNoEnqueueCommand("I Have a message!");
            var commandHandler = new FakeSimpleCommandHandler();

            RegisterTypeOnProvider<ICommandHandler<FakeSimpleNoEnqueueCommand>>(commandHandler);

            await _classUnderTest.Send(command);

            Assert.Equal(command.Message, commandHandler.Message);
            Assert.Empty(_eventQueue.InternalQueue);
        }

        #endregion

        #region SendWithResponse Tests

        [Fact]
        public async Task SendWithResponse_ShouldReturnTheSendedMessage()
        {
            var command = new FakeResponseCommand("I Have a message!");
            var commandHandler = new FakeResponseCommandHandler();

            RegisterTypeOnProvider<ICommandHandler<FakeCommandResponse, FakeResponseCommand>>(commandHandler);

            var response = await _classUnderTest.Send<FakeCommandResponse, FakeResponseCommand>(command);

            Assert.Equal(command.Message, response.Message);
        }

        [Fact]
        public async Task SendWithResponse_ShouldSaveTheCommandIntoEventStoreWithNoId_WhenIdIsNull()
        {
            var command = new FakeResponseCommand("I Have a message!");
            var commandHandler = new FakeResponseCommandHandler();

            _userIdentityHolder.Setup(a => a.UserId).Returns<string>(null);

            RegisterTypeOnProvider<ICommandHandler<FakeCommandResponse, FakeResponseCommand>>(commandHandler);

            var response = await _classUnderTest.Send<FakeCommandResponse, FakeResponseCommand>(command);

            Assert.Contains(_eventQueue.InternalQueue,
                e => e.CommandName == "Fake Response Command"
                    && (e.Content as FakeSimpleCommand).Message == command.Message
                    && (e.Result as FakeCommandResponse).Message == command.Message
                    && e.CreatedBy == "NoId");
        }

        [Fact]
        public async Task SendWithResponse_ShouldSaveTheCommandIntoEventStoreWithId_WhenIdExists()
        {
            var command = new FakeResponseCommand("I Have a message!");
            var commandHandler = new FakeResponseCommandHandler();
            var createdBy = "CreatedBy";

            _userIdentityHolder.Setup(a => a.UserId).Returns(createdBy);

            RegisterTypeOnProvider<ICommandHandler<FakeCommandResponse, FakeResponseCommand>>(commandHandler);

            var response = await _classUnderTest.Send<FakeCommandResponse, FakeResponseCommand>(command);

            Assert.Contains(_eventQueue.InternalQueue,
                e => e.CommandName == "Fake Response Command"
                    && (e.Content as FakeSimpleCommand).Message == command.Message
                    && (e.Result as FakeCommandResponse).Message == command.Message
                    && e.CreatedBy == createdBy);
        }

        [Fact]
        public async Task SendWithResponse_ShouldThrowInvalidOperationException_WhenTheHandlerWasNotRegistered()
        {
            var command = new FakeResponseCommand("I Have a message!");
            var commandHandler = new FakeResponseCommandHandler();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _classUnderTest.Send<FakeCommandResponse, FakeResponseCommand>(command)
            );

            Assert.Equal("there's no command handler regitered for FakeResponseCommand", exception.Message);
        }

        [Fact]
        public async Task SendWithResponse_ShouldLogTheFailureAndThrowItAgain_WhenExceptionWasThrown()
        {
            var command = new FakeResponseCommand("I Have a message!");
            var commandHandler = new Mock<ICommandHandler<FakeCommandResponse, FakeResponseCommand>>();
            var resultMessage = "weee i have failed =D";

            var exception = new Exception(resultMessage);

            commandHandler.Setup(a => a.Handle(It.Is<FakeResponseCommand>(s => s == command)))
                .Throws(exception);

            RegisterTypeOnProvider(commandHandler.Object);

            var exceptionRes = await Assert.ThrowsAsync<Exception>(async () =>
                await _classUnderTest.Send<FakeCommandResponse, FakeResponseCommand>(command));

            Assert.Equal(exception, exceptionRes);

            Assert.Contains(_eventQueue.InternalQueue,
                e => e.CommandName == "Fake Response Command"
                    && (e.Content as FakeSimpleCommand).Message == command.Message
                    && (e.Result as Exception).Message == resultMessage);
        }

        [Fact]
        public async Task SendWithResponse_ShouldNotEnqueueMessage_WhenAttributeIsAdded()
        {
            var command = new FakeNoEnqueueResponseCommand("I Have a message!");
            var commandHandler = new FakeResponseCommandHandler();

            RegisterTypeOnProvider<ICommandHandler<FakeCommandResponse, FakeNoEnqueueResponseCommand>>(commandHandler);

            var response = await _classUnderTest.Send<FakeCommandResponse, FakeNoEnqueueResponseCommand>(command);

            Assert.Equal(command.Message, response.Message);
            Assert.Empty(_eventQueue.InternalQueue);
        }

        #endregion

        #region Query Tests

        [Fact]
        public async Task Query_ShouldReturnTheSendedMessage()
        {
            var query = new FakeQuery("I'm a message :D");
            var queryHandler = new FakeQueryHandler();

            RegisterTypeOnProvider<IQueryHandler<FakeQueryResponse, FakeQuery>>(queryHandler);

            var result = await _classUnderTest.Query<FakeQueryResponse, FakeQuery>(query);

            Assert.Equal(query.Message, result.Message);
        }

        [Fact]
        public async Task Query_ShouldThrowInvalidOperationException_WhenTheHandlerWasNotRegistered()
        {
            var query = new FakeQuery("I'm a message :D");
            var queryHandler = new FakeQueryHandler();

            var exception = await Assert.ThrowsAsync<InvalidOperationException>(
                () => _classUnderTest.Query<FakeQueryResponse, FakeQuery>(query)
            );

            Assert.Equal("there's no query handler regitered for FakeQuery", exception.Message);
        }

        #endregion

        #region Private methods

        private void RegisterTypeOnProvider<TType>(TType instace)
        {
            _serviceProvider.Setup(a => a.GetService(
                It.Is<Type>(t => t.GUID == typeof(TType).GUID)
            )).Returns(instace);
        }

        #endregion
    }
}
