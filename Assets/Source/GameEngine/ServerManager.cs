using GameEngine.Resources;
using Networking.Loggers;
using UndefinedNetworking.GameEngine;
using UndefinedNetworking.GameEngine.Resources;
using UndefinedNetworking.GameEngine.Scenes.UI;

namespace GameEngine
{
    public class ServerManager : ServerManagerBase
    {
        public ServerManager(Logger logger) : base(logger)
        {
            ResourcesManager = new ServerResourcesManager();
            InternalResourcesManager = new InternalResourcesManager();
        }

        public override IUIView GetView(uint id) => Undefined.Player.ActiveScene.GetView(id);

        protected override ServerType _ServerType => ServerType.Client;
        public override IResourcesManager ResourcesManager { get; }
        public InternalResourcesManager InternalResourcesManager { get; }
    }
}