using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Projet2Cpi
{
    public class Document
    {
        /// <summary>
        /// Cette classe représente les documents associés aux tâches
        /// 
        /// </summary>
        private int _id;//Entier représentant l'identifiant du document
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

        private string _title;//string représentant le titre du document
        public string title
        {
            get
            {
                return _title;
            }
            set
            {
                _title = value;
            }
        }

        private string _path;//string représentant le chemin du document
        public string path
        {
            get
            {
                return _path;
            }
            set
            {
                _path = value;
            }
        }

        //Les différents constructeurs de cette classe
        public Document(int id = 0,string title = "Aucun nom",string path= "Aucun fichier")
        {
            _id = id;
            _title = title;
            _path = path;
        }

    }
}
