using System;

namespace Goiar.Simple.Cqrs.Attributes
{
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class NoEnqueueEventAttribute : Attribute
    {
        public NoEnqueueEventAttribute()
        {
        }
    }

}
