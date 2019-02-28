using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet2Cpi
{
    [Serializable]
    public class Tache : CalendarObj
    {
        /// <summary>
        /// Cette classe représente les tâches
        /// 
        ///L'attribut PRIVE _etat représente un entier qui peut prendre les valeurs 0, 1 ou 2. le 0 signifie que la tâche
        /// non réalisée, le 1 que la tâche est en cours de réalisD:\brouillons-programmes\C#\WpfApp1\WpfApp1\DataTypes\Tache.csation et le 2 que la tâche est réalisée. 
        ///L'attribut PUBLIC etat représente un string prenant 3 valeurs possibles : "realise", "en cours" et
        ///"non realise", ce sont des setters et getters pour _priorité ("realise" equivaut à 2, "en cours" equivaut à 1 et 
        ///"non realise" equivaut à 0)
        /// 
        /// Pour l'attribut PRIVE _priorite, et l'attribut PUBLIC priorite, suivre la même logique que _etat et etat. Sauf
        /// que dans ce cas 0 equivaut à "elevee", 1 equivaut à "moyenne" et 2 equivaut à "faible"
        /// 
        /// </summary>
        private int _id;//Identification de la tâche
        public int id
        {
            get { return _id; }
            set { _id = value; }
        }

        private string _title;
        public string title
        {
            get { return _title; }
            set { _title = value; }
        }

        private string _details;//Designation de la tâche
        public string Details
        {
            get { return _details; }
            set { _details = value; }
        }

        private string _etat;//attribut _etat, voir documentation ci dessus
        public string etat
        {
            get { return _etat; }
            set { _etat = value; }
        }

        private string _priorite;//attribut _priorie, voir ci dessus également, même logique que pour _etat
        public string priorite
        {
            get { return _priorite; }
            set { _priorite = value; }
        }

        private DateTime _dateDebut;
        public DateTime dateDebut
        {
            get
            {
                return _dateDebut;
            }
            set
            {
                _dateDebut = value;
            }
        }

        private DateTime _dateFin;
        public DateTime dateFin
        {
            get
            {
                return _dateFin;
            }
            set
            {
                _dateFin = value;
            }
        }

        private List<String> fichiers = new List<string>();
        public List<String> Fichiers
        {
            set { this.fichiers = value; }
            get { return this.fichiers; }
        }
        // Activité
        private String activitee;
        public String Activitee
        {
            set { this.activitee = value; }
            get { return activitee; }
        }
        private List<Notif> alarms;
        public List<Notif> Alarms
        {
            set { this.alarms = value; }
            get { return this.alarms; }
        }

        public Tache()
        {

        }

    }
}
