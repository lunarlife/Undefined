using Utils.Events;

namespace Events.Scene
{

    public abstract class SceneEvent : Event
    {
        public abstract global::GameEngine.Scenes.Scene Scene { get; }
    }
}