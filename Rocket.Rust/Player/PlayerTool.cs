using System;
using System.Linq;

namespace Rocket.Rust.Player
{
    public class PlayerNotFoundException : Exception
    {
        public readonly string Name;
        public readonly ulong ID;

        public PlayerNotFoundException(ulong id) : base($"Could not find a player with the id: \"{id}\".") { ID = id; }
        public PlayerNotFoundException(string name) : base($"Could not find a player with the name: \"{name}\".") { Name = name; }
    }

    public static class PlayerTool
    {
        public static RustPlayer GetPlayerByID(ulong id)
        {
            BasePlayer basePlayer = 
                BasePlayer.activePlayerList.FirstOrDefault(x => x.userID == id) ?? 
                BasePlayer.sleepingPlayerList.FirstOrDefault(x => x.userID == id);

            if (basePlayer == null)
            {
                throw new PlayerNotFoundException(id);
            }

            return new RustPlayer(basePlayer);
        }

        public static RustPlayer GetPlayerByID(string id)
        {
            if (ulong.TryParse(id, out ulong steamId))
            {
                return GetPlayerByID(steamId);
            }

            throw new ArgumentException("Please enter a valid CSteamdID.", "id");
        }

        public static RustPlayer GetPlayerByName(string name)
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

        public static bool TryGetPlayerByID(ulong id, out RustPlayer output)
        {
            BasePlayer basePlayer = BasePlayer.activePlayerList.Where(x => x.userID == id).FirstOrDefault() ?? BasePlayer.sleepingPlayerList.Where(x => x.userID == id).FirstOrDefault();

            if (basePlayer == null)
            {
                output = null;
                return false;
            }

            output = new RustPlayer(basePlayer);
            return true;
        }

        public static bool TryGetPlayerByID(string id, out RustPlayer output)
        {
            if (ulong.TryParse(id, out ulong steamId))
            {
                return TryGetPlayerByID(steamId, out output);
            }

            output = null;
            return false;
        }

        public static bool TryGetPlayerByName(string name, out RustPlayer output)
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
