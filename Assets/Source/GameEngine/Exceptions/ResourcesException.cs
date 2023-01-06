using System;

namespace GameEngine.Exceptions
{
    public class ResourcesException : Exception
    {
        public ResourcesException(string? msg = null) : base(msg)
        {
        }
    }
}