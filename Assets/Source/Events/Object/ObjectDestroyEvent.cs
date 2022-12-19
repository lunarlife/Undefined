using GameEngine.GameObjects.Core;

namespace Events.Object
{

    public class ObjectDestroyEvent : ObjectEvent
    {
        public ObjectDestroyEvent(ObjectCore o)
        {
            Object = o;
        }

        public override ObjectCore Object { get; }
    }
}