using System;

namespace Goiar.Simple.Cqrs.Attributes
{
    /// <summary>
    /// A class that allows the message get through without passing through the event store
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class NoEnqueueEventAttribute : Attribute
    {
        /// <summary>
        /// Creates a new <see cref="NonSerializedAttribute"/>
        /// </summary>
        public NoEnqueueEventAttribute()
        {
        }
    }

}
