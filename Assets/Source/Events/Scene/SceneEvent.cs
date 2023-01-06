using GameEngine.Scenes;
using Utils.Events;

namespace Events.Scene
{
    public abstract class SceneEvent : Event
    {
        public abstract OldScene Scene { get; }
    }
}