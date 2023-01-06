using System.Collections.Generic;
using System.Linq;
using GameEngine.Exceptions;
using UndefinedNetworking.GameEngine.Resources;

namespace GameEngine.Resources
{
    public sealed class InternalResourcesManager : IResourcesManager
    {
        private readonly Dictionary<string, IResource> _resources = new();
        private readonly List<string> _resourcesFiles = new();

        public IEnumerable<string> ResourcesFiles => _resourcesFiles;

        public bool IsLoaded { get; private set; }
        public int ResourcesCount => _resources.Count;
        public string ResourcesFolder => throw new ResourcesException("internal resources dont have a folder");

        public void LoadAll()
        {
            IsLoaded = IsLoaded ? throw new ResourcesLoadException("resources already loaded") : true;
            LoadInternalShader(InternalShaderType.Blink, "Shaders/Blink_Shader");
            LoadInternalShader(InternalShaderType.ObjectRectMask, "Shaders/RectMask_Shader");
            LoadInternalShader(InternalShaderType.WorldRectMask, "Shaders/WorldRectMask_Shader");
        }

        public IResource GetResource(int id)
        {
            return id < 0 || id >= _resources.Count
                ? throw new ResourceNotFoundException("id less then 0 or greater then resources count")
                : _resources.ElementAt(id).Value;
        }

        public IResource GetResource(string path)
        {
            throw new ResourcesException("internal resources dont have a folder");
        }

        public bool TryGetResource(int id, out IResource? resource)
        {
            return (resource = id >= 0 && id < _resources.Count ? _resources.ElementAt(id).Value : null) is not null;
        }

        public bool TryGetResource(string pathInResources, out IResource? resource)
        {
            throw new ResourcesException("internal resources dont have a folder");
        }


        public ISprite GetSprite(string pathInResources)
        {
            return _resources.ContainsKey(pathInResources)
                ? _resources[pathInResources] as Sprite ?? throw new ResourceNotFoundException("resource is not sprite")
                : throw new ResourceNotFoundException();
        }

        public bool TryGetSprite(string pathInResources, out ISprite? sprite)
        {
            return (sprite = _resources.ContainsKey(pathInResources) ? _resources[pathInResources] as ISprite : null) is
                not null;
        }

        public InternalShader GetInternalShader(InternalShaderType type)
        {
            if (!_resources.ContainsKey(type.ToString())) throw new ResourceNotFoundException("shader not found");
            return _resources[type.ToString()] as InternalShader;
        }

        private void LoadInternalShader(InternalShaderType type, string path)
        {
            _resources.Add(type.ToString(), new InternalShader(path, type.ToString(), _resources.Count));
        }

        private void LoadSprite(InternalSpriteType type, string path)
        {
            _resources.Add(type.ToString(), new Sprite(path, _resources.Count));
        }
    }
}