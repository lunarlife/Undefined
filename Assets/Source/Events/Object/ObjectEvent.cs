using GameEngine.GameObjects.Core;
using Utils.Events;

namespace Events.Object
{
    public abstract class ObjectEvent : EventData
    {
        public abstract ObjectCore Object { get; }
    }
}