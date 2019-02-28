using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Windows;
using System.Web.Script.Serialization;
using Newtonsoft.Json;

namespace Projet2Cpi
{
    class TacheTimeComparer : IComparer<Tache>
    {
        public int Compare(Tache t1, Tache t2)
        {
            return t1.dateDebut.CompareTo(t2.dateDebut);
        }
    }
    class TacheUrgenceComparer : IComparer<Tache>
    {
        public int Compare(Tache t1, Tache t2)
        {
            int ut1 = t1.priorite == "Urgente" ? 3 : t1.priorite == "Normale" ? 2 : 1;
            int ut2 = t2.priorite == "Urgente" ? 3 : t2.priorite == "Normale" ? 2 : 1;
            return ut1 - ut2;
        }
    }
    class EventTimeComparer : IComparer<Evenement>
    {
        public int Compare(Evenement e1, Evenement e2)
        {
            return e1.DateDebut.CompareTo(e1.DateDebut);
        }
    }
    class EventUrgenceComparer : IComparer<Evenement>
    {
        public int Compare(Evenement e1, Evenement e2)
        {
            int ut1 = e1.Priority == "Urgente" ? 3 : e1.Priority == "Normale" ? 2 : 1;
            int ut2 = e2.Priority == "Urgente" ? 3 : e2.Priority == "Normale" ? 2 : 1;
            return ut1 - ut2;
        }
    }
    class DataSupervisor
    {
        public void SupprimerTache(Tache t)
        {
            if (t.dateDebut.Year == MonthData.Month.Year && t.dateDebut.Month == MonthData.Month.Month)
            {
                MonthData.Data[new DateTime(t.dateDebut.Year, t.dateDebut.Month, t.dateDebut.Day)].Remove(t);
                MonthData.StoreData();
            } else
            {
                CalendarInfo ci = new CalendarInfo(this.currentUserDataPath + $"\\MonthData{t.dateDebut.Month}-{t.dateDebut.Year}.json", new DateTime(t.dateDebut.Year, t.dateDebut.Month, 1));
                ci.Data[new DateTime(t.dateDebut.Year, t.dateDebut.Month, t.dateDebut.Day)].Remove(t);
                ci.StoreData();
            }
        }

        public void SupprimerEvent(Evenement e)
        {
            if (e.DateDebut.Year == MonthData.Month.Year && e.DateDebut.Month == MonthData.Month.Month)
            {
                MonthData.Data[new DateTime(e.DateDebut.Year, e.DateDebut.Month, e.DateDebut.Day)].Remove(e);
                MonthData.StoreData();
            }
            else
            {
                CalendarInfo ci = new CalendarInfo(this.currentUserDataPath + $"\\MonthData{e.DateDebut.Month}-{e.DateDebut.Year}.json", new DateTime(e.DateDebut.Year, e.DateDebut.Month, 1));
                ci.Data[new DateTime(e.DateDebut.Year, e.DateDebut.Month, e.DateDebut.Day)].Remove(e);
                ci.StoreData();
            }
        }

        public static List<Tache> FilterByActivities(List<Tache> list, string activitee)
        {
            List<Tache> l = new List<Tache>();
            foreach (Tache i in list) if (i.Activitee == activitee) l.Add(i);
            return l;
        }
        public static List<Evenement> FilterByActivities(List<Evenement> list, string activitee)
        {
            List<Evenement> l = new List<Evenement>();
            foreach (Evenement i in list) if (i.activite == activitee) l.Add(i);
            return l;
        }
        static public DataSupervisor ds;
        public User user;
        public String currentUserDataPath;
        private UsersLst usersLst;
        public CalendarInfo MonthData;
        public ContactInfo Contactes;
        public string userPasswrd = "";

        public static Tache ConvertFromSeanceToTache(Seance s, DateTime day)
        {
            Tache t = new Tache();
            t.title = s.name;
            t.Details = s.description;
            t.dateDebut = new DateTime(day.Year, day.Month, day.Day, s.begin.Hour, s.begin.Minute, s.begin.Second);
            t.dateFin = new DateTime(day.Year, day.Month, day.Day, s.end.Hour, s.end.Minute, s.end.Second);
            t.priorite = s.priority;
            t.Activitee = "Activité scolaire";
            return t;
        }

        private bool inPeriod(DateTime d, Tuple<DateTime?, DateTime?> p)
        {
            if (p == null) return false;
            if (p.Item1 == null || p.Item2 == null) return false;
            return (d >= p.Item1.Value) && (d <= p.Item2.Value);
        }

        public List<Tache> GetDayAdditionalSeancesTaches(DateTime day)
        {
            List<Tache> list = new List<Tache>();
            Tuple<DateTime?, DateTime?> p1 = DataSupervisor.ds.user.PlanningPeriod1;
            Tuple<DateTime?, DateTime?> p2 = DataSupervisor.ds.user.PlanningPeriod2;
            if (!(this.inPeriod(day, p1) || this.inPeriod(day, p2))) return list;
            DayOfWeek d = day.DayOfWeek;
            if (!user.Planning.ContainsKey(d)) return list;
            foreach (Seance s in user.Planning[d].Values)
            {
                list.Add(ConvertFromSeanceToTache(s, day));
            }
            return list;
        }

