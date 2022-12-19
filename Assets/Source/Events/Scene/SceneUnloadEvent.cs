namespace Events.Scene
{

    public class SceneUnloadEvent : SceneEvent
    {
        public SceneUnloadEvent(global::GameEngine.Scenes.Scene scene)
        {
            Scene = scene;
        }

        public override global::GameEngine.Scenes.Scene Scene { get; }
    }
}