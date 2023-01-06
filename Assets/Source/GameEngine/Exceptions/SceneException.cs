using System;

namespace GameEngine.Exceptions
{
    public class SceneException : Exception
    {
        public SceneException(string? msg) : base(msg)
        {
        }
    }
}