        public Dictionary<int, Seance> GetJourneeSeances(DayOfWeek d)
        {
            if (user == null) throw new Exception("User account not loaded");
            if (user.Planning.ContainsKey(d)) return user.Planning[d];
            else return null;
        }

        public void SetSeance(DayOfWeek d, int nbSeance, Seance seance)
        {
            if (!this.user.Planning.ContainsKey(d)) this.user.Planning[d] = new Dictionary<int, Seance>();
            this.user.Planning[d][nbSeance] = seance;
        }

        public void AddJourFerie(DateTime jour, string title)
        {
            if (user == null) throw new Exception("User account not loaded");
            user.JoursFeries[jour] = title;
        }

        public bool DeleteJourFerie(DateTime d)
        {
            if (user == null) throw new Exception("User account not loaded");
            if (!user.JoursFeries.ContainsKey(d)) return false;
            user.JoursFeries.Remove(d);
            return true;
        }

        public Dictionary<DateTime, string> GetJoursFeries()
        {
            if (user == null) throw new Exception("User account not loaded");
            return user.JoursFeries;
        }

        public List<Tache> GetDayTaches(int day, int month, int year)
        {
            List<Tache> lst;
            if (MonthData.Month.Month == month && MonthData.Month.Year == year)
            {
                lst = MonthData.GetDayTaches(year, month, day);
            } else
            {
                CalendarInfo data = new CalendarInfo(this.currentUserDataPath + $"\\MonthData{month}-{year}.json", new DateTime(year, month, day));
                lst = data.GetDayTaches(year, month, day);
            }
            return lst;
        }

        public List<Evenement> GetDayEvents(int day, int month, int year)
        {
            if (MonthData.Month.Month == month && MonthData.Month.Year == year)
            {
                return MonthData.GetDayEvents(year, month, day);
            }
            else
            {
                CalendarInfo data = new CalendarInfo(this.currentUserDataPath + $"\\MonthData{month}-{year}.json", new DateTime(year, month, day));
                return data.GetDayEvents(year, month, day);
            }
        }

        public void LoadContactes()
        {
            if (user == null) throw new Exception("User account is not loaded");
            this.Contactes = new ContactInfo(this.currentUserDataPath + @"\contactes.json");
        }

        public void LoadMonthData(int year, int month)
        {
            if (this.user == null) throw new Exception("User account is not loaded");
            if (this.MonthData != null) this.MonthData.StoreData();
            this.MonthData = new CalendarInfo(this.currentUserDataPath + $"\\MonthData{month}-{year}.json", new DateTime(year, month, 1));
        }
        
        private Notification ConvertToNotification(Notif not, string eventType, DateTime date, string title)
        {
            Notification n = new Notification()
            {
                Notif_type = not.Type,
                Num_tel_ou_email = not.Type == "email" ? user.userMail : user.phoneNumber,
                Date_notif = not.time,
                Date_event = date,
                Event_type = eventType,
                Username = user.userName,
                Message = title
            };
            return n;
        }

        public void AddTache(Tache t)
        {
            if (this.user == null) throw new Exception("User account is not loaded");
            if (t.dateDebut.Month == this.MonthData.Month.Month && t.dateDebut.Year == this.MonthData.Month.Year)
            {
                this.MonthData.AddTache(new DateTime(t.dateDebut.Year, t.dateDebut.Month, t.dateDebut.Day), t);
                MainWindow.CalendarField.UpdateCalendar();
                MainWindow.TachesEventsField.Update();
            } else
            {
                CalendarInfo ci = new CalendarInfo(this.currentUserDataPath + $"\\MonthData{t.dateDebut.Month}-{t.dateDebut.Year}.json", new DateTime(t.dateDebut.Year, t.dateDebut.Month, 1));
                ci.AddTache(new DateTime(t.dateDebut.Year, t.dateDebut.Month, t.dateDebut.Day), t);
                ci.StoreData();
            }
            foreach (Notif n in t.Alarms)
            {
                OtherThread.sendData(ConvertToNotification(n, "tache", t.dateDebut, t.title));
            }
        }

        public void AddEvenement(Evenement e)
        {
            if (this.user == null) throw new Exception("User account is not loaded");
            if (e.DateDebut.Month == this.MonthData.Month.Month && e.DateDebut.Year == this.MonthData.Month.Year)
            {
                this.MonthData.AddEvenement(new DateTime(e.DateDebut.Year, e.DateDebut.Month, e.DateDebut.Day), e);
                MainWindow.CalendarField.UpdateCalendar();
                MainWindow.TachesEventsField.Update();
            }
            else
            {
                CalendarInfo ci = new CalendarInfo(this.currentUserDataPath + $"\\MonthData{e.DateDebut.Month}-{e.DateDebut.Year}.json", new DateTime(e.DateDebut.Year, e.DateDebut.Month, 1));
                ci.AddEvenement(new DateTime(e.DateDebut.Year, e.DateDebut.Month, e.DateDebut.Day), e);
                ci.StoreData();
            }
            foreach (Notif n in e.Alarms)
            {
                OtherThread.sendData(ConvertToNotification(n, "evenment", e.DateDebut, e.Title));
            }
        }


