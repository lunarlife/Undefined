using System;

namespace GameEngine.Exceptions
{
    public class ResourcesLoadException : Exception
    {
        public ResourcesLoadException(string? msg = null) : base(msg)
        {
        }
    }
}