using System.IO;
using UndefinedNetworking.Exceptions;

namespace GameEngine.Resources
{
    public class ExternalShader : Shader
    {
        public ExternalShader(string path, string name, int id)
        {
            if (!File.Exists(path)) throw new ShaderException("file not exists");
            if (!path.EndsWith(".png")) throw new SpriteException("unknown file extension");
            Name = name;
            Id = id;
            var fileName = System.IO.Path.Combine(Paths.ExternalResources, name + ".png");
            File.Copy(path, fileName);
            //UnityEditor.AssetDatabase.ImportAsset(fileName);
            UnityShader =
                UnityEngine.Resources.Load<UnityEngine.Shader>(System.IO.Path.Combine(Paths.ExternalResources, name));
        }

        public override string Path { get; }
        public override int Id { get; }
        public override string Name { get; }
        public override UnityEngine.Shader UnityShader { get; }
    }
}