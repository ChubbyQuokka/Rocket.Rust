using System;
using System.Collections.Generic;
using System.Linq;

using Rocket.API.Player;

namespace Rocket.Rust.Player
{
    public class PlayerNotFoundException : Exception
    {
        public readonly string Name;
        public readonly ulong ID;

        public PlayerNotFoundException(ulong id) : base($"Could not find a player with the id: \"{id}\".") { ID = id; }
        public PlayerNotFoundException(string name) : base($"Could not find a player with the name: \"{name}\".") { Name = name; }
    }

    public class RustPlayerManager : IPlayerManager
    {
        public IEnumerable<IPlayer> Players
        {
            get
            {
                List<IPlayer> players = new List<IPlayer>();

                foreach (BasePlayer player in BasePlayer.activePlayerList)
                {
                    players.Add(new RustPlayer(player));
                }

                foreach (BasePlayer player in BasePlayer.sleepingPlayerList)
                {
                    players.Add(new RustPlayer(player));
                }

                return players;
            }
        }

        public bool Ban(IPlayer player, string reason, TimeSpan? timeSpan = null)
        {
            throw new NotImplementedException();
        }

        public bool Kick(IPlayer player, string reason)
        {
            if (player is RustPlayer rustPlayer)
            {
                if (!rustPlayer.Player.IsSleeping())
                {
                    rustPlayer.Player.Kick(reason);
                    return true;
                }
            }

            return false;
        }

        public IPlayer GetPlayer(string uniqueID)
        {
            if (ulong.TryParse(uniqueID, out ulong steamId))
            {
                return GetPlayer(steamId);
            }
            else
            {
                throw new ArgumentException("Please enter a valid id.", "uniqueID");
            }
        }

        IPlayer GetPlayer(ulong uniqueID)
        {
            BasePlayer basePlayer = 
                BasePlayer.activePlayerList.FirstOrDefault(x => x.userID == uniqueID) ?? 
                BasePlayer.sleepingPlayerList.FirstOrDefault(x => x.userID == uniqueID);

            if (basePlayer == null)
            {
                throw new PlayerNotFoundException(uniqueID);
            }

            return new RustPlayer(basePlayer);
        }

        bool TryGetPlayer(ulong uniqueID, out IPlayer output)
        {
            BasePlayer basePlayer = 
                BasePlayer.activePlayerList.Where(x => x.userID == uniqueID).FirstOrDefault() ?? 
                BasePlayer.sleepingPlayerList.Where(x => x.userID == uniqueID).FirstOrDefault();

            if (basePlayer == null)
            {
                output = null;
                return false;
            }

            output = new RustPlayer(basePlayer);
            return true;
        }

        public bool TryGetPlayer(string uniqueID, out IPlayer output)
        {
            if (ulong.TryParse(uniqueID, out ulong steamId))
            {
                return TryGetPlayer(steamId, out output);
            }

            output = null;
            return false;
        }

        public IPlayer GetPlayerByName(string name)
        {
            BasePlayer basePlayer =
                BasePlayer.activePlayerList.FirstOrDefault(x => (x.displayName ?? string.Empty).Equals(name, StringComparison.InvariantCultureIgnoreCase)) ??
                BasePlayer.activePlayerList.FirstOrDefault(x => (x.displayName ?? string.Empty).Contains(name)) ??
                BasePlayer.sleepingPlayerList.FirstOrDefault(x => (x.displayName ?? string.Empty).Equals(name, StringComparison.InvariantCultureIgnoreCase)) ??
                BasePlayer.sleepingPlayerList.FirstOrDefault(x => (x.displayName ?? string.Empty).Contains(name));

            if (basePlayer == null)
            {
                throw new PlayerNotFoundException(name);
            }

            return new RustPlayer(basePlayer);
        }

        public bool TryGetPlayerByName(string name, out IPlayer output)
        {
            BasePlayer basePlayer =
                BasePlayer.activePlayerList.FirstOrDefault(x => (x.displayName ?? string.Empty).Equals(name, StringComparison.InvariantCultureIgnoreCase)) ??
                BasePlayer.activePlayerList.FirstOrDefault(x => (x.displayName ?? string.Empty).Contains(name)) ??
                BasePlayer.sleepingPlayerList.FirstOrDefault(x => (x.displayName ?? string.Empty).Equals(name, StringComparison.InvariantCultureIgnoreCase)) ??
                BasePlayer.sleepingPlayerList.FirstOrDefault(x => (x.displayName ?? string.Empty).Contains(name));

            if (basePlayer == null)
            {
                output = null;
                return false;
            }

            output = new RustPlayer(basePlayer);
            return true;
        }
    }
}
