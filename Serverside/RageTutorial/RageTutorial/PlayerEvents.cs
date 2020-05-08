using GTANetworkAPI;

namespace RageTutorial {
    class PlayerEvents : Script{

        [ServerEvent(Event.PlayerConnected)]
        public void OnPlayerConnected(Player player) {

            //Notification an Spieler senden
            player.SendNotification("Willkommen auf dem ~b~Server~w~.");

            //Nachricht an Spieler senden
            player.SendChatMessage("Willkommen auf dem Server.");

            //auf Player-Objekt zugreifen und Variable in einen String einfügen
            NAPI.Util.ConsoleOutput($"[Server] Der Spieler {player.Name} hat den Server betreten.");
        }

        [ServerEvent(Event.PlayerDisconnected)]
        public void OnPlayerDisconnected(Player player, DisconnectionType type, string reason) {
            if (!IPlayer.IsPlayerLoggedIn(player)) return;

            IPlayer iplayer = player.GetData<IPlayer>("PlayerData");
            iplayer.Disconnect();
        }

        [ServerEvent(Event.PlayerSpawn)]
        public void OnPlayerSpawn(Player player) {

            //Spieler eine Waffe geben
            player.GiveWeapon(WeaponHash.Carbinerifle_mk2, 500);

            //HP und Armor des Spielers überschreiben
            player.Health = 50;
            player.Armor = 60;

            player.Position = new Vector3(-418.94867, 1147.3202, 325.8597);
            player.Rotation = new Vector3(0, 0, 164.09552);
        }
    }
}
