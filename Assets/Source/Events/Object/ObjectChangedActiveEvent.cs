using GameEngine.GameObjects.Core;

namespace Events.Object
{
    public class ObjectChangedActiveEvent : ObjectEvent
    {
        public override ObjectCore Object { get; }

        public ObjectChangedActiveEvent(ObjectCore o)
        {
            Object = o;
        }
    }
}