using System.IO;
using GameEngine.Exceptions;
using UndefinedNetworking.GameEngine.Resources;

namespace GameEngine.Resources
{
    public class DownloadingResource : IResource
    {
        private bool _started;
        private FileStream _stream;

        public DownloadingResource(string pathInResource, int id)
        {
            Id = id;
            Path = pathInResource;
            FullPath = System.IO.Path.Join(Paths.ExternalResources, pathInResource);
        }

        public int Downloaded => (int)_stream.Length;
        public long TotalSize { get; private set; }
        public string Path { get; }
        public string FullPath { get; }
        public int Id { get; }

        public void Start(long totalSize)
        {
            if (_started) throw new DownloadingResourceException("already started");
            TotalSize = totalSize;
            _started = true;
            _stream = File.OpenWrite(FullPath);
        }

        public void Write(byte[] buffer)
        {
            _stream.Write(buffer);
        }

        public void Stop()
        {
            if (!_started) throw new DownloadingResourceException("already stopped");
            _started = false;
            _stream.Flush();
            _stream.Close();
        }
    }
}