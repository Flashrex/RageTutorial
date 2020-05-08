using GTANetworkAPI;
using MySql.Data.MySqlClient;
using RageTutorial.Imported;
using System;

namespace RageTutorial.Database {
    class PlayerData : Script{

        public static void RegisterPlayer(IPlayer iplayer, string password) {

            string saltedPassword = PasswordDerivation.Derive(password);

            using(MySqlConnection connection = new MySqlConnection("SERVER=localhost; DATABASE=ragetutorial; UID=ragetutorial; PASSWORD=1234")) {
                try {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO users (name, password, level, adminlevel, cash) VALUES (@name, @password, @level, @adminlevel, @cash)";

                    command.Parameters.AddWithValue("@name", iplayer.Name);
                    command.Parameters.AddWithValue("@password", saltedPassword);
                    command.Parameters.AddWithValue("@level", iplayer.Level);
                    command.Parameters.AddWithValue("@adminlevel", iplayer.AdminLevel);
                    command.Parameters.AddWithValue("@cash", iplayer.Cash);

                    command.ExecuteNonQuery();
                    connection.Close();


                } catch(Exception e) {
                    NAPI.Util.ConsoleOutput($"[Exception] RegisterAccount: {e.Message}");
                    NAPI.Util.ConsoleOutput($"[Exception] RegisterAccount: {e.StackTrace}");
                }
            }

        }

        public static void LoadPlayer(IPlayer iplayer) {

            using(MySqlConnection connection = new MySqlConnection("SERVER=localhost; DATABASE=ragetutorial; UID=ragetutorial; PASSWORD=1234")) {

                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM users WHERE name=@name LIMIT 1";
                command.Parameters.AddWithValue("@name", iplayer.Name);

                using(MySqlDataReader reader = command.ExecuteReader()) {
                    if (reader.HasRows) {
                        reader.Read();
                        iplayer.Level = reader.GetInt16("level");
                        iplayer.AdminLevel = reader.GetInt16("adminlevel");
                        iplayer.Cash = reader.GetInt32("cash");
                    }
                }
                connection.Close();
            }

        }

        public static void UpdatePlayer(IPlayer iplayer) {

            using (MySqlConnection connection = new MySqlConnection("SERVER=localhost; DATABASE=ragetutorial; UID=ragetutorial; PASSWORD=1234")) {

                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "UPDATE users SET level=@level, adminlevel=@adminlevel, cash=@cash WHERE name=@name";

                command.Parameters.AddWithValue("@level", iplayer.Level);
                command.Parameters.AddWithValue("@adminlevel", iplayer.AdminLevel);
                command.Parameters.AddWithValue("@cash", iplayer.Cash);
                command.Parameters.AddWithValue("@name", iplayer.Name);

                command.ExecuteNonQuery();
                connection.Close();
            }
        }

        public static bool CheckPassword(string name, string input) {
            string password = "";

            using (MySqlConnection connection = new MySqlConnection("SERVER=localhost; DATABASE=ragetutorial; UID=ragetutorial; PASSWORD=1234")) {
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT password FROM users WHERE name=@name LIMIT 1";
                command.Parameters.AddWithValue("@name", name);

                using(MySqlDataReader reader = command.ExecuteReader()) {
                    if(reader.HasRows) {
                        reader.Read();
                        password = reader.GetString("password");
                    }
                }
                connection.Close();
            }

            if(PasswordDerivation.Verify(password, input)) {
                return true;
            }
            return false;
        }

        public static bool DoesPlayerNameExists(string name) {

            using (MySqlConnection connection = new MySqlConnection("SERVER=localhost; DATABASE=ragetutorial; UID=ragetutorial; PASSWORD=1234")) {
                
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM users WHERE name=@name LIMIT 1";
                command.Parameters.AddWithValue("@name", name);

                using (MySqlDataReader reader = command.ExecuteReader()) {
                    if (reader.HasRows) {
                        connection.Close();
                        return true;
                    }
                }
                connection.Close();
            }
            return false;
        }
    }
}
