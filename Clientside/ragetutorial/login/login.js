const Browser = require('./ragetutorial/classes/browser');
const Camera = require('./ragetutorial/classes/camera');

let loginBrowser = new Browser('loginBrowser', 'package://ragetutorial/web/login/login.html');
let loginCam = new Camera('loginCam', new mp.Vector3(24.48141, -255.5137, 141.269), new mp.Vector3(-51.10479, -680.2582, 140.7251));
loginCam.startMoving(50.0);


setTimeout(() => {
    mp.gui.cursor.show(true, true);
    mp.gui.chat.show(false);
}, 250);

mp.events.add({
    "Login.Register": (name, password) => {
        mp.gui.cursor.show(false, false);
        mp.gui.chat.show(true);

        mp.events.callRemote('Login.OnRegister', name, password);

        if(loginBrowser !== null) {
            loginBrowser.close();
            loginBrowser = null;
        }
    },

    "Login.Submit": (name, password) => {
        mp.events.callRemote('Login.OnLogin', name, password);
    },

    "Login.ToggleRegister": () => {
        if(loginBrowser !== null) loginBrowser.callFunction("activateRegisterInput");
    },

    "Login.UpdateErrors": (errors) => {
        if(loginBrowser !== null) loginBrowser.callFunction("toggleWarningText", errors);
    },

    "Login.Success": () => {
        mp.gui.cursor.show(false, false);
        mp.gui.chat.show(true);

        if(loginBrowser !== null) {
            loginBrowser.close();
            loginBrowser = null;
        }
    }
})