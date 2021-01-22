using Newtonsoft.Json;
using Goiar.Simple.Cqrs.Commands;
using System;
using System.Text;
using System.Diagnostics;

namespace Goiar.Simple.Cqrs.Entities
{
    /// <summary>
    /// Class used to persist commands into the event store
    /// </summary>
    public class Event 
    {
        #region Fields

        private readonly Stopwatch _stopWatch;

        #endregion

        #region Constructors

        /// <summary>
        /// EF Constructor
        /// Done because we have some transformation on domain's constructor / dotnet 2.1 compatibility
        /// </summary>
        protected Event() { }

        /// <summary>
        /// Instanciates a <see cref="Event"/>
        /// Serializes the command into the content
        /// </summary>
        /// <param name="createdBy"> An identifier of the person that created this event </param>
        /// <param name="correlationId"> A batch identifier </param>
        /// <param name="serializerSettings"> If null instanciates a new setting ignoring loop references and with idented format </param>
        public Event(string createdBy, Guid correlationId)
        {
            Id = Guid.NewGuid();
            CreatedBy = createdBy;
            CreatedOn = DateTime.UtcNow;
            CorrelationId = correlationId;

            _stopWatch = new Stopwatch();
        }

        #endregion

        #region Properties

        /// <summary>
        /// Event's identifier
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// User that created or submited this event
        /// </summary>
        public string CreatedBy { get; set; }

        /// <summary>
        /// Date of summit
        /// </summary>
        public DateTime CreatedOn { get; set; }

        /// <summary>
        /// Batch's identifier
        /// </summary>
        public Guid CorrelationId { get; set; }

        /// <summary>
        /// Affected Entity's  identifier
        /// </summary>
        public Guid EntityId { get; set; }

        /// <summary>
        /// Name of the command
        /// </summary>
        public string CommandName { get; set; }

        /// <summary>
        /// Command's content
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Result of the command
        /// "Success" if void
        /// Serialized exception if failed
        /// </summary>
        public object Result { get; set; }

        /// <summary>
        /// The time it took to proccess the command
        /// </summary>
        public TimeSpan TimeElapsed { get; set; }

        #endregion

        #region Public Methods

        /// <summary>
        /// Sets the command used
        /// </summary>
        /// <typeparam name="TResponse"></typeparam>
        /// <typeparam name="TCommand"></typeparam>
        /// <param name="command"></param>
        public void SetCommand<TResponse, TCommand>(object command) 
            where TCommand : ICommand<TResponse>
            where TResponse : class
        {
            var typedCommand = command as ICommand<TResponse>;
            EntityId = typedCommand.EntityId;
            Content = typedCommand;
            CommandName = SeparatePascalCase(command.GetType().Name);
            _stopWatch.Start();
        }

        /// <summary>
        /// Fills the result with the exception that made this fail
        /// </summary>
        /// <param name="ex"></param>
        public void Failed(Exception ex)
        {
            Result = ex;
            _stopWatch.Stop();
            TimeElapsed = _stopWatch.Elapsed;
        }

        /// <summary>
        /// Sets the result with 
        /// "Success" if void
        /// The serialized response
        /// </summary>
        /// <param name="response"></param>
        public void Success(object response)
        {
            if (response is WeirdVoid)
            {
                Result = "Success";
            }
            else
            {
                Result = response;
            }

            _stopWatch.Stop();
            TimeElapsed = _stopWatch.Elapsed;
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Converts "HiIAmAString" into "Hi I Am A String"
        /// Empry if input is empty
        /// </summary>
        /// <param name="str">input string to convert</param>
        /// <returns>A spaced separeted string</returns>
        private static string SeparatePascalCase(string str)
        {
            //If empty we no need to do anything
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty;
            }

            // We pop the first that is Upper case to no put spaces at the first prosition
            var result = new StringBuilder();
            result.Append(str[0]);
            str = str.Remove(0, 1);

            foreach (var chr in str)
            {
                //If it has an upper case put an space
                if (char.IsUpper(chr))
                {
                    result.Append(" ");
                }

                result.Append(chr);
            }

            return result.ToString();
        }

        #endregion
    }

    public class Event<TResponse> : Event where TResponse : class
    {
        #region Constructors

        /// <summary>
        /// EF Constructor
        /// Done because we have some transformation on domain's constructor / dotnet 2.1 compatibility
        /// </summary>
        protected Event() : base() { }

        /// <summary>
        /// Instanciates a <see cref="Event"/>
        /// Serializes the command into the content
        /// </summary>
        /// <param name="createdBy"> An identifier of the person that created this event </param>
        /// <param name="correlationId"> A batch identifier </param>
        /// <param name="serializerSettings"> If null instanciates a new setting ignoring loop references and with idented format </param>
        public Event(string createdBy, Guid correlationId, JsonSerializerSettings serializerSettings = null) : base(createdBy, correlationId)
        {
        }

        #endregion

        #region Properties

        public ICommand<TResponse> Command => Content as ICommand<TResponse>;

        #endregion
    }

}
