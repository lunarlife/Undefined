using Networking;
using Utils.Events;

namespace Events.Networking.NetPlayerEvents
{
    public abstract class PlayerEvent : Event
    {
        public abstract NetPlayer Player { get; }
    }
}