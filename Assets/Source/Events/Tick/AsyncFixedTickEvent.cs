using Utils.Events;

namespace Events.Tick
{

    public class AsyncFixedTickEvent : Event
    {
        public AsyncFixedTickEvent(float delta)
        {
            DeltaTime = delta;
        }

        public float DeltaTime { get; }
    }
}