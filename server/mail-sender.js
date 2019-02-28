'use strict';

const fs = require('fs');
const nodemailer = require('nodemailer');
const schedule = require('node-schedule');

var content = ['<td valign="top" class="textContent"><h3 style="color:#5F5F5F;line-height:125%;font-family:Helvetica,Arial,sans-serif;font-size:20px;font-weight:normal;margin-top:0;margin-bottom:3px;text-align:left;">Salut ',
'</h3><div style="text-align:left;font-family:Helvetica,Arial,sans-serif;font-size:15px;margin-bottom:0;margin-top:3px;color:#5F5F5F;line-height:135%;">	    je te rappelle que tu as prévu un',
' le ',
' à partir de ',
'.<br/>  voici quelques informations à propos de ton ',
' : ',
'<br/>   Je compte sur toi pour réaliser ce que tu as à  faire !<br/></div></td>']
var htmlTemp;
fs.readFile('./tem1.html', 'UTF-8', function(err, html) {
    if (err) {
        console.log(err);
    }

    var mytemp = html.split(/{{{.*?}}}/);
    htmlTemp = function(name, typeActivity, workTime, message) {
        let gender = (typeActivity === 'tach')?'e ':' ';
        let work = new Date(workTime);
        return mytemp[0] + content[0] + name + content[1] + gender + typeActivity + content[2] + work.toLocaleDateString() +
            content[3] + work.toLocaleTimeString() + content[4] + typeActivity + content[5] + message + content[6] + mytemp[1];
    }
});

var transporter = nodemailer.createTransport({
    service: 'gmail',
    port: 465,
    secure: true,
    auth: {
      user: 'IPlannerInc@gmail.com',
      pass: 'azerty21' // pasword for sms is Adem Moss Red Ned
    }
});

module.exports = function(notif) {

    var j = schedule.scheduleJob(notif.date_notif, function(){
        console.log("it is time for work");
        
            // setup email data with unicode symbols
            let mailOptions = {
                from: '"Plan it" <Planit@Plan.com>', // sender address
                to: notif.addr, // list of receivers
                subject: '[Plan it] ' + notif.message + ' ✔', // Subject line
                html: htmlTemp("User", notif.event_type, notif.date_event, notif.message),// html body
            };
        
            // send mail with defined transport object
            transporter.sendMail(mailOptions, (error, info) => {
                if (error) {
                    return console.log(error);
                }
                console.log('Message sent: %s', info.messageId);
                // Preview only available when sending through an Ethereal account
                //console.log('Preview URL: %s', nodemailer.getTestMessageUrl(info));
            });
        });

    };

