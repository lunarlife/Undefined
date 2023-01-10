using Utils.Events;

namespace Events.Tick
{
    public class AsyncFixedTickEventData : EventData
    {
        public AsyncFixedTickEventData(float delta)
        {
            DeltaTime = delta;
        }

        public float DeltaTime { get; }
    }
}