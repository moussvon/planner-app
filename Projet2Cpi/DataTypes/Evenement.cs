using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet2Cpi
{
    [Serializable]
    public class Evenement : CalendarObj
    {
        /// <summary>
        /// Cette classe désigne les événements
        /// 
        /// </summary>

        private List<String> fichiers = new List<string>();
        public List<String> Fichiers
        {
            set { this.fichiers = value; }
            get { return this.fichiers; }
        }

        private int _id;// identifiant de l'événement
        public int id
        {
            get
            {
                return _id;
            }
            set
            {
                _id = value;
            }
        }
        private string title;
        public string Title
        {
            set { this.title = value; }
            get { return this.title; }
        }
        private string priority;
        public string Priority
        {
            get { return this.priority; }
            set { this.priority = value; }
        }
        private string description;
        public string Description
        {
            set { this.description = value; }
            get { return this.description; }
        }
        private DateTime dateDebut;
        public DateTime DateDebut
        {
            set { this.dateDebut = value; }
            get { return this.dateDebut; }
        }

        private DateTime dateFin;
        public DateTime DateFin
        {
            set { this.dateFin = value; }
            get { return this.dateFin; }
        }

        private string _place; //string représentant le lieu de l'événement
        public string place
        {
            get
            {
                return _place;
            }
            set
            {
                _place = value;
            }
        }


        private string _activite;
        public string activite
        {
            get
            {
                return _activite;
            }
            set
            {
                _activite = value;
            }
        }
        private List<ContactData> _listOfContacts;
        public List<ContactData> listOfContacts
        {
            get { return _listOfContacts; }
            set { _listOfContacts = value; }
        }

        private List<Notif> alarms;
        public List<Notif> Alarms
        {
            set { this.alarms = value; }
            get { return this.alarms; }
        }

        //Les constructeurs possibles pour cette classe
        public Evenement(int id = 0,string designation = "Aucune désignation",string place = "Aucun endroit spécifié",DateTime date = new DateTime(),List<ContactData> listOfContacts = null)
        {
            _id = id;
            //_designation = designation;
            _place = place;
            //_date = date;
            _listOfContacts = listOfContacts;
        }

        public Evenement()
        {
            this.priority = "Urgente";
            this.title = "";
            this._place = "";
            this.description = "";
        }
    }
}
