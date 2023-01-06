using Utils.AsyncOperations;

namespace GameEngine.Scenes
{
    public abstract class SceneLoader
    {
        public abstract string SceneName { get; }

        public abstract void OnLoading(AsyncOperationInfo<SceneLoadingState> info);
    }
}