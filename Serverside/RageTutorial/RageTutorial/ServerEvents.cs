using GTANetworkAPI;

namespace RageTutorial {
    public class ServerEvents : Script{

        /// <summary>
        /// Dieses Event wird aufgerufen sobald der Server startet.
        /// </summary>
        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart() {

            if(Settings.LoadServerSettings()) {
                //Nachricht in die Konsole schreiben
                NAPI.Util.ConsoleOutput($"[Server] {Settings.Server_Settings.GetServerName()} wurde erfolgreich mit der Version {Settings.Server_Settings.GetVersion()} gestartet.");
            }

            
        }
    }
}
