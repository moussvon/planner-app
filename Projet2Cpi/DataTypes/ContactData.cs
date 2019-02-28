using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Projet2Cpi
{
     public class ContactData
    { /// <summary>
      /// Cette classe représente un contact
      /// 
      /// </summary>
        private string _id;//Cet attribut représente l'identifiant du contact
        public string id
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

        private string _twitter;//Cet attribut représente le site web de l'utilisateur
        public string twitter
        {
            get { return _twitter; }
            set { _twitter = value; }
        }

        private string _profession;//Cet attribut représente la profession du contact
        public string profession { get { return _profession; } set { _profession = value; } }

        private string _lieuTravail;//Cet attribut représente le lieu de travail du contact
        public string lieuTravail { set { _lieuTravail = value; } get { return _lieuTravail; } }

        private string _pathPicture; //Cet attribut désigne le chemin de la photo de profil du contact
        public string pathPicture { get { return _pathPicture; } set {_pathPicture = value; } }

        private string _dateAnniv;//Cet attribut représente la date d'anniversaire du contact
        public string dateAnniv { get { return _dateAnniv; } set { _dateAnniv = value; } }

        public int indexInListView;


        //Différents constructeurs pour cette classe
        public ContactData(string name = "Aucun nom", string adress = "Aucune adresse", string numOfPhone = "xx-xx-xx-xx-xx", string emailAdress = "Aucune adresse", string webSite = "Aucun site", string fbLink = "www.facebook.com", string linkedInLink = "www.linkedin.com", string twitter = "www.twitter.com",string profession="Aucune profession",string pathPict = @"\images\photoProfilDefault.jpg")
        {
            _id = encode(DateTime.Now.ToBinary());
            _name = name;
            _adress = adress;
            _numOfPhone = numOfPhone;
            _emailAdress = emailAdress;
            _webSite = webSite;
            _fbLink = fbLink;
            _linkedInLink = linkedInLink;
            _twitter = twitter;
            _profession = profession;
            if (pathPict == @"\images\photoProfilDefault.jpg") _pathPicture = Environment.CurrentDirectory + pathPict;
            else _pathPicture = pathPict;
            System.Threading.Thread.Sleep(1);
        }
        public string encode(long val)
        {
            string s = val.ToString(),ss=String.Empty;
            foreach(char c in s)
            {
                switch (c)
                {
                    case '0':
                        ss+= 'a';
                        break;
                    case '1':
                        ss += 'b';
                        break;
                    case '2':
                        ss += 'c';
                        break;
                    case '3':
                        ss += 'd';
                        break;
                    case '4':
                        ss += 'e';
                        break;
                    case '5':
                        ss += 'f';
                        break;
                    case '6':
                        ss += 'g';
                        break;
                    case '7':
                        ss += 'h';
                        break;
                    case '8':
                        ss += 'i';
                        break;
                    case '9':
                        ss += 'j';
                        break;
                    case '-':
                        ss += 'k';
                        break;
                }
            }
            return ss;
        }
    }
}
