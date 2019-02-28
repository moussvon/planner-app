'use strict';

const mailSender = require('./mail-sender');
const smsSender = require('./sms-sender')

module.exports = function (json) {
    var data = JSON.parse(json).map(function(value) { return JSON.parse(value); } );
    data.forEach(element => {
    if (element.notif_type === 'sms') smsSender(element);
    else if (element.notif_type === 'email') mailSender(element);
    });
};
