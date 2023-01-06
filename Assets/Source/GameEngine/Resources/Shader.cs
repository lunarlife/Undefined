using UndefinedNetworking.GameEngine.Resources;

namespace GameEngine.Resources
{
    public abstract class Shader : IShader
    {
        public abstract string Name { get; }
        public abstract UnityEngine.Shader UnityShader { get; }
        public abstract string Path { get; }
        public string FullPath => System.IO.Path.Join(Paths.ExternalResources, Name);
        public abstract int Id { get; }
    }
}