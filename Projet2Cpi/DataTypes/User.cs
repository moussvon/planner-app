using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet2Cpi
{
    [Serializable]
    public class User
    {
        /// <summary>
        /// Cette classe désigne un utilisateur de l'application, elle contient
        /// les informations demandées dans le cahier des charges
        /// 
        /// Les identifiants de cette classe varient entre 100000 et 199999
        /// </summary>
        private int _id;// Identifiant de l'utilisateur
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        private Dictionary<DateTime, string> joursFeries = new Dictionary<DateTime, string>();
        public Dictionary<DateTime, string> JoursFeries
        {
            set { this.joursFeries = value; }
            get { return this.joursFeries; }
        }

        private string _userName; //Nom de l'utilisateur
        public string userName
        {
            get { return _userName; }
            set { _userName = value; }
        }
        
        private string _userFirstName = "";//Prenom de l'utilisateur
        public string userFirstName
        {
            get { return _userFirstName; }
            set { _userFirstName = value; }
        }

        private string _userSecondName;
        public string userSecondName
        {
            get { return this._userSecondName; }
            set { this._userSecondName = value; }
        }

        private string _phoneNumber;//numero de telephone
        public string phoneNumber
        {
            get { return _phoneNumber; }
            set { _phoneNumber = value; }
        }

        private string _pathOfPicture; // chemin de l'image de profil
        public string pathOfPicture
        {
            get { return _pathOfPicture; }
            set { _pathOfPicture = value; }
        }

        private string _userMail; //Adresse email de l'utilisateur
        public string userMail
        {
            get { return _userMail; }
            set { _userMail = value; }
        }

        private string _nameOfSchool; //Nom de l'établissement scholaire
        public string nameOfSchool
        {
            get { return _nameOfSchool; }
            set { _nameOfSchool = value; }
        }

        private int theme;
        public int Theme
        {
            set { theme = value; }
            get { return theme; }
        }

        private Dictionary<String, String> activities = new Dictionary<string, string>();
        public Dictionary<String, String> Activities
        {
            set { this.activities = value; }
            get { return this.activities; }
        }

        private int eventsPriority;
        public int EventsPriority
        {
            set { eventsPriority = value; }
            get { return eventsPriority; }
        }

        private ArrayList weekends;
        public ArrayList Weekends
        {
            set { weekends = value; }
            get { return weekends; }
        }

        private Dictionary<DayOfWeek, Dictionary<int, Seance>> planning = new Dictionary<DayOfWeek, Dictionary<int, Seance>>();
        public Dictionary<DayOfWeek, Dictionary<int, Seance>> Planning
        {
            set { this.planning = value; }
            get { return this.planning; }
        }

        private Tuple<DateTime?, DateTime?> planningPeriod1 = null;
        public Tuple<DateTime?, DateTime?> PlanningPeriod1
        {
            get { return this.planningPeriod1; }
            set { this.planningPeriod1 = value; }
        }

        private Tuple<DateTime?, DateTime?> planningPeriod2 = null;
        public Tuple<DateTime?, DateTime?> PlanningPeriod2
        {
            get { return this.planningPeriod2; }
            set { this.planningPeriod2 = value; }
        }

        private Tuple<DateTime?, DateTime?> controlesInt = null;
        public Tuple<DateTime?, DateTime?> ControlesInt
        {
            set { this.controlesInt = value; }
            get { return this.controlesInt; }
        }

        private Tuple<DateTime?, DateTime?> examensFinals;
        public Tuple<DateTime?, DateTime?> ExmensFinals
        {
            set { this.examensFinals = value; }
            get { return this.examensFinals; }
        }

    }
}
