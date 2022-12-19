using Utils.AsyncOperations;

namespace GameEngine.Scenes
{
    public abstract class SceneLoader
    {
        public abstract string SceneName { get; }
        public SceneLoader()
        {
            
        }

        public abstract void OnLoading(AsyncOperationInfo<SceneLoadingState> info);
    }
}