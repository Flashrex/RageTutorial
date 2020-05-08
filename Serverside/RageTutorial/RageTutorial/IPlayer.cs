using GTANetworkAPI;
using RageTutorial.Database;

namespace RageTutorial {
    class IPlayer : Script {

        public Player Player { get; set; }
        public string Name { get; set; }
        public int Level { get; set; }
        public int AdminLevel { get; set; }
        public long Cash { get; set; }

        public IPlayer() {
            
        }

        public IPlayer(string _name, Player _player) {
            //Werte bei neuer Registration
            Name = _name;
            Player = _player;
            Level = 1;
            Cash = 500;
        }

        public void Register(string _name, string _password) {
            Name = _name;
            PlayerData.RegisterPlayer(this, _password);

            Login(true);
        }

        public void Login(bool firstLogin) {
            Player.Name = Name;

            if(!firstLogin) PlayerData.LoadPlayer(this);

            Player.SetData("PlayerData", this);
            Player.SendChatMessage("Willkommen auf dem Server.");
        }

        public void Disconnect() {
            Save();
        }

        public void Save() {
            NAPI.Util.ConsoleOutput($"[Server] Spieler {Name} wurde gespeichert.");
            PlayerData.UpdatePlayer(this);
        }

        public static bool IsPlayerLoggedIn(Player player) {
            return player.HasData("PlayerData");
        }

        public bool HasAdminLevel(int level) {
            return AdminLevel >= level;
        }
    }
}
