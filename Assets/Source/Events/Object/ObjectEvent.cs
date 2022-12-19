using GameEngine.GameObjects.Core;
using Utils.Events;

namespace Events.Object
{

    public abstract class ObjectEvent : Event
    {
        public abstract ObjectCore Object { get; }
    }
}