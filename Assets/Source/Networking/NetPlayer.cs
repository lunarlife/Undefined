using System;
using GameEngine;

namespace Networking
{
    public class NetPlayer : IEquatable<NetPlayer>
    {


        public Identifier Identifier { get; }
        public string Nickname { get; }
        public bool IsMine => this == Undefined.MyPlayer;

        //public ClientColony Colony { get; private set; }

        public NetPlayer(Identifier identifier, string nickname)
        {
            Identifier = identifier;
            Nickname = nickname;
        }

        //public void CreateColony(ColonyInitializePacket cip) => Colony = new ClientColony(cip.Position);

        public static bool operator ==(NetPlayer left, NetPlayer right) => left?.Identifier == right?.Identifier;

        public static bool operator !=(NetPlayer left, NetPlayer right) => !(left == right);
        public bool Equals(NetPlayer other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return Equals(Identifier, other.Identifier);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((NetPlayer)obj);
        }
        public override int GetHashCode()
        {
            return (Identifier != null ? Identifier.GetHashCode() : 0);
        }
    }
}