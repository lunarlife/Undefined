using GameEngine.Resources;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Resources;

namespace GameEngine
{
    public class ServerManager : ServerManagerBase
    {
        public ServerManager()
        {
            ResourcesManager = new ServerResourcesManager();
            InternalResourcesManager = new InternalResourcesManager();
        }

        public override IResourcesManager ResourcesManager { get; }
        public InternalResourcesManager InternalResourcesManager { get; }
    }
}