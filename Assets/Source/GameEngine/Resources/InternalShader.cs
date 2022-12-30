using System.IO;
using UndefinedNetworking.Exceptions;
using UndefinedNetworking.GameEngine.Resources;

namespace GameEngine.Resources
{
    public class InternalShader : Shader
    {
        public override string ResourcePath { get; }
        public override string Name { get; }
        public override UnityEngine.Shader UnityShader { get; }

        public InternalShader(string path, string name)
        {
            Name = name;
            UnityShader = UnityEngine.Resources.Load<UnityEngine.Shader>(path);
        }
    }
}