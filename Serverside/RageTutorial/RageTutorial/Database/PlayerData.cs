using GTANetworkAPI;
using MySql.Data.MySqlClient;
using RageTutorial.Imported;
using System;

namespace RageTutorial.Database {
    class PlayerData : Script{

        /// <summary>
        /// Funktion mit der wir einen Spieler in der Datenbank anlegen.
        /// </summary>
        /// <param name="iplayer"></param>
        /// <param name="password"></param>
        public static void RegisterPlayer(IPlayer iplayer, string password) {

            //Derive verschlüsselt das übergebene Passwort.
            string saltedPassword = PasswordDerivation.Derive(password);

            //Hier wird eine MySql-Verbindung aufgebaut.
            using(MySqlConnection connection = new MySqlConnection(Settings.Server_Settings.GetConnectionString())) {
                try {
                    //Hier wird die Verbindung geöffnet
                    connection.Open();

                    //Wir erstellen einen Befehl, der später ausgeführt werden soll
                    MySqlCommand command = connection.CreateCommand();

                    //Wir füllen den Befehl mit einem Querystring mit der Anweisung INSERT INTO
                    command.CommandText = "INSERT INTO users (name, password, level, adminlevel, cash) VALUES (@name, @password, @level, @adminlevel, @cash)";

                    //Wir fügen unsere gewünschten Variablen hinzu, welche in die Tabelle eingefügt werden sollen
                    command.Parameters.AddWithValue("@name", iplayer.Name);
                    command.Parameters.AddWithValue("@password", saltedPassword);
                    command.Parameters.AddWithValue("@level", iplayer.Level);
                    command.Parameters.AddWithValue("@adminlevel", iplayer.AdminLevel);
                    command.Parameters.AddWithValue("@cash", iplayer.Cash);

                    //Query wird ausgeführt
                    command.ExecuteNonQuery();

                    //Verbindung wird geschlossen
                    connection.Close();


                } catch(Exception e) {

                    //Bei Fehler geben wir Fehlermessage und Stacktrace auf die Konsole aus
                    NAPI.Util.ConsoleOutput($"[Exception] RegisterAccount: {e.Message}");
                    NAPI.Util.ConsoleOutput($"[Exception] RegisterAccount: {e.StackTrace}");
                }
            }

        }

        /// <summary>
        /// Mit dieser Funktion wird ein Spieler aus der Datenbank geladen
        /// </summary>
        /// <param name="iplayer"></param>
        public static void LoadPlayer(IPlayer iplayer) {

            //Hier wird eine MySql-Verbindung aufgebaut.
            using (MySqlConnection connection = new MySqlConnection(Settings.Server_Settings.GetConnectionString())) {

                //Hier wird die Verbindung geöffnet
                connection.Open();

                //Wir erstellen einen Befehl, der später ausgeführt werden soll
                MySqlCommand command = connection.CreateCommand();

                //Wir füllen den Befehl mit einem Querystring mit der Anweisung SELECT
                //* steht für everything oder alles
                command.CommandText = "SELECT * FROM users WHERE name=@name LIMIT 1";

                //Wir fügen unsere gewünschten Variablen hinzu, welche in die Tabelle eingefügt werden sollen
                command.Parameters.AddWithValue("@name", iplayer.Name);

                //Es wird ein Reader ausgeführt, der die Zeilen aus der Tabelle ausliest
                using(MySqlDataReader reader = command.ExecuteReader()) {

                    //Reader hat Zeilen gefunden
                    if (reader.HasRows) {

                        //Reader soll lesen
                        reader.Read();

                        //Variablen werden gelesen und in unserem Spieler-Objekt gespeichert.
                        iplayer.Level = reader.GetInt16("level");
                        iplayer.AdminLevel = reader.GetInt16("adminlevel");
                        iplayer.Cash = reader.GetInt32("cash");
                    }
                }

                //Verbindung wird geschlossen
                connection.Close();
            }

        }

