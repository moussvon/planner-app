using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Projet2Cpi
{
    public class ContactInfo
    {
        public Dictionary<string, ContactData> Data = new Dictionary<string, ContactData>();
        private string workingFile;
        public string WorkingFile
        {
            get { return this.workingFile; }
            set { this.workingFile = value; }
        }

        public ContactData this[string name]
        {
            get { return Data[name]; }
            set
            {
                Data[name] = value;
                this.StoreData();
            }
        }

        /*~ContactInfo()
        {
            this.StoreData();
        }*/

        public ContactInfo(string WorkingFile)
        {
            this.WorkingFile = WorkingFile;
            bool exists = true;
            string SaveFile = "";
            try
            {
                SaveFile = File.ReadAllText(this.WorkingFile).Trim();
            }
            catch 
            {
                exists = false;
            }
            if (exists)
            {
                SaveFile = Sec.decrypt(SaveFile, DataSupervisor.ds.userPasswrd);
                this.Data = new Dictionary<string, ContactData>();
                this.Data = JsonConvert.DeserializeObject<Dictionary<string, ContactData>>(SaveFile, new JsonSerializerSettings()
                {
                    TypeNameHandling = TypeNameHandling.Auto
                });
            }
            else this.Data = new Dictionary<string, ContactData>();
        }
        
        public void StoreData()
        {
            string s = JsonConvert.SerializeObject(this.Data, Formatting.None, new JsonSerializerSettings
            {
                TypeNameHandling = TypeNameHandling.Auto
            }).Trim();
            s = Sec.encrypt(s, DataSupervisor.ds.userPasswrd);
            File.WriteAllText(this.WorkingFile, s);
        }
    }
}
