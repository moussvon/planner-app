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
    /// Logique d'interaction pour StackDate.xaml
    /// </summary>
    partial class StackDate : UserControl
    {
        public StackDate()
        {
            InitializeComponent();
        }
        public void LoadData(List<Tache> list, int day, int month, int year)
        {
            switch (month) {
                case 1:
                    this.JourTitle.Text = $"{day} Janvier {year}";
                    break;
                case 2:
                    this.JourTitle.Text = $"{day} Février {year}";
                    break;
                case 3:
                    this.JourTitle.Text = $"{day} Mars {year}";
                    break;
                case 4:
                    this.JourTitle.Text = $"{day} Avril {year}";
                    break;
                case 5:
                    this.JourTitle.Text = $"{day} May {year}";
                    break;
                case 6:
                    this.JourTitle.Text = $"{day} Juin {year}";
                    break;
                case 7:
                    this.JourTitle.Text = $"{day} Juillet {year}";
                    break;
                case 8:
                    this.JourTitle.Text = $"{day} Août {year}";
                    break;
                case 9:
                    this.JourTitle.Text = $"{day} Septembre {year}";
                    break;
                case 10:
                    this.JourTitle.Text = $"{day} Octobre {year}";
                    break;
                case 11:
                    this.JourTitle.Text = $"{day} Novembre {year}";
                    break;
                case 12:
                    this.JourTitle.Text = $"{day} Décembre {year}";
                    break;

            }
            Dictionary<string, List<Tache>> d = new Dictionary<string, List<Tache>>();
            foreach (Tache t in list)
            {
                if (!d.ContainsKey(t.Activitee)) d[t.Activitee] = new List<Tache>();
                d[t.Activitee].Add(t);
            }
            foreach (string s in d.Keys)
            {
                StackActivite sa = new StackActivite();
                sa.LoadData(s, DataSupervisor.ds.user.Activities.ContainsKey(s) ? DataSupervisor.ds.user.Activities[s] : "#555555", d[s]);
                actStack.Children.Add(sa);
            }
            if (actStack.Children.Count == 1)
            {
                TextBlock tb = new TextBlock()
                {
                    Text = "Vous n'avez pas ajouté de tâches",
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                actStack.Children.Add(tb);
            }
        }
        public void LoadData(List<Evenement> list, int day, int month, int year)
        {
            this.JourTitle.Text = $"{day} {month} {year}";
            Dictionary<string, List<Evenement>> d = new Dictionary<string, List<Evenement>>();
            foreach (Evenement t in list)
            {
                if (!d.ContainsKey(t.activite)) d[t.activite] = new List<Evenement>();
                d[t.activite].Add(t);
            }
            foreach (string s in d.Keys)
            {
                StackActivite sa = new StackActivite();
                sa.LoadData(s, DataSupervisor.ds.user.Activities[s], d[s]);
                actStack.Children.Add(sa);
            }
            if (actStack.Children.Count == 1)
            {
                TextBlock tb = new TextBlock()
                {
                    Text = "Vous n'avez pas ajouté d'événements",
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center
                };
                actStack.Children.Add(tb);
            }
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FormulaireAddTache form = new FormulaireAddTache();
            WindowFormulaires winF = new WindowFormulaires();
            form.windowParent = winF;
            winF.gridPrincWinForm.Children.Add(form);
            winF.ShowDialog();
        }
    }
}
