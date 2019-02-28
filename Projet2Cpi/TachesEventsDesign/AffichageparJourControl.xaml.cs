using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Projet2Cpi
{
    /// <summary>
    /// Interaction logic for AffichageparJourControl.xaml
    /// </summary>
    public partial class AffichageparJourControl : UserControl
    {
        public AffichageparJourControl()
        {
            InitializeComponent();
        }

        public void UpdateWithTaches(int day, CalendarInfo c)
        {
            activitiesStack.Children.Clear();
            DateTime d = new DateTime(c.Month.Year, c.Month.Month, day);
            List<Tache> datal = c.GetDayTaches(c.Month.Year, c.Month.Month, day);
            if (datal.Count != 0)
            {
                Dictionary<string, List<Tache>> actDictionary = new Dictionary<string, List<Tache>>();
                actDictionary[""] = new List<Tache>();
                foreach (string s in DataSupervisor.ds.user.Activities.Keys)
                {
                    actDictionary[s] = new List<Tache>();
                }
                foreach (Tache t in datal)
                {
                    if (actDictionary.ContainsKey(t.Activitee)) actDictionary[t.Activitee].Add(t);
                    else actDictionary[""].Add(t);
                }
                
                foreach (KeyValuePair<string, List<Tache>> kv in actDictionary)
                {
                    if (kv.Key == "") continue;
                    StackActivite s = new StackActivite();
                    string color = "#FFFFFF";
                    if (DataSupervisor.ds.user.Activities.ContainsKey(kv.Key)) color = DataSupervisor.ds.user.Activities[kv.Key];
                    s.LoadData(kv.Key, color, kv.Value);
                    activitiesStack.Children.Add(s);
                }
            } else
            {
                TextBlock tb = new TextBlock()
                {
                    Text = "Vous n'avez pas ajouté de tâches",
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                activitiesStack.Children.Add(tb);
            }
        }

        public void UpdateWithEvents(int day, CalendarInfo c)
        {
            activitiesStack.Children.Clear();
            DateTime d = new DateTime(c.Month.Year, c.Month.Month, day);
            if (c.Data.ContainsKey(d))
            {
                List<Evenement> datal = c.GetDayEvents(c.Month.Year, c.Month.Month, day);
                Dictionary<string, List<Evenement>> actDictionary = new Dictionary<string, List<Evenement>>();
                actDictionary[""] = new List<Evenement>();
                foreach (string s in DataSupervisor.ds.user.Activities.Keys)
                {
                    actDictionary[s] = new List<Evenement>();
                }
                foreach (Evenement t in datal)
                {
                    if (actDictionary.ContainsKey(t.activite)) actDictionary[t.activite].Add(t);
                    else actDictionary[""].Add(t);
                }
                foreach (KeyValuePair<string, List<Evenement>> kv in actDictionary)
                {
                    if (kv.Key == "") continue;
                    StackActivite s = new StackActivite();
                    string color = "#FFFFFF";
                    if (DataSupervisor.ds.user.Activities.ContainsKey(kv.Key)) color = DataSupervisor.ds.user.Activities[kv.Key];
                    s.LoadData(kv.Key, color, kv.Value);
                    activitiesStack.Children.Add(s);
                }
            }
            else
            {
                TextBlock tb = new TextBlock()
                {
                    Text = "Vous n'avez pas ajouté d'événements",
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                activitiesStack.Children.Add(tb);
            }
        }
    }
}
