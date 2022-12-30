using System.Threading.Tasks;
using Events.Scene;
using Utils.AsyncOperations;
using Utils.Events;

namespace GameEngine.Scenes
{

    public sealed class OldScene : IEventCaller
    {
        private readonly SceneLoader _loader;
        private readonly object _syncLock = new();
        private readonly object _asyncLock = new();
        public static OldScene CurrentScene { get; private set; }

        public string SceneName { get; }

        private OldScene(SceneLoader loader)
        {
            _loader = loader;
            SceneName = loader.SceneName;
        }

        private async void Load(AsyncOperationInfo<SceneLoadingState> info)
        {
            info.SetFinishCallback(op => ULogger.ShowInfo($"Scene {SceneName} loaded"));
            info.Start(new SceneLoadingState
            {
                State = $"Loading {SceneName}",
                LoadComplete = 0
            });
            await Task.Run(() =>
            {
                CurrentScene = this;
                this.RegisterListener();
                _loader.OnLoading(info);
                info.Finish();
                CurrentScene?.Unload();
                this.CallEvent(new SceneLoadedEvent(this));
            });
        }


        public void Unload()
        {
            CurrentScene = null;
            this.UnregisterEventsSafity();
            //this.CallEvent(new SceneUnloadEvent(this));
        }
        
        public static (T loader, AsyncOperationInfo<SceneLoadingState> info) LoadScene<T>() where T : SceneLoader, new()
        {
            var loader = new T();
            var scene = new OldScene(loader);
            var asyncOperation = new AsyncOperationInfo<SceneLoadingState>(20);
            scene.Load(asyncOperation);
            return (loader, asyncOperation);
        }
    }
}