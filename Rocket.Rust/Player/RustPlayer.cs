using System;
using System.Linq;

using Rocket.API.Player;

namespace Rocket.Rust.Player
{
    public class RustPlayer : IPlayer, IEquatable<ulong>, IComparable<ulong>, IEquatable<BasePlayer>, IComparable<BasePlayer>
    {
        #region IPlayer Implementation
        public string UniqueID => CSteamID.ToString();
        public string DisplayName => Player.displayName;
        public string IsAdmin => "";
        //public bool IsAdmin => Player.IsAdmin;
        #endregion
            
        public ulong CSteamID => Player.userID;
        public readonly BasePlayer Player;

        internal RustPlayer(BasePlayer player)
        {
            Player = player;
        }

        #region Object Implementation
        public override int GetHashCode()
        {
            return BitConverter.ToInt32(BitConverter.GetBytes(CSteamID), 4);
        }

        public override bool Equals(object obj)
        {
            Type type = obj.GetType();

            if (type == typeof(IPlayer))
            {
                return Equals((IPlayer)obj);
            }
            else if (type == typeof(ulong))
            {
                return Equals((ulong)obj);
            }
            else if (type == typeof(string))
            {
                return Equals((string)obj);
            }
            else if (type == typeof(BasePlayer))
            {
                return Equals((BasePlayer)obj);
            }

            throw new ArgumentException($"Cannot equate the type \"{typeof(RustPlayer).Name}\" to \"{type.Name}\".");
        }
        #endregion

        #region IEquatable and IComparable Implementation
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
            return UniqueID.CompareTo(other);
        }

        public bool Equals(ulong other)
        {
            return CSteamID.Equals(other);
        }

        public bool Equals(string other)
        {
            return UniqueID.Equals(other);
        }

        public bool Equals(BasePlayer other)
        {
            return CSteamID.Equals(other.userID);
        }

        public int CompareTo(BasePlayer other)
        {
            return UniqueID.CompareTo(other.userID.ToString());
        }
#endregion
    }
}
