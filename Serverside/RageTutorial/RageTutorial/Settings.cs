using GTANetworkAPI;
using System;
using System.IO;

namespace RageTutorial {
    class Settings : Script {

        //Öffentliches Objekt auf welches wir von überall zugreifen können
        public static Settings Server_Settings;

        //Name des Servers
        public string Servername { get; set; }

        //Version des Servers
        public string Version { get; set; }

        //Hostname (Adresse) der Datenbank
        public string Hostname { get; set; }

        //Username in der Datenbank
        public string Username { get; set; }

        //Passwort des Users
        public string Password { get; set; }

        //Datenbank auf die zugegriffen wird
        public string Database { get; set; }

        //Es muss immer ein leerer Konstruktor vorhanden sein
        public Settings() { }

        /// <summary>
        /// Returnt den Servernamen
        /// </summary>
        /// <returns></returns>
        public string GetServerName() {
            return Servername;
        }

        /// <summary>
        /// Returnt die Version
        /// </summary>
        /// <returns></returns>
        public string GetVersion() {
            return Version;
        }

        /// <summary>
        /// Erstellt den Connectionstring und returnt ihn
        /// </summary>
        /// <returns></returns>
        public string GetConnectionString() {
            return $"SERVER={Hostname}; DATABASE={Database}; UID={Username}; PASSWORD={Password}";
        }

        /// <summary>
        /// Lädt die Serversettings
        /// </summary>
        /// <returns></returns>
        public static bool LoadServerSettings() {

            //Überprüfe ob Datei existiert
            if(!File.Exists(@"./serverdata/settings.json")) {

                //Gebe Nachricht auf Konsole aus
                NAPI.Util.ConsoleOutput("[Fehler] Settings konnten nicht geladen werden.");
                NAPI.Util.ConsoleOutput("[Fehler] Server wird heruntergefahren...");

                //Führe Task nach 3000ms aus
                NAPI.Task.Run(() => {
                    
                    //Beendet den Server
                    Environment.Exit(0);
                }, delayTime: 3000);

                //Returnt false da Settings nicht geladen werden konnten.
                return false;

            //Falls Datei existiert
            } else {

                //Gebe Nachricht auf Konsole aus
                NAPI.Util.ConsoleOutput("[Server] Settings wurden erfolgreich geladen.");

                //Lese Datei aus
                string result = File.ReadAllText(@"./serverdata/settings.json");

                //Erstelle Settings Objekt aus dem Json-String
                Server_Settings = NAPI.Util.FromJson<Settings>(result);

                //Return true da Settings erfolgreich geladen
                return true;
            }
        }
    }
}
