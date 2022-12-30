using UndefinedNetworking.GameEngine.Resources;

namespace GameEngine.Resources
{
    public abstract class Shader : IShader
    {
        public abstract string ResourcePath { get; }
        public abstract string Name { get; }
        public abstract UnityEngine.Shader UnityShader { get; }
    }
}