using Utils.Events;

namespace Events.Tick
{
    public class SyncTickEventData : EventData
    {
        public SyncTickEventData(float deltaTime)
        {
            DeltaTime = deltaTime;
        }

        public float DeltaTime { get; }
    }
}