using GameEngine.Scenes;
using Utils.AsyncOperations;

namespace Scenes
{

    public class GameScene : SceneLoader
    {
        public override string SceneName => "Game";

        public override void OnLoading(AsyncOperationInfo<SceneLoadingState> info)
        {
            InitObjects();

        }
        /*public World? World;
        private ClientChat _chat;*/

        private void InitObjects()
        {
            /*_ = new CameraControl(Camera.Current!);
            _ = new DebugInfo();
            if (World is not null) throw new Exception("World is loaded");
            World = new World(); //TODO SERVER LOAD;
            _chat = new ClientChat();*/
        }
    }
}