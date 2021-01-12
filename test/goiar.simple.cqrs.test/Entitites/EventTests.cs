using Newtonsoft.Json;
using Goiar.Simple.Cqrs.Entities;
using Goiar.Simple.Cqrs.Tests.Fakes;
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

            var @event = new Event("CreatedBy", Guid.NewGuid(), _settings);

            @event.SetCommand<WeirdVoid, FakeSimpleCommand>(command);

            Assert.Equal("Fake Simple Command", @event.CommandName);
        }


        [Fact]
        public void SetCommand_ShouldSetEntityId()
        {
            var command = new FakeSimpleCommand("Im a command c:");

            var @event = new Event("CreatedBy", Guid.NewGuid(), _settings);

            @event.SetCommand<WeirdVoid, FakeSimpleCommand>(command);

            Assert.Equal(command.EntityId, @event.EntityId);
        }


        [Fact]
        public void SetCommand_ShouldSetContent()
        {
            var command = new FakeSimpleCommand("Im a command c:");

            var serializedCommand = JsonConvert.SerializeObject(command, _settings);

            var @event = new Event("CreatedBy", Guid.NewGuid(), _settings);

            @event.SetCommand<WeirdVoid, FakeSimpleCommand>(command);

            Assert.Equal(serializedCommand, @event.Content);
        }

        #endregion

        #region Failed Tests

        [Fact]
        public void Failed_ShouldSetResult()
        {
            var ex = new Exception("Im can make your code break c:");

            var serializedEx = JsonConvert.SerializeObject(ex, _settings);

            var @event = new Event("CreatedBy", Guid.NewGuid(), _settings);

            @event.Failed(ex);

            Assert.Equal(serializedEx, @event.Result);
        }

        #endregion

        #region Success Tests

        [Fact]
        public void Success_ShouldSetResult_WithAnActualResponse()
        {
            var response = new FakeCommandResponse("Im a good response =D");

            var serializedresponse = JsonConvert.SerializeObject(response, _settings);

            var @event = new Event("CreatedBy", Guid.NewGuid(), _settings);

            @event.Success(response);

            Assert.Equal(serializedresponse, @event.Result);
        }

        [Fact]
        public void Success_ShouldSetResultInSuccess_WithAWeirdVoid()
        {
            var @event = new Event("CreatedBy", Guid.NewGuid(), _settings);

            @event.Success(WeirdVoid.Value);

            Assert.Equal("Success", @event.Result);
        }

        #endregion
    }
}
