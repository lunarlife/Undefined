namespace Events.Scene
{

    public class SceneLoadedEvent : SceneEvent
    {
        public SceneLoadedEvent(global::GameEngine.Scenes.Scene scene)
        {
            Scene = scene;
        }

        public override global::GameEngine.Scenes.Scene Scene { get; }
    }
}