using System;

namespace GameEngine.Exceptions
{
    public class DownloadingResourceException : Exception
    {
        public DownloadingResourceException(string msg) : base(msg)
        {
        }
    }
}