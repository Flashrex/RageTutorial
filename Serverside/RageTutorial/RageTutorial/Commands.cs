using GTANetworkAPI;
using RageTutorial.Database;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace RageTutorial {
    class Commands : Script {

        //Dieses Enum enthält alle Adminränge
        public enum AdminRanks { Supporter = 1, Moderator, Admin, Serverleitung};

        //Ein Command wird registriert
        [Command("veh", Alias = "vehicle")]
        public void CMD_Veh(Player player, string vehName) {

            //Es wird überprüft ob der Spieler eingeloggt ist, wenn nicht wird die Funktion abgebrochen
            if (!IPlayer.IsPlayerLoggedIn(player)) return;

            //Spielerklasse wird vom Spieler geladen
            IPlayer iplayer = player.GetData<IPlayer>("PlayerData");

            //Überprüfung ob Spieler das nötige Adminlevel hat
            if(!iplayer.HasAdminLevel((int)AdminRanks.Admin)) {

                //Wenn nicht sende Spieler eine Nachricht und beende Funktion
                player.SendChatMessage("Du hast nicht genug Rechte.");
                return;
            }


            //Versuche mit dem eingegebenen Fahrzeugnamen das passende Fahrzeug zu finden
            VehicleHash vehHash = NAPI.Util.VehicleNameToModel(vehName);

            //Wenn kein Fahrzeug gefunden,
            if(vehHash == 0) {

                //sende Spieler Nachricht und beende Funktion
                player.SendChatMessage("Ungültiger Fahrzeugname.");
                return;
            }

            //Überprüfe ob Spieler bereits ein Fahrzeug hat
            if(player.HasData("PlayerData.Temp.Adminveh")) {

                //Wenn ja lade altes Fahrzeug vom Spieler und lösche es
                Vehicle old_veh = player.GetData<Vehicle>("PlayerData.Temp.Adminveh");
                old_veh.Delete();
            }

            //Erstelle ein Fahrzeug an der Position des Spielers mit dem erzeugten Hash und der Farbe1 sowie Farbe2 -> 5 
            Vehicle veh = NAPI.Vehicle.CreateVehicle(vehHash, player.Position, player.Rotation, 5, 5);

            //Setze Spieler in Fahrzeug (derzeit leider verbuggt)
            player.SetIntoVehicle(veh, 0);

            //Zeige Spieler Benachrichtigung
            player.SendNotification("Fahrzeug gespawnt.");

            //Speichere erstelltes Fahrzeug am Spieler
            player.SetData("PlayerData.Temp.Adminveh", veh);
        }

        //Ein Command wird registriert
        [Command("getpos", "Nutze: /getpos [Name] um deine aktuelle Position zu speichern.")]
        public void CMD_GetPos(Player player, string name) {

            //Lege verschiedene Variablen an
            Vector3 position; Vector3 rotation; string line;

            //Checke ob Spieler in einem Fahrzeug ist
            if(player.IsInVehicle) {
                //In Fahrzeug

                //Speichere Spielers position sowie rotation
                position = player.Vehicle.Position;
                rotation = player.Vehicle.Rotation;

                //erzeuge mit Hilfe der Position/Rotation einen String
                //CultureInfo("en-US") sorgt dafür,dass das englische Komma (.) genutzt wird
                line = $"VehiclePos | {name}: {position.X.ToString(new CultureInfo("en-US"))}, {position.Y.ToString(new CultureInfo("en-US"))}, {position.Z.ToString(new CultureInfo("en-US"))}, {rotation.Z.ToString(new CultureInfo("en-US"))}";

            } else {
                //Zu Fuß

                //Speichere Spielers position sowie rotation
                position = player.Position;
                rotation = player.Rotation;

                //erzeuge mit Hilfe der Position/Rotation einen String
                //CultureInfo("en-US") sorgt dafür,dass das englische Komma (.) genutzt wird
                line = $"OnFootPos | {name}: {position.X.ToString(new CultureInfo("en-US"))}, {position.Y.ToString(new CultureInfo("en-US"))}, {position.Z.ToString(new CultureInfo("en-US"))}, {rotation.Z.ToString(new CultureInfo("en-US"))}";
            }

            //Sende erzeugten String an Spieler im Chat
            player.SendChatMessage(line);

            //Schreibe string in eine Datei
            using (StreamWriter file = new StreamWriter(@".\serverdata\positions.txt", true)) {
                file.WriteLine(line);
            }
        }


        //Ein Command wird registriert
        [Command("register", "~y~Nutze: ~w~/register [Name] [Passwort] um dich zu registrieren.")]
        public void CMD_Register(Player player, string name, string password) {

            //Erzeuge ein Regex-Objekt mit dem gegebenen Argument
            //Wikipedia Regex: Ein regulärer Ausdruck ist in der theoretischen Informatik eine Zeichenkette, die der Beschreibung von Mengen von Zeichenketten mit Hilfe bestimmter syntaktischer Regeln dient. 
            Regex regex = new Regex(@"([a-zA-Z]+)_([a-zA-Z]+)");
            
            //Wenn die Eingabe nicht unseren Vorgaben enspricht (also kein Vorname_Nachname)
            if(!regex.IsMatch(name)) {

                //Sende Spieler Nachricht und beende Funktion
                player.SendChatMessage("Der Name muss dem Format Vorname_Nachname entsprechen!");
                return;
            }

            //Checke ob Spieler bereits in der Datenbank existiert
            if (PlayerData.DoesPlayerNameExists(name)) {

                //Sende Spieler Nachricht und beende Funktion
                player.SendChatMessage("Dieser Name ist bereits bei uns registriert!");
                player.SendChatMessage("Nutze /login um dich zu einzuloggen.");
                return;
            }

            //Erzeuge Spielerobjekt
            IPlayer iplayer = new IPlayer(name, player);

            //Registriere den Spieler in der Datenbank
            iplayer.Register(name, password);
        }

        //Ein Command wird registriert
        [Command("login", "~y~Nutze: ~w~/login [Name] [Passwort] um dich einzuloggen.")]
        public void CMD_Login(Player player, string name, string password) {

            //Checke ob der Spieler bereits eingeloggt ist
            if(IPlayer.IsPlayerLoggedIn(player)) {
                //Sende Spieler Nachricht und beende Funktion
                player.SendChatMessage("Du bist bereits eingeloggt!");
                return;
            }

            //Erzeuge Regex-Objekt mit gegebener Anweisung
            Regex regex = new Regex(@"([a-zA-Z]+)_([a-zA-Z]+)");

            //Wenn die Eingabe nicht unseren Vorgaben enspricht (also kein Vorname_Nachname)
            if (!regex.IsMatch(name)) {

                //Sende Spieler Nachricht und beende Funktion
                player.SendChatMessage("Der Name muss dem Format Vorname_Nachname entsprechen!");
                return;
            }

            //Checke ob Spieler bereits in der Datenbank existiert
            if (!PlayerData.DoesPlayerNameExists(name)) {

                //Sende Spieler Nachricht und beende Funktion
                player.SendChatMessage("Name wurde nicht gefunden!");
                player.SendChatMessage("Nutze /register um dich zu registrieren.");
                return;
            }

            //Überprüfe ob das Passwort stimmt
            if (!PlayerData.CheckPassword(name, password)) {
                player.SendChatMessage("Das Passwort stimmt nicht!");
                return;
            }

            //Erzeuge Spielerobjekt
            IPlayer iplayer = new IPlayer(name, player);

            //Logge den Spieler ein
            iplayer.Login(false);
        }

        //Ein Command wird registriert
        [Command("stats")]
        public void CMD_Stats(Player player) {

            //Checke ob Spieler eingeloggt ist, wenn nicht beende Funktion
            if (!IPlayer.IsPlayerLoggedIn(player)) return;

            //Lade Spielerobjekt vom Spieler
            IPlayer iplayer = player.GetData<IPlayer>("PlayerData");

            //Sende Spieler Nachricht mit all seinen Daten
            player.SendChatMessage($"Name: ~b~{iplayer.Name}~w~, Level: ~b~{iplayer.Level}~w~, Adminlevel: ~b~{iplayer.AdminLevel}~w~, Cash: ~b~{iplayer.Cash}~w~");
        }

        //Ein Command wird registriert
        [Command("freeze")]
        public void CMD_Freeze(Player player) {
            //Checke ob Spieler eingeloggt ist, wenn nicht beende Funktion
            if (!IPlayer.IsPlayerLoggedIn(player)) return;

            //Lade Spielerobjekt vom Spieler
            IPlayer iplayer = player.GetData<IPlayer>("PlayerData");

            //Rufe Clientside Event "Player.Freeze" beim Spieler auf
            NAPI.ClientEvent.TriggerClientEvent(player, "Player.Freeze", !iplayer.Freezed);

            //Setze Freezed auf das Gegenteil
            iplayer.Freezed = !iplayer.Freezed;
        }

        [Command("givemoney")]
        public void CMD_GiveMoney(Player player, int cash) {
            if (!IPlayer.IsPlayerLoggedIn(player)) return;
            IPlayer iplayer = player.GetData<IPlayer>("PlayerData");

            iplayer.AddCash(cash);
        }
    }
}
