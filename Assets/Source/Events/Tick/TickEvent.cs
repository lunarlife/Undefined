using Utils.Events;

namespace Events.Tick
{
    public class TickEvent : Event
    {
        public TickEvent(float deltaTime)
        {
            DeltaTime = deltaTime;
        }

        public float DeltaTime { get; }
    }
}