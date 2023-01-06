using System;

namespace GameEngine.Exceptions
{
    public class EngineException : Exception
    {
        public EngineException(string msg) : base(msg)
        {
        }
    }
}