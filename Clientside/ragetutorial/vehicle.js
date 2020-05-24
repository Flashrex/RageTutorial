
//Variable in der die Zeit der letzten Interaktion in ms seid 01.01.1970 00:00:00 gespeichert wird
let lastInteraction = 0;

//Überprüft ob der Spieler einen Cooldown hat
function HasCooldown() { return lastInteraction + 3000 > Date.now(); }

//Sorgt dafür, dass Motor nicht automatisch angeht beim reinsetzen
mp.game.vehicle.defaultEngineBehaviour = false;

//Fixed den "shake" Bug im Auto
mp.players.local.setConfigFlag(429, true);

//Ein Keybind wird registriert
//Keycodes: https://docs.microsoft.com/de-de/windows/win32/inputdev/virtual-key-codes?redirectedfrom=MSDN 
mp.keys.bind(0x4D,true, function() { //M-Taste

    //Check ob Spieler in einem Auto ist
    if(mp.players.local.vehicle) {

        //Check ob der Motor läuft
        if(!mp.players.local.vehicle.getIsEngineRunning()) {

            //Schalte Motor an
            mp.players.local.vehicle.setEngineOn(true, false, false);
        } else {

            //Schalte Motor ab
            mp.players.local.vehicle.setEngineOn(false, false, false);
        }
    }
});

//Registriere das Event "Player.Freeze"
mp.events.add('Player.Freeze', (toggle) => {

    //Freeze/Unfreeze den Spieler
    mp.players.local.freezePosition(toggle);

    //Sende Spieler eine Nachricht
    mp.gui.chat.push('Du wurdest ' +(toggle ? 'gefreezed' : 'unfreezed'));
});

//Ein Keybind wird registriert
mp.keys.bind(0x4E, true, function() {

    //Checke ob Spieler Cooldown hat
    if(HasCooldown()) return;

    //Setze letzte Interaktion auf aktuelle Zeit
    lastInteraction = Date.now();

    //Rufe RemoteEvent "chat" auf
    mp.events.callRemote('chat', Date.now());
})