using System;

namespace GameEngine.Exceptions
{
    public class ResourceNotFoundException : Exception
    {
        public ResourceNotFoundException(string? msg = null) : base(msg)
        {
        }
    }
}