        // Changes the password of the current user
        public void ChangePassword(String password)
        {
            if (this.user == null) throw new Exception("User account is not loaded");
            else
            {
                String username = this.user.userName;
                this.usersLst.ChangePassword(username, password);
            }
        }
        
        // Authenticates a user and sets its profile data to user object
        // Returns : null if the username doesn't exist, false if the password is not correct or true if the authenticathion succeeds
        public bool? LogIn(String username, String password)
        {
            if (this.user != null) this.LogOut();
            bool? authenRes = this.usersLst.AuthenticateUser(username, password);
            if (authenRes != true) return authenRes;
            userPasswrd = password;
            this.currentUserDataPath = Environment.CurrentDirectory + @"\Data\" + username;
            this.LoadUserProfile(username);
            this.LoadMonthData(DateTime.Now.Year, DateTime.Now.Month);
            this.LoadContactes();
            MainWindow.mw.LoadActivities();
            OtherThread.start();
            return true;
        }
        // Saves the data in user object and close it
        public void LogOut()
        {
            OtherThread.stop();
            if (this.user == null) return;
            this.SaveUserProfile();
            this.user = null;
            this.MonthData = null;
            this.Contactes = null;
            MainWindow.settingsField = null;
            MainWindow.ContactesField = null;
            userPasswrd = "";
        }
        // Creates a new account from the user profile and his password
        // Returns : false if the user already exists, true otherwise
        public bool CreateAccount(User user, String password)
        {
            if (this.usersLst.Exists(user.userName)) return false;
            this.userPasswrd = password;
            this.user = user;
            Directory.CreateDirectory(Environment.CurrentDirectory + $"\\Data\\{user.userName}");
            this.usersLst.AddUser(user.userName, password);
            this.LogIn(user.userName, password);
            return true;
        }
        public void SaveUserProfile()
        {
            String userDirectory = Environment.CurrentDirectory + $"\\Data\\{this.user.userName}";
            if (!Directory.Exists(userDirectory))
            {
                Directory.CreateDirectory(userDirectory);
            }
            String rawJSON = JsonConvert.SerializeObject(this.user);
            String encryptedJSON = Sec.encrypt(rawJSON, DataSupervisor.ds.userPasswrd);
            File.WriteAllText(userDirectory + "\\userProfile.json", encryptedJSON);
        }
        // Loads the user profile data from its corresponding file
        private void LoadUserProfile(String username)
        {
            string dataPath = this.currentUserDataPath + @"\userProfile.json";
            if (this.user != null) return;
            if (File.Exists(dataPath))
            {
                String rawJSON = "{}";
                String decryptedJSON = "{}";
                rawJSON = File.ReadAllText(dataPath).Trim();
                if (rawJSON == "") decryptedJSON = "{}";
                else decryptedJSON = Sec.decrypt(rawJSON, DataSupervisor.ds.userPasswrd);
                this.user = JsonConvert.DeserializeObject<User>(decryptedJSON);
            } 
        }
        // Saves all users credentials
        private void SaveUsersLst()
        {
            string rawJSON = JsonConvert.SerializeObject(this.usersLst);
            //TODO : insert encryption
            string encryptedJSON = rawJSON;
            using (StreamWriter sw = File.CreateText(Environment.CurrentDirectory + @"\Data\userslst.json"))
            {
                sw.Write(encryptedJSON);
            }
        }
        
        // The object constructor : loads the list of users credentials
        public DataSupervisor()
        {
            String dataDir = Environment.CurrentDirectory + @"\Data";
            if (!Directory.Exists(dataDir)) Directory.CreateDirectory(dataDir);
            if (File.Exists(dataDir + @"\userslst.json"))
            {
                using (StreamReader sr = new StreamReader(dataDir + @"\userslst.json"))
                {
                    string encryptedStr = sr.ReadToEnd();
                    //
                    string decryptedStr = encryptedStr;
                    this.usersLst = JsonConvert.DeserializeObject<UsersLst>(decryptedStr);
                }
            } else
            {
                usersLst = new UsersLst();
            }
        }
        // The object destructor : saves automatically the data being used
        ~DataSupervisor()
        {
            this.LogOut();
            this.SaveUsersLst();
        }
        private class UsersLst
        {
            public Dictionary<String, String> usersCredentials = new Dictionary<string, string>();
            public void ChangePassword(String username, String password)
            {
                this.usersCredentials[username] = Sec.hash(password);
            }
            public bool AddUser(String userName, String password)
            {
                if (this.usersCredentials.ContainsKey(userName)) return false;
                usersCredentials[userName] = Sec.hash(password);
                return true;
            }
            public bool? AuthenticateUser(String userName, String password)
            {
                if (!this.usersCredentials.ContainsKey(userName)) return null;
                return Sec.hash_and_compare(password, usersCredentials[userName]);
            }
            public bool Exists(String user)
            {
                return this.usersCredentials.ContainsKey(user);
            }
        }
    }
}
