using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.Tests.Fakes;
using Newtonsoft.Json;
using System;
using Xunit;

namespace Goiar.Simple.Cqrs.Tests.Entitites
{
    public class EventTests
    {
        #region Fields

        private readonly JsonSerializerSettings _settings = new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None
        };

        #endregion

        #region New Tests

        [Fact]
        public void New_ShouldSetDefaultsAndSendedFields()
        {
            var createdBy = "Me c:";
            var correlationId = Guid.NewGuid();

            var @event = new Event(createdBy, correlationId);

            Assert.Equal(createdBy, @event.CreatedBy);
            Assert.Equal(correlationId, @event.CorrelationId);
            Assert.NotEqual(default(Guid), @event.Id);
            Assert.NotEqual(default(DateTime), @event.CreatedOn);
            Assert.Equal(default(Guid), @event.EntityId);
            Assert.Equal(default(string), @event.CommandName);
            Assert.Equal(default(string), @event.Content);
            Assert.Equal(default(string), @event.Result);
        }

        #endregion

        #region SetCommand Tests

        [Fact]
        public void SetCommand_ShouldSetCommandName()
        {
            var command = new FakeSimpleCommand("Im a command c:");

            var @event = new Event("CreatedBy", Guid.NewGuid());

            @event.SetCommand<WeirdVoid, FakeSimpleCommand>(command);

            Assert.Equal("Fake Simple Command", @event.CommandName);
        }


        [Fact]
        public void SetCommand_ShouldSetEntityId()
        {
            var command = new FakeSimpleCommand("Im a command c:");

            var @event = new Event("CreatedBy", Guid.NewGuid());

            @event.SetCommand<WeirdVoid, FakeSimpleCommand>(command);

            Assert.Equal(command.EntityId, @event.EntityId);
        }


        [Fact]
        public void SetCommand_ShouldSetContent()
        {
            var command = new FakeSimpleCommand("Im a command c:");

            var @event = new Event("CreatedBy", Guid.NewGuid());

            @event.SetCommand<WeirdVoid, FakeSimpleCommand>(command);

            Assert.Equal(command, @event.Content);
        }

        #endregion

        #region Failed Tests

        [Fact]
        public void Failed_ShouldSetResult()
        {
            var ex = new Exception("Im can make your code break c:");

            var @event = new Event("CreatedBy", Guid.NewGuid());

            @event.Failed(ex);

            Assert.Equal(ex, @event.Result);
        }

        [Fact]
        public void Failed_ShouldSetTimeElapsed()
        {
            var ex = new Exception("Im can make your code break c:");
            var command = new FakeSimpleCommand("Im a command c:");

            var @event = new Event("CreatedBy", Guid.NewGuid());
            @event.SetCommand<WeirdVoid, FakeSimpleCommand>(command);

            @event.Failed(ex);

            Assert.True(@event.TimeElapsed.Ticks > 0);
        }

        #endregion

        #region Success Tests

        [Fact]
        public void Success_ShouldSetResult_WithAnActualResponse()
        {
            var response = new FakeCommandResponse("Im a good response =D");

            var @event = new Event("CreatedBy", Guid.NewGuid());

            @event.Success(response);

            Assert.Equal(response, @event.Result);
        }

        [Fact]
        public void Success_ShouldSetResultInSuccess_WithAWeirdVoid()
        {
            var @event = new Event("CreatedBy", Guid.NewGuid());

            @event.Success(WeirdVoid.Value);

            Assert.Equal("Success", @event.Result);
        }
        
        [Fact]
        public void Success_ShouldSetTimeElapsed()
        {
            var command = new FakeSimpleCommand("Im a command c:");
            var @event = new Event("CreatedBy", Guid.NewGuid());

            @event.SetCommand<WeirdVoid, FakeSimpleCommand>(command);
            @event.Success(WeirdVoid.Value);

            Assert.True(@event.TimeElapsed.TotalMilliseconds > 0);
        }

        #endregion
    }
}
