using System;

namespace goiar.simple.cqrs.sample.aspnetcore.rabbitmq.pub.Models
{
    public class Human
    {
        #region Constructors
        
        public Human(Guid id, string name)
        {
            Id = id;
            Name = name;
        }

        public Human(string name) : this(Guid.NewGuid(), name)
        { }

        #endregion

        #region Properties

        public Guid Id { get; set; }
        public string Name { get; set; }

        #endregion
    }
}
