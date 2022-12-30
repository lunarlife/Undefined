using System.Collections.Generic;
using System.Data;
using GameEngine.Resources;
using UndefinedNetworking.GameEngine.Resources;
using Utils;

namespace GameData
{
    public static class Data
    {
        private static bool _isLoaded;
        private static readonly Dictionary<string, Sprite> Sprites = new();
        private static readonly Dictionary<string, ExternalShader> ExternalShaders = new();
        private static readonly Dictionary<InternalShaderType, InternalShader> InternalShaders = new();

        public static Version Version { get; } = new("0.1alpha");
        
        
        public static void Load()
        {
            if (_isLoaded) throw new DataException("Data is loaded");
            _isLoaded = true;
            LoadInternalShader(InternalShaderType.ObjectRectMask, "Shaders/RectMask_Shader");
            LoadInternalShader(InternalShaderType.RectMaskWorld, "Shaders/WorldRectMask_Shader");
        }
        private static void LoadSprite(string name, string path) => Sprites.Add(name, new Sprite(path, name));
        public static ISprite GetSprite(string name)
        {
            if (!Sprites.ContainsKey(name)) throw new DataException("sprite not found");
            return Sprites[name];
        }
        private static void LoadInternalShader(InternalShaderType type, string path) => InternalShaders.Add(type, new InternalShader(path, type.ToString()));
        private static void LoadExternalShader(string name, string path) => ExternalShaders.Add(name, new ExternalShader(path, name));
        public static ExternalShader GetExternalShader(string name)
        {
            if (!ExternalShaders.ContainsKey(name)) throw new DataException("shader not found");
            return ExternalShaders[name];
        }
        public static InternalShader GetInternalShader(InternalShaderType type)
        {
            if (!InternalShaders.ContainsKey(type)) throw new DataException("shader not found");
            return InternalShaders[type];
        }
    }
}