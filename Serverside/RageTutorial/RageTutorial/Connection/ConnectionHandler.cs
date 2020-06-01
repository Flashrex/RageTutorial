using GTANetworkAPI;
using RageTutorial.Database;
using System.Text.RegularExpressions;

namespace RageTutorial.Connection {
    class ConnectionHandler : Script{

        [RemoteEvent("Login.OnLogin")]
        public void OnLogin(Player player, string name, string password) {

            //Checke ob der Spieler bereits eingeloggt ist
            if (IPlayer.IsPlayerLoggedIn(player)) {
                //Sende Spieler Nachricht und beende Funktion
                player.SendNotification("Du bist bereits eingeloggt!");
                return;
            }

            //Erzeuge Regex-Objekt mit gegebener Anweisung
            Regex regex = new Regex(@"([a-zA-Z]+)_([a-zA-Z]+)");

            //Wenn die Eingabe nicht unseren Vorgaben enspricht (also kein Vorname_Nachname)
            if (!regex.IsMatch(name)) {

                //Sende Spieler Nachricht und beende Funktion
                player.SendNotification("Der Name muss dem Format Vorname_Nachname entsprechen!");
                return;
            }

            //Checke ob Spieler bereits in der Datenbank existiert
            if (!PlayerData.DoesPlayerNameExists(name)) {

                NAPI.ClientEvent.TriggerClientEvent(player, "Login.ToggleRegister");
                return;
            }

            //Überprüfe ob das Passwort stimmt
            if (!PlayerData.CheckPassword(name, password)) {

                int errors = 0;
                if (player.HasData("PlayerData.LoginErrors")) {
                    errors = player.GetData<int>("PlayerData.LoginErrors");
                }

                if(errors >= 3) {
                    player.Kick();
                    return;
                }

                NAPI.ClientEvent.TriggerClientEvent(player, "Login.UpdateErrors", $"Falsches Password! ({errors+1}/3)");
                player.SetData("PlayerData.LoginErrors", errors + 1);
                return;
            }

            //Erzeuge Spielerobjekt
            IPlayer iplayer = new IPlayer(name, player);

            //Logge den Spieler ein
            iplayer.Login(false);
            NAPI.ClientEvent.TriggerClientEvent(player, "Login.Success");

            NAPI.Player.SpawnPlayer(player, new Vector3());
        }

        [RemoteEvent("Login.OnRegister")]
        public void OnRegister(Player player, string name, string password) {

            //Erzeuge ein Regex-Objekt mit dem gegebenen Argument
            //Wikipedia Regex: Ein regulärer Ausdruck ist in der theoretischen Informatik eine Zeichenkette, die der Beschreibung von Mengen von Zeichenketten mit Hilfe bestimmter syntaktischer Regeln dient. 
            Regex regex = new Regex(@"([a-zA-Z]+)_([a-zA-Z]+)");

            //Wenn die Eingabe nicht unseren Vorgaben enspricht (also kein Vorname_Nachname)
            if (!regex.IsMatch(name)) {

                //Sende Spieler Nachricht und beende Funktion
                player.SendNotification("Der Name muss dem Format Vorname_Nachname entsprechen!");
                return;
            }

            //Checke ob Spieler bereits in der Datenbank existiert
            if (PlayerData.DoesPlayerNameExists(name)) {

                player.Kick();
                return;
            }

            //Erzeuge Spielerobjekt
            IPlayer iplayer = new IPlayer(name, player);

            //Registriere den Spieler in der Datenbank
            iplayer.Register(name, password);

            NAPI.Player.SpawnPlayer(player, new Vector3());
        }
    }
}
