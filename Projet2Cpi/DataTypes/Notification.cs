using System;

namespace Projet2Cpi
{
    public class Notification
    {
        private String notif_type;
        public String Notif_type
        {
            get { return this.notif_type; }
            set { this.notif_type = value; }
        }
        private String num_tel_ou_email;
        public String Num_tel_ou_email
        {
            get { return this.num_tel_ou_email; }
            set { this.num_tel_ou_email = value; }
        }
        private DateTime date_notif;
        public DateTime Date_notif
        {
            get { return this.date_notif; }
            set { this.date_notif = value; }
        }
        private DateTime date_event;
        public DateTime Date_event
        {
            get { return this.date_event; }
            set { this.date_event = value; }
        }
        private String event_type;
        public String Event_type
        {
            get { return this.event_type; }
            set { this.event_type = value; }
        }
        private String message;
        public String Message
        {
            get { return this.message; }
            set { this.message = value; }
        }
        private string username;
        public string Username
        {
            get { return Username; }
            set { username = value; }
        }

        public String toString()
        {
            return $"{{\"notif_type\": \"{notif_type}\", \"addr\": \"{num_tel_ou_email}\", \"date_notif\": \"{date_notif.ToString("yyyy-MM-ddTHH:mm:ss")}\", \"date_event\": \"{date_event.ToString("yyyy-MM-ddTHH:mm:ss")}\", \"event_type\": \"{event_type}\", \"message\": \"{message}\", \"username\":\"{username}\"}}";
        }
    }
}