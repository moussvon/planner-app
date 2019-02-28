'use strict';

var http = require('http');
var urlencode = require('urlencode');
const schedule = require('node-schedule');

var username = 'thered555official@yahoo.com';
// The hash key could be found under Help->All Documentation->Your hash key. Alternatively you can use your Textlocal password in plain text.
var hash = '74bff5068677a46f522042131f02cc1595d7bfcb2b1252565cdd79fd6aae8f2e'; 
var sender = 'Plan%20it';

module.exports = function(notif) {

  var j = schedule.scheduleJob(notif.date_notif, function(){
    console.log("it is time for work");

    var date_event = new Date(notif.date_event);
    var gander = (notif.event_type === 'Tache')?'e ':' ';
    var msg = urlencode('Rappele d\'un' + gander + notif.event_type + ':  ' + notif.message + ' pour le ' + date_event.toLocaleDateString() +' à partir de ' + date_event.toLocaleTimeString());

    console.info(notif.addr.substr(1));

    var data = 'username=' + username + 
      '&hash=' + hash + 
      '&sender=' + sender + 
      '&numbers=+213' + notif.addr.substr(1) + 
      '&message=' + msg;

    console.info(data);

    var options = {
      host: 'api.txtlocal.com', path: '/send?' + data
    };

    http.request(options, function (response) {
      var str = '';//another chunk of data has been recieved, so append it to `str`
      response.on('data', function (chunk) {
        str += chunk;
      });//the whole response has been recieved, so we just print it out here
      response.on('end', function () {
        console.log(str);
      });
    }).end();
  });
};
