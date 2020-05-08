using GTANetworkAPI;
using RageTutorial.Database;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

namespace RageTutorial {
    class Commands : Script {

        public enum AdminRanks { Supporter = 1, Moderator, Admin, Serverleitung};

        // /veh /vehicle
        [Command("veh", Alias = "vehicle")]
        public void CMD_Veh(Player player, string vehName) {
            if (!IPlayer.IsPlayerLoggedIn(player)) return;
            IPlayer iplayer = player.GetData<IPlayer>("PlayerData");

            if(!iplayer.HasAdminLevel((int)AdminRanks.Admin)) {
                player.SendChatMessage("Du hast nicht genug Rechte.");
                return;
            }

            // /veh Sultan
            VehicleHash vehHash = NAPI.Util.VehicleNameToModel(vehName);
            if(vehHash == 0) {
                player.SendChatMessage("Ungültiger Fahrzeugname.");
                return;
            }

            if(player.HasData("PlayerData.Temp.Adminveh")) {
                Vehicle old_veh = player.GetData<Vehicle>("PlayerData.Temp.Adminveh");
                old_veh.Delete();
            }

            Vehicle veh = NAPI.Vehicle.CreateVehicle(vehHash, player.Position, player.Rotation, 5, 5);
            player.SetIntoVehicle(veh, 0);
            player.SendNotification("Fahrzeug gespawnt.");
            player.SetData("PlayerData.Temp.Adminveh", veh);
        }

        // /getpos [Name]
        [Command("getpos", "Nutze: /getpos [Name] um deine aktuelle Position zu speichern.")]
        public void CMD_GetPos(Player player, string name) {

            Vector3 position; Vector3 rotation; string line;
            if(player.IsInVehicle) {
                //In Fahrzeug
                position = player.Vehicle.Position;
                rotation = player.Vehicle.Rotation;

                line = $"VehiclePos | {name}: {position.X.ToString(new CultureInfo("en-US"))}, {position.Y.ToString(new CultureInfo("en-US"))}, {position.Z.ToString(new CultureInfo("en-US"))}, {rotation.Z.ToString(new CultureInfo("en-US"))}";
                // VehiclePos | Name : x, y, z, r

            } else {
                //Zu Fuß
                position = player.Position;
                rotation = player.Rotation;
                line = $"OnFootPos | {name}: {position.X.ToString(new CultureInfo("en-US"))}, {position.Y.ToString(new CultureInfo("en-US"))}, {position.Z.ToString(new CultureInfo("en-US"))}, {rotation.Z.ToString(new CultureInfo("en-US"))}";
            }

            player.SendChatMessage(line);

            using (StreamWriter file = new StreamWriter(@".\serverdata\positions.txt", true)) {
                file.WriteLine(line);
            }
        }


        // /register [Name] [Passwort]
        // /login [Name] [Passwort]
        [Command("register", "~y~Nutze: ~w~/register [Name] [Passwort] um dich zu registrieren.")]
        public void CMD_Register(Player player, string name, string password) {

            Regex regex = new Regex(@"([a-zA-Z]+)_([a-zA-Z]+)");
            if(!regex.IsMatch(name)) {
                player.SendChatMessage("Der Name muss dem Format Vorname_Nachname entsprechen!");
                return;
            }

            if (PlayerData.DoesPlayerNameExists(name)) {
                player.SendChatMessage("Dieser Name ist bereits bei uns registriert!");
                player.SendChatMessage("Nutze /login um dich zu einzuloggen.");
                return;
            }

            IPlayer iplayer = new IPlayer(name, player);
            iplayer.Register(name, password);
        }

        [Command("login", "~y~Nutze: ~w~/login [Name] [Passwort] um dich einzuloggen.")]
        public void CMD_Login(Player player, string name, string password) {

            if(IPlayer.IsPlayerLoggedIn(player)) {
                player.SendChatMessage("Du bist bereits eingeloggt!");
                return;
            }

            Regex regex = new Regex(@"([a-zA-Z]+)_([a-zA-Z]+)");
            if (!regex.IsMatch(name)) {
                player.SendChatMessage("Der Name muss dem Format Vorname_Nachname entsprechen!");
                return;
            }

            if(!PlayerData.DoesPlayerNameExists(name)) {
                player.SendChatMessage("Name wurde nicht gefunden!");
                player.SendChatMessage("Nutze /register um dich zu registrieren.");
                return;
            }

            IPlayer iplayer = new IPlayer(name, player);
            iplayer.Login(false);
        }

        [Command("stats")]
        public void CMD_Stats(Player player) {
            if (!IPlayer.IsPlayerLoggedIn(player)) return;

            IPlayer iplayer = player.GetData<IPlayer>("PlayerData");
            player.SendChatMessage($"Name: ~b~{iplayer.Name}~w~, Level: ~b~{iplayer.Level}~w~, Adminlevel: ~b~{iplayer.AdminLevel}~w~, Cash: ~b~{iplayer.Cash}~w~");
        }
    }
}
