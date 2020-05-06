using GTANetworkAPI;

namespace RageTutorial {
    class IPlayer : Script {

        public string Name { get; set; }
        public int Level { get; set; }
        public int AdminLevel { get; set; }
        public long Cash { get; set; }

        public IPlayer() {
            //Werte bei neuer Registration
            Level = 1;
            Cash = 500;
        }

        public static bool IsPlayerLoggedIn(Player player) {
            return player.HasData("PlayerData");
        }

        public bool HasAdminLevel(int level) {
            return AdminLevel >= level;
        }
    }
}
