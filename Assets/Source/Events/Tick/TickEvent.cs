using Utils.Events;

namespace Events.Tick
{

    public class TickEvent : Event
    {
        public float DeltaTime { get; }
        public TickEvent(float deltaTime)
        {
            DeltaTime = deltaTime;
        }
    }
}