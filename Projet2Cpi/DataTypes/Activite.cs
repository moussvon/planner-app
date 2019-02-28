using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet2Cpi
{
    class Activite
    {
        /// <summary>
        /// Cette classe représente une activité quelconque, dans l'application on en préparera certaines par
        /// défaut mais l'utilisateur a le droit d'en créer de nouvelles.
        /// Le nombre d'activités créables est limité par l'attribut static (maxNumberOfActivities)
        /// </summary>

        private string _designation; // désignation de l'activité
        public string designation
        {
            get { return _designation; }
            set { _designation = value; }
        }

        private string _typeOfActivity;// type de l'activité
        public string typeOfActivity
        {
            get { return _typeOfActivity; }
            set { _typeOfActivity = value; }
        }

        private List<Tache> _listOfTaches; // représente la liste des tâches.
        public List<Tache> listOfTaches
        {
            get { return _listOfTaches; }
            set { _listOfTaches = value; }
        }

        private List<Evenement> _listOfEvents; // représente la liste des tâches.
        public List<Evenement> listOfEvents
        {
            get { return _listOfEvents; }
            set { _listOfEvents = value; }
        }

        //Différents constructeurs pour cette classe

        public Activite(string designation="Aucune designation", string typeOfActivity="Aucun type",List<Tache> listOfTaches= null, List<Evenement> listOfEvents = null)
        {
            _designation = designation;
            _typeOfActivity = typeOfActivity;
            _listOfTaches = listOfTaches;
            _listOfEvents = listOfEvents;
        }        
    }
}