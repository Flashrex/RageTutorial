//Require Browserklasse
const Browser = require('./ragetutorial/classes/browser');

//Require Cameraklasse
const Camera = require('./ragetutorial/classes/camera');

//Erstelle Browser
let loginBrowser = new Browser('loginBrowser', 'package://ragetutorial/web/login/login.html');

//Erstelle Camera
let loginCam = new Camera('loginCam', new mp.Vector3(24.48141, -255.5137, 141.269), new mp.Vector3(-51.10479, -680.2582, 140.7251));

//Lässt die Camera von links nach rechts und zurück bewegen
loginCam.startMoving(50.0);

//Führe Code nach 250ms aus
setTimeout(() => {

    //Zeigt Cursor an
    mp.gui.cursor.show(true, true);

    //Verstecke den Chat
    mp.gui.chat.show(false);
}, 250);

//Registriere Events
mp.events.add({
    "Login.Register": (name, password) => {
        //Versteckt Cursor
        mp.gui.cursor.show(false, false);

        //Zeigt Chat an
        mp.gui.chat.show(true);

        //Rufe Event beim Server auf
        mp.events.callRemote('Login.OnRegister', name, password);

        //wenn Camera existiert lösche sie
        if(loginCam !== null) {
            loginCam.delete();
            loginCam = null;
        }

        //Wenn Browser existiert lösche ihn
        if(loginBrowser !== null) {
            loginBrowser.close();
            loginBrowser = null;
        }
    },

    "Login.Submit": (name, password) => {
        //Rufe Event beim Server auf
        mp.events.callRemote('Login.OnLogin', name, password);
    },

    "Login.ToggleRegister": () => {
        //wenn browser existiert, aktiviere Register Input
        if(loginBrowser !== null) loginBrowser.callFunction("activateRegisterInput");
    },

    "Login.UpdateErrors": (errors) => {
        //wenn browser existiert, aktiviere WarningText
        if(loginBrowser !== null) loginBrowser.callFunction("toggleWarningText", errors);
    },

    "Login.Success": () => {
        //Versteckt Cursor
        mp.gui.cursor.show(false, false);

        //Zeigt Chat an
        mp.gui.chat.show(true);

        //wenn Camera existiert lösche sie
        if(loginCam !== null) {
            loginCam.delete();
            loginCam = null;
        }

        //Wenn Browser existiert lösche ihn
        if(loginBrowser !== null) {
            loginBrowser.close();
            loginBrowser = null;
        }
    }
})