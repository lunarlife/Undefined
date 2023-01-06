using GameEngine.GameObjects.Core;

namespace Events.Object
{
    public class ObjectChangedActiveEvent : ObjectEvent
    {
        public ObjectChangedActiveEvent(ObjectCore o)
        {
            Object = o;
        }

        public override ObjectCore Object { get; }
    }
}