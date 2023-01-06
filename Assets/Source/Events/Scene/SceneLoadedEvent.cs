using GameEngine.Scenes;

namespace Events.Scene
{
    public class SceneLoadedEvent : SceneEvent
    {
        public SceneLoadedEvent(OldScene scene)
        {
            Scene = scene;
        }

        public override OldScene Scene { get; }
    }
}