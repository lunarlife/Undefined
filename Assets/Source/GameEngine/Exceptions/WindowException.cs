using System;

namespace GameEngine.Exceptions
{
    public class WindowException : Exception
    {
        public WindowException(string msg) : base(msg)
        {
        }
    }
}