using Events.Networking.NetPlayerEvents;
using Networking;
using UndefinedNetworking;

namespace Events.Networking.PlayerEvents
{
    public class PlayerDisconnectEvent : PlayerEvent
    {
        public override NetPlayer Player { get; }
        public DisconnectCause Cause { get; }
        public string Message { get; }

        public PlayerDisconnectEvent(NetPlayer player, DisconnectCause cause, string message)
        {
            Player = player;
            Cause = cause;
            Message = message;
        }
    }
}