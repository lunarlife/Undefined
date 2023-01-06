using Events.Networking.NetPlayerEvents;
using Networking;

namespace Events.Networking.PlayerEvents
{
    public class PlayerConnectedEvent : PlayerEvent
    {
        public PlayerConnectedEvent(NetPlayer player)
        {
            Player = player;
        }

        public override NetPlayer Player { get; }
    }
}