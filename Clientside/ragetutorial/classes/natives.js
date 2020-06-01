/*
    Author : https://rage.mp/profile/32455-flashrex/
*/

let natives = {};

natives.SET_BLIP_SPRITE = (blip, sprite) => mp.game.invoke("0xDF735600A4696DAF", blip, sprite);
natives.SET_BLIP_ALPHA = (blip, alpha) => mp.game.invoke("0x45FF974EEE1C8734", blip, alpha);
natives.SET_BLIP_COLOUR = (blip, color) => mp.game.invoke("0x03D7FB09E75D6B7E", blip, color);
natives.GET_BLIP_COLOUR = (blip) => mp.game.invoke("0xDF729E8D20CF7327", blip);
natives.SHOW_HEADING_INDICATOR_ON_BLIP = (blip, status) => mp.game.invoke("0x5FBCA48327B914DF", blip, status);
natives.SET_BLIP_CATEGORY = (blip, category) => mp.game.invoke("0x234CDD44D996FD9A", blip, category);
natives.SET_BLIP_ROTATION = (blip, rotation) => mp.game.invoke("0xF87683CDF73C3F6E", blip, rotation);
natives.ADD_BLIP_FOR_ENTITY = (entityHandle) => mp.game.invoke("0x5CDE92C702A8FCE7", entityHandle);
natives.SET_THIS_SCRIPT_CAN_REMOVE_BLIPS_CREATED_BY_ANY_SCRIPT = (toggle) => mp.game.invoke("0xB98236CAAECEF897", toggle);
natives.GET_FIRST_BLIP_INFO_ID = (i) => mp.game.invoke("0x1BEDE233E6CD2A1F", i);
natives.GET_NEXT_BLIP_INFO_ID = (i) => mp.game.invoke("0x14F96AA50D6FBEA7", i);
natives.DOES_BLIP_EXIST = (blip) => mp.game.invoke("0xA6DB27D19ECBB7DA", blip);
natives.CLEAR_FOCUS = () => mp.game.invoke("0x31B73D1EA9F01DA2");

exports = natives;
