//https://wiki.rage.mp/index.php?title=HUD_Components
const HUD_CASH = 3;

//Bestimmt ob Geld angezeigt wird oder nicht
let cashHudActive = false;

//Zur Sicherheit einmal das Geld oben rechts resetten
mp.game.stats.statSetInt(mp.game.joaat("SP0_TOTAL_CASH"), 0, false);

//Hiermit kann SharedData abgerufen werden
//mp.players.local.getVariable("PlayerData.Shared.Cash");

//Registriere Events
mp.events.add({

    //Wird aufgerufen wenn sich das Geld verändert
    "Hud.UpdateCash": (cash) => {

        //Update Geld
        mp.game.stats.statSetInt(mp.game.joaat("SP0_TOTAL_CASH"), cash, false);

        //Zeige Geld für 6 Sekunden an falls es normalerweise deaktiviert ist
        if(!cashHudActive) {
            cashHudActive = true;
            setTimeout(() => {
                cashHudActive = false;
            }, 6000);
        }
    },

    //Wird aufgerufen um das Geld anzuzeigen/zu verstecken
    "Hud.ToggleCash": (toggle) => {
        cashHudActive = toggle;
    },

    //Wird bei jedem Frame automatisch aufgerufen
    "render": () => {
        //Wenn CashHudActive true zeige Cash an
        if(cashHudActive) mp.game.ui.showHudComponentThisFrame(HUD_CASH);
    }
})

//Wird automatisch aufgerufen sobald die angegebene Data verändert
mp.events.addDataHandler("PlayerData.Shared.Cash", (entity, value, oldvalue) => {
    //wenn Daten beim lokalen Spieler verändert wurden update das Geld oben rechts
    if(entity.id === mp.players.local.id) mp.events.call("Hud.UpdateCash", parseInt(value));
})