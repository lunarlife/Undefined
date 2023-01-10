using Networking;
using Utils.Events;

namespace Events.Networking.PlayerEvents
{
    public abstract class PlayerEvent : EventData
    {
        public abstract NetPlayer Player { get; }
    }
}