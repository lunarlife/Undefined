using GameEngine.GameObjects.Core;

namespace Events.Object
{

    public class ObjectInstanceEvent : ObjectEvent
    {
        public override ObjectCore Object { get; }

        public ObjectInstanceEvent(ObjectCore o)
        {
            Object = o;
        }
    }
}