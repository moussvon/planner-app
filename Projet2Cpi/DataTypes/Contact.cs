using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Project_Calendar
{
    public class Contact
    {
        /// <summary>
        /// Cette classe représente un contact
        /// 
        /// </summary>
        private int _id;//Cet attribut représente l'identifiant du contact
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

        private string _name;//Cet attribut représente le nom du contact
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value;
            }
        }

        private string _adress;//Cet attribut représente l'adresse du contact
        public string adress
        {
            get { return _adress; }
            set { _adress = value; }
        }

        private string _numOfPhone;//Cet attribut représente le numero de téléphone du contact
        public string numOfPhone
        {
            get { return _numOfPhone; }
            set { _numOfPhone = value; }
        }

        private string _emailAdress;//Cet attribut représente l'adresse email du contact
        public string emailAdress
        {
            get { return _emailAdress; }
            set { _emailAdress = value; }
        }

        private string _webSite;//Cet attribut représente le site web de l'utilisateur
        public string webSite
        {
            get { return _webSite; }
            set { _webSite = value; }
        }

        private string _fbLink;//Cet attribut représente le site web de l'utilisateur
        public string fbLink
        {
            get { return _fbLink; }
            set { _fbLink = value; }
        }

        private string _linkedInLink;//Cet attribut représente le site web de l'utilisateur
        public string linkedInLink
        {
            get { return _linkedInLink; }
            set { _linkedInLink = value; }
        }

        //Différents constructeurs pour cette classe
        public Contact(int id=0,string name="Aucun nom",string adress="Aucune adresse",string numOfPhone="xx-xx-xx-xx-xx",string emailAdress="Aucune adresse",string webSite="Aucun site",string fbLink="Aucun compte Facebook",string linkedInLink="Aucun compte LinkedIn")
        {
            _id = id;
            _name = name;
            _adress = adress;
            _numOfPhone = numOfPhone;
            _emailAdress = emailAdress;
            _webSite = webSite;
            _fbLink = fbLink;
            _linkedInLink = linkedInLink;
        }
        
    }

}