        /// <summary>
        /// Mit dieser Funktion wird ein Spieler in der Datenbank geupdated
        /// </summary>
        /// <param name="iplayer"></param>
        public static void UpdatePlayer(IPlayer iplayer) {

            //Hier wird eine MySql-Verbindung aufgebaut.
            using (MySqlConnection connection = new MySqlConnection(Settings.Server_Settings.GetConnectionString())) {

                //Hier wird die Verbindung geöffnet
                connection.Open();

                //Wir erstellen einen Befehl, der später ausgeführt werden soll
                MySqlCommand command = connection.CreateCommand();

                //Wir füllen den Befehl mit einem Querystring mit der Anweisung UPDATE
                command.CommandText = "UPDATE users SET level=@level, adminlevel=@adminlevel, cash=@cash WHERE name=@name";

                //Wir fügen unsere gewünschten Variablen hinzu, welche in der Tabelle geupdatet werden sollen
                command.Parameters.AddWithValue("@level", iplayer.Level);
                command.Parameters.AddWithValue("@adminlevel", iplayer.AdminLevel);
                command.Parameters.AddWithValue("@cash", iplayer.Cash);
                command.Parameters.AddWithValue("@name", iplayer.Name);

                //Query wird ausgeführt
                command.ExecuteNonQuery();

                //Verbindung wird geschlossen
                connection.Close();
            }
        }

        /// <summary>
        /// Mit dieser Funktion wird überprüft ob das übergeben Passwort (input) mit dem aus der Datenbank übereinstimmt.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool CheckPassword(string name, string input) {

            //Variable um Passwort aus der Datenbank zwischenzuspeichern
            string password = "";

            //Hier wird eine MySql-Verbindung aufgebaut.
            using (MySqlConnection connection = new MySqlConnection(Settings.Server_Settings.GetConnectionString())) {

                //Hier wird die Verbindung geöffnet
                connection.Open();

                //Wir erstellen einen Befehl, der später ausgeführt werden soll
                MySqlCommand command = connection.CreateCommand();

                //Wir füllen den Befehl mit einem Querystring mit der Anweisung SELECT
                command.CommandText = "SELECT password FROM users WHERE name=@name LIMIT 1";

                //Wir fügen unsere gewünschten Variablen hinzu, welche in der Tabelle ausgelesen werden sollen
                command.Parameters.AddWithValue("@name", name);

                //Es wird ein Reader ausgeführt, der die Zeilen aus der Tabelle ausliest
                using (MySqlDataReader reader = command.ExecuteReader()) {

                    //Reader hat Zeilen gefunden
                    if (reader.HasRows) {

                        //Reader soll lesen
                        reader.Read();

                        //Password wird ausgelesen und im oben angelegten string gespeichert
                        password = reader.GetString("password");
                    }
                }

                //Verbindung wird geschlossen
                connection.Close();
            }

            //Verify überprüft ob das verschlüsselte Passwort (password) mit dem unverschlüsselten Input (input) übereinstimmt
            if(PasswordDerivation.Verify(password, input)) {

                //Return true, wenn das Passwort stimmt
                return true;
            }

            //Return false, wenn das Passwort nicht übereinstimmt
            return false;
        }

        /// <summary>
        /// Diese Funktion überprüft ob der übergebene Name bereits in der Datenbank existiert.
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public static bool DoesPlayerNameExists(string name) {

            //Hier wird eine MySql-Verbindung aufgebaut.
            using (MySqlConnection connection = new MySqlConnection(Settings.Server_Settings.GetConnectionString())) {

                //Hier wird die Verbindung geöffnet
                connection.Open();

                //Wir erstellen einen Befehl, der später ausgeführt werden soll
                MySqlCommand command = connection.CreateCommand();

                //Wir füllen den Befehl mit einem Querystring mit der Anweisung SELECT
                command.CommandText = "SELECT * FROM users WHERE name=@name LIMIT 1";

                //Wir fügen unsere gewünschten Variablen hinzu, welche in der Tabelle ausgelesen werden sollen
                command.Parameters.AddWithValue("@name", name);

                //Es wird ein Reader ausgeführt, der die Zeilen aus der Tabelle ausliest
                using (MySqlDataReader reader = command.ExecuteReader()) {

                    //Reader hat Zeilen gefunden
                    if (reader.HasRows) {

                        //Verbindung wird geschlossen
                        connection.Close();

                        //Return true, wenn der Name gefunden wurde
                        return true;
                    }
                }

                //Verbindung wird geschlossen
                connection.Close();
            }

            //Return false, falls kein Name gefunden wurde
            return false;
        }
    }
}
