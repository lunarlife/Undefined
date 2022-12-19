using Events.Networking.NetPlayerEvents;
using Networking;

namespace Events.Networking.PlayerEvents
{
    public class PlayerConnectedEvent : PlayerEvent
    {
        public override NetPlayer Player { get; }
        
        public PlayerConnectedEvent(NetPlayer player)
        {
            Player = player;
        }

    }
}