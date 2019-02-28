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
    /// Interaction logic for AffichageParSemaineControl.xaml
    /// </summary>
    public partial class AffichageParSemaineControl : UserControl
    {
        public AffichageParSemaineControl()
        {
            InitializeComponent();
        }

        public void UpdateWithTaches(int week, int month, int year)
        {
            dateStack.Children.Clear();
            Dictionary<int, List<DateTime>> d = DivideToWeeks(month, year);
            foreach (DateTime i in d[week])
            {
                StackDate sd = new StackDate();
                List<Tache> l = DataSupervisor.ds.GetDayTaches(i.Day, i.Month, i.Year);
                sd.LoadData(l, i.Day, i.Month, i.Year);
                dateStack.Children.Add(sd);
            }
        }

        public void UpdateWithEvents(int week, int month, int year)
        {
            dateStack.Children.Clear();
            Dictionary<int, List<DateTime>> d = DivideToWeeks(month, year);
            foreach (DateTime i in d[week])
            {
                StackDate sd = new StackDate();
                List<Evenement> l = DataSupervisor.ds.GetDayEvents(i.Day, i.Month, i.Year);
                sd.LoadData(l, i.Day, i.Month, i.Year);
                dateStack.Children.Add(sd);
            }
        }

        public static Dictionary<int, List<DateTime>> DivideToWeeks(int month, int year)
        {
            Dictionary<int, List<DateTime>> weekDictionary = new Dictionary<int, List<DateTime>>();
            DateTime d = new DateTime(year, month, 1);
            d = d.AddDays(-(int)d.DayOfWeek);
            // week1
            int i = 1;
            weekDictionary[i] = new List<DateTime>();
            weekDictionary[i].Add(new DateTime(d.Ticks));
            d = d.AddDays(1);
            while ((int)d.DayOfWeek != 0)
            {
                weekDictionary[i].Add(new DateTime(d.Ticks));
                d = d.AddDays(1);
            }
            i += 1;
            while (d.Month == month)
            {
                weekDictionary[i] = new List<DateTime>();
                weekDictionary[i].Add(new DateTime(d.Ticks));
                d = d.AddDays(1);
                while ((int)d.DayOfWeek != 0)
                {
                    weekDictionary[i].Add(new DateTime(d.Ticks));
                    d = d.AddDays(1);
                }
                i += 1;
            }
            return weekDictionary;
        }
    }
}
