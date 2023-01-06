using GameEngine.GameObjects.Core;

namespace Events.Object
{
    public class ObjectInstanceEvent : ObjectEvent
    {
        public ObjectInstanceEvent(ObjectCore o)
        {
            Object = o;
        }

        public override ObjectCore Object { get; }
    }
}