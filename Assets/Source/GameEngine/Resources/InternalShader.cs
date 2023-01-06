namespace GameEngine.Resources
{
    public class InternalShader : Shader
    {
        public InternalShader(string path, string name, int id)
        {
            Name = name;
            Id = id;
            Path = path;
            UnityShader = UnityEngine.Resources.Load<UnityEngine.Shader>(path);
        }

        public override string Path { get; }
        public override int Id { get; }
        public override string Name { get; }
        public override UnityEngine.Shader UnityShader { get; }
    }
}