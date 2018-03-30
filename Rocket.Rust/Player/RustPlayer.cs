using System;

using Rocket.API.Player;

namespace Rocket.Rust.Player
{
    public class RustPlayer : IPlayer, IEquatable<ulong>, IComparable<ulong>
    {
        public string UniqueID => CSteamID.ToString();
        public ulong CSteamID { get; private set; }

        public string DisplayName => throw new NotImplementedException();

        public string IsAdmin => throw new NotImplementedException();

        public int CompareTo(IPlayer other)
        {
            return UniqueID.CompareTo(other.UniqueID);
        }

        public bool Equals(IPlayer other)
        {
            return UniqueID.Equals(other.UniqueID);
        }

        public int CompareTo(ulong other)
        {
            return CSteamID.ToString().CompareTo(other.ToString());
        }

        public int CompareTo(string other)
        {
            return UniqueID.CompareTo(UniqueID);
        }

        public bool Equals(ulong other)
        {
            return CSteamID.Equals(other);
        }

        public bool Equals(string other)
        {
            return UniqueID.Equals(other);
        }
    }
}
