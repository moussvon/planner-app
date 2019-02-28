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
    /// Logique d'interaction pour StackActivite.xaml
    /// </summary>
    partial class StackActivite : UserControl
    {
        public StackActivite()
        {
            InitializeComponent();
        }
        public void LoadData(string activityTitle, string color, List<Tache> list)
        {
            this.ActivityTitle.Text = activityTitle;
            this.activityIndicator.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            int order = MainWindow.TachesEventsField == null ? 0 : MainWindow.TachesEventsField.orderSelect.SelectedIndex;
            if (order == 0) list.Sort(new TacheTimeComparer());
            else list.Sort(new TacheUrgenceComparer());
            foreach (Tache i in list)
            {
                if (!(i.Activitee == activityTitle)) continue;
                CarteTache c = new CarteTache(i);
                this.Stack.Children.Add(c);
            };
            if (this.Stack.Children.Count == 1)
            {
                TextBlock tb = new TextBlock()
                {
                    Text = "Vous n'avez pas ajouté de tâches",
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                this.Stack.Children.Add(tb);
            }
        }
        public void LoadData(string activityTitle, string color, List<Evenement> list)
        {
            this.ActivityTitle.Text = activityTitle;
            this.activityIndicator.Stroke = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
            int order = MainWindow.TachesEventsField == null ? 0 : MainWindow.TachesEventsField.orderSelect.SelectedIndex;
            if (order == 0) list.Sort(new EventTimeComparer());
            else list.Sort(new EventUrgenceComparer());
            foreach (Evenement i in list)
            {
                if (!(i.activite == activityTitle)) continue;
                CarteEvent c = new CarteEvent(i);
                this.Stack.Children.Add(c);
            };
            if (this.Stack.Children.Count == 1)
            {
                TextBlock tb = new TextBlock()
                {
                    Text = "Vous n'avez pas ajouté d'événements",
                    FontSize = 16,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                this.Stack.Children.Add(tb);
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
