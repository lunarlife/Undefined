namespace Events.Scene
{

    public class SceneLoadedEvent : SceneEvent
    {
        public SceneLoadedEvent(global::GameEngine.Scenes.OldScene scene)
        {
            Scene = scene;
        }

        public override global::GameEngine.Scenes.OldScene Scene { get; }
    }
}