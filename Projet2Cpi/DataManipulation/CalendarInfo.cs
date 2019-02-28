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
    [Serializable]
    public class CalendarInfo
    {

        public Dictionary<DateTime, List<CalendarObj>> Data = new Dictionary<DateTime, List<CalendarObj>>();
        public DateTime Month = DateTime.Now;
        public string WorkingFile;

        public List<Tache> GetDayTaches(int year, int month, int day)
        {
            List<Tache> lst = new List<Tache>();
            if (year != this.Month.Year || month != this.Month.Month) throw new Exception("Day not in this month file");
            DateTime d = new DateTime(year, month, day);
            foreach (Tache t in DataSupervisor.ds.GetDayAdditionalSeancesTaches(d))
            {
                lst.Add(t);
            }
            if (!this.Data.ContainsKey(d)) return lst;
            foreach (CalendarObj c in this.Data[d])
            {
                if (c is Tache) lst.Add((Tache) c);
            }
            return lst;
        }

        public List<Evenement> GetDayEvents(int year, int month, int day)
        {
            List<Evenement> lst = new List<Evenement>();
            if (year != this.Month.Year || month != this.Month.Month) throw new Exception("Day not in this month file");
            DateTime d = new DateTime(year, month, day);
            if (!this.Data.ContainsKey(d)) return lst;
            foreach (CalendarObj c in this.Data[d])
            {
                if (c is Evenement) lst.Add((Evenement) c);
            }
            return lst;
        }

        public Dictionary<DateTime, List<Tache>> getMonthTaches()
        {
            Dictionary<DateTime, List<Tache>> d = new Dictionary<DateTime, List<Tache>>();
            foreach (KeyValuePair<DateTime, List<CalendarObj>> t in this.Data)
            {
                List<Tache> lst = new List<Tache>();
                foreach (CalendarObj i in t.Value)
                {
                    if (i is Tache)
                    {
                        lst.Add((Tache) i);
                    }
                }
                d[t.Key] = lst;
            }
            for (int i = 1; i <= DateTime.DaysInMonth(Month.Year, Month.Month); ++i)
            {
                DateTime day = new DateTime(Month.Year, Month.Month, i);
                List<Tache> lt = DataSupervisor.ds.GetDayAdditionalSeancesTaches(day);
                if (!d.ContainsKey(day))
                {
                    d[day] = lt;
                } else
                {
                    foreach (Tache tache in lt)
                    {
                        d[day].Add(tache);
                    }
                }
            };
            return d;
        }

        public Dictionary<DateTime, List<Evenement>> getMonthEvenements()
        {
            Dictionary<DateTime, List<Evenement>> d = new Dictionary<DateTime, List<Evenement>>();
            foreach (KeyValuePair<DateTime, List<CalendarObj>> t in this.Data)
            {
                List<Evenement> lst = new List<Evenement>();
                foreach (CalendarObj i in t.Value)
                {
                    if (i is Evenement)
                    {
                        lst.Add((Evenement)i);
                    }
                }
                d[t.Key] = lst;
            }
            return d;
        }

        public void AddTache(DateTime d, Tache t)
        {
            if (this.Data.ContainsKey(d)) this.Data[d].Add(t);
            else
            {
                this.Data.Add(d, new List<CalendarObj>());
                this.Data[d].Add(t);
            }
            this.StoreData();
        }

        public void AddEvenement(DateTime d, Evenement e)
        {
            if (this.Data.ContainsKey(d)) this[d].Add(e);
            else
            {
                this[d] = new List<CalendarObj>();
                this[d].Add(e);
            }
            this.StoreData();
        }
        
        public List<CalendarObj> this[DateTime day]
        {
            get { return Data[day]; }
            set { Data[day] = value; }
        }

        public CalendarInfo(string WorkingFile, DateTime month)
        {
            this.WorkingFile = WorkingFile;
            this.Month = month;
            bool exists = true;
            string SaveFile = "";
            try
            {
                SaveFile = File.ReadAllText(this.WorkingFile);
            } catch (FileNotFoundException)
            {
                exists = false;
            }
            if (exists) SaveFile = Sec.decrypt(SaveFile, DataSupervisor.ds.userPasswrd);
            else SaveFile = "{}";
            this.Data = (Dictionary<DateTime, List<CalendarObj>>) JsonConvert.DeserializeObject(SaveFile, this.Data.GetType(), new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto
            });
        }

        public void StoreData()
        {
            string saveData = JsonConvert.SerializeObject(this.Data, Formatting.Indented, new JsonSerializerSettings()
            {
                TypeNameHandling = TypeNameHandling.Auto,
            });
            string encrypted = Sec.encrypt(saveData, DataSupervisor.ds.userPasswrd);
            File.WriteAllText(this.WorkingFile, encrypted);
        }
    }
}
