using GTANetworkAPI;
using MySql;
using MySql.Data.MySqlClient;
using RageTutorial.Imported;
using System;

namespace RageTutorial.Database {
    class PlayerData : Script {

        // Register, Speichern/Updaten, Laden, CheckName, CheckPassword

        public static void RegisterPlayer(IPlayer iplayer, string password) {

            string saltedPassword = PasswordDerivation.Derive(password);

            using(MySqlConnection connection = new MySqlConnection("SERVER=localhost; DATABASE=ragetutorial; UID=ragetutorial; PASSWORD=1234;")) {
                try {
                    connection.Open();
                    MySqlCommand command = connection.CreateCommand();

                    command.CommandText = "INSERT INTO (name, password, level, adminlevel, cash) VALUES (@name, @password, @level, @adminlevel, @cash)";

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


    }
}
