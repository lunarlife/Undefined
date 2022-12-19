using Utils.Events;

namespace Events.Tick
{

    public class AsyncTickEvent : Event
    {
        public float DeltaTime { get; }

        public AsyncTickEvent(float deltaTime)
        {
            DeltaTime = deltaTime;
        }

    }
}