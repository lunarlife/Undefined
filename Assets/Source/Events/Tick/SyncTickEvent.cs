using Utils.Events;

namespace Events.Tick
{
    public class SyncTickEvent : Event
    {
        public SyncTickEvent(float deltaTime)
        {
            DeltaTime = deltaTime;
        }

        public float DeltaTime { get; }
    }
}