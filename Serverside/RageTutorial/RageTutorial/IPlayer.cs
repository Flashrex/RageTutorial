using GTANetworkAPI;
using RageTutorial.Database;

namespace RageTutorial {
    class IPlayer : Script {

        //NAPI.Player
        public Player Player { get; set; }

        //Name des Spielers
        public string Name { get; set; }

        //Level des Spielers
        public int Level { get; set; }

        //Adminlevel des Spielers
        public int AdminLevel { get; set; }

        //Geld (Bargeld) des Spielers
        public long Cash { get; set; }

        //Es muss immer ein leerer Konstruktor vorhanden sein
        public IPlayer() {}

        //Konstruktor
        public IPlayer(string _name, Player _player) {
            //Werte bei neuer Registration

            //Setze Spielernamen
            Name = _name;

            //Setze NAPI.Player
            Player = _player;

            //Setze Level
            Level = 1;

            //Setze Bargeld
            Cash = 500;
        }

        /// <summary>
        /// Registriert den Spieler in der Datenbank
        /// </summary>
        /// <param name="_name"></param>
        /// <param name="_password"></param>
        public void Register(string _name, string _password) {

            //Setze Name
            Name = _name;

            //Registriere Spieler
            PlayerData.RegisterPlayer(this, _password);

            //Logge Spieler ein
            Login(true);
        }


        /// <summary>
        /// Loggt den Spieler ein.
        /// </summary>
        /// <param name="firstLogin"></param>
        public void Login(bool firstLogin) {

            //Setze Nametag des Spielers auf seinen Namen
            Player.Name = Name;

            //Checke ob es der erste Login ist, wenn nicht lade Spieler aus der Datenbank
            if(!firstLogin) PlayerData.LoadPlayer(this);

            //Setze Spielerobjekt an Spieler
            Player.SetData("PlayerData", this);

            //Sende Nachricht
            Player.SendChatMessage("Willkommen auf dem Server.");
        }

        /// <summary>
        /// Disconnecte den Spieler.
        /// </summary>
        public void Disconnect() {

            //Spieler speichern.
            Save();
        }

        /// <summary>
        /// Speichere den Spieler.
        /// </summary>
        public void Save() {

            //Message auf Konsole
            NAPI.Util.ConsoleOutput($"[Server] Spieler {Name} wurde gespeichert.");

            //Update den Spieler in der Datenbank
            PlayerData.UpdatePlayer(this);
        }

        /// <summary>
        /// Checkt ob Spieler eingeloggt ist.
        /// </summary>
        /// <param name="player"></param>
        /// <returns></returns>
        public static bool IsPlayerLoggedIn(Player player) {

            //Returnt true,wenn Spieler die Daten mit dem Key "PlayerData" hat
            // (werden beim Login gesetzt)
            return player.HasData("PlayerData");
        }

        /// <summary>
        /// Checkt ob Spieler gleiches oder höheres Adminlevel wie übergebenes level hat
        /// </summary>
        /// <param name="level"></param>
        /// <returns></returns>
        public bool HasAdminLevel(int level) {

            //Return true, wenn Adminlevel höher oder gleich level
            return AdminLevel >= level;
        }
    }
}
