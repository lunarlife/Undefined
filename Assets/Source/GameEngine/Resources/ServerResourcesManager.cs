using System.Collections.Generic;
using System.Linq;
using GameEngine.Exceptions;
using UndefinedNetworking.GameEngine.Resources;

namespace GameEngine.Resources
{
    internal sealed class ServerResourcesManager : IResourcesManager
    {
        private readonly Dictionary<string, IResource> _resources = new();
        private readonly List<string> _resourcesFiles = new();

        public ServerResourcesManager()
        {
            ResourcesFolder = Paths.ExternalResources;
        }

        public IEnumerable<string> ResourcesFiles => _resourcesFiles;

        public bool IsLoaded { get; private set; }
        public int ResourcesCount => _resources.Count;
        public string ResourcesFolder { get; }

        public void LoadAll()
        {
            IsLoaded = IsLoaded ? throw new ResourcesLoadException("resources already loaded") : true;
        }

        public IResource GetResource(int id)
        {
            return id < 0 || id >= _resources.Count
                ? throw new ResourceNotFoundException("id less then 0 or greater then resources count")
                : _resources.ElementAt(id).Value;
        }

        public IResource GetResource(string path)
        {
            return !_resources.ContainsKey(path)
                ? throw new ResourceNotFoundException("path not found")
                : _resources[path];
        }

        public bool TryGetResource(int id, out IResource? resource)
        {
            return (resource = id >= 0 && id < _resources.Count ? _resources.ElementAt(id).Value : null) is not null;
        }

        public bool TryGetResource(string pathInResources, out IResource? resource)
        {
            return _resources.TryGetValue(pathInResources, out resource);
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

        public void AddResource(string path)
        {
            if (path.EndsWith(".shader"))
            {
                //AssetDatabase.ImportAsset();
            }
            else if (path.EndsWith(".png"))
            {
                _resources.Add(path, new Sprite(path, ResourcesCount));
            }
        }
    }
}