using GTANetworkAPI;

namespace RageTutorial {
    public class ServerEvents : Script{

        [ServerEvent(Event.ResourceStart)]
        public void OnResourceStart() {

            //Nachricht in die Konsole schreiben
            NAPI.Util.ConsoleOutput("[Server] Der Server wurde erfolgreich gestartet.");
        }
    }
}
