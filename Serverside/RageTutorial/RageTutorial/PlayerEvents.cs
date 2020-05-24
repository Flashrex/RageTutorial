using GTANetworkAPI;

namespace RageTutorial {
    class PlayerEvents : Script{

        /// <summary>
        /// Wird aufgerufen wenn ein Spieler auf den Server connectet
        /// </summary>
        /// <param name="player"></param>
        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnected(Player player) {

            //Notification an Spieler senden
            player.SendNotification("Willkommen auf dem ~b~Server~w~.");

            //Nachricht an Spieler senden
            player.SendChatMessage("Willkommen auf dem Server.");

            //auf Player-Objekt zugreifen und Variable in einen String einfügen
            NAPI.Util.ConsoleOutput($"[Server] Der Spieler {player.Name} hat den Server betreten.");
        }

        /// <summary>
        /// Wird aufgerufen wenn ein Spieler den Server verlässt
        /// </summary>
        /// <param name="player"></param>
        /// <param name="type"></param>
        /// <param name="reason"></param>
        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Player player, DisconnectionType type, string reason) {

            //Checke ob Spieler eingeloggt ist
            if (!IPlayer.IsPlayerLoggedIn(player)) return;

            //Lade Spielerobjekt von Spieler
            IPlayer iplayer = player.GetData<IPlayer>("PlayerData");

            //Disconnecte den Spieler
            iplayer.Disconnect();
        }

        /// <summary>
        /// Wird aufgerufen wenn Spieler spawnt
        /// </summary>
        /// <param name="player"></param>
        [ServerEvent(Event.PlayerSpawn)]
        public void OnPlayerSpawn(Player player) {

            //Spieler eine Waffe geben
            player.GiveWeapon(WeaponHash.Carbinerifle_mk2, 500);

            //HP und Armor des Spielers überschreiben
            player.Health = 50;
            player.Armor = 60;

            //Setze Position und Rotation des Spielers
            player.Position = new Vector3(-418.94867, 1147.3202, 325.8597);
            player.Rotation = new Vector3(0, 0, 164.09552);
        }

        /// <summary>
        /// Remote Event "chat" wird registriert
        /// </summary>
        /// <param name="player"></param>
        /// <param name="time"></param>
        [RemoteEvent("chat")]
        public void OnChat(Player player, string time) {

            //Sende Spieler Nachricht mit dem vom Client übergebenen string
            player.SendChatMessage(time);
        }
    }
}
