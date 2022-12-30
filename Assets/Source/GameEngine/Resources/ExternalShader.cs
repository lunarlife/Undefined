using System.IO;
using UndefinedNetworking.Exceptions;

namespace GameEngine.Resources
{
    public class ExternalShader : Shader
    {
        public override string ResourcePath { get; }
        public override string Name { get; }
        public override UnityEngine.Shader UnityShader { get; }

        public ExternalShader(string path, string name)
        {
            if (!File.Exists(path)) throw new ShaderException("file not exists");
            if (!path.EndsWith(".png")) throw new SpriteException("unknown file extension");
            Name = name;
            var fileName = Path.Combine(Paths.ExternalShaders, name + ".png");
            File.Copy(path, fileName);
            UnityEditor.AssetDatabase.ImportAsset(fileName);
            UnityShader = UnityEngine.Resources.Load<UnityEngine.Shader>(Path.Combine(Paths.ExternalShaders, name));
        }
    }
}