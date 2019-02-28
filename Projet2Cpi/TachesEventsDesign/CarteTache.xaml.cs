using System;
using System.Collections.Generic;
using System.IO;
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
using System.Text.RegularExpressions;

namespace Projet2Cpi
{
    /// <summary>
    /// Logique d'interaction pour CarteTache.xaml
    /// </summary>
    public partial class CarteTache : UserControl
    {
        public CarteTache()
        {
            InitializeComponent();
        }

        private Tache t;

        public CarteTache(Tache t)
        {
            InitializeComponent();
            Regex r = new Regex(@"[^/\\]+$");
            this.TacheTitle.Text = t.title;
            this.TacheDetails.Text = t.Details;
            string time1 = t.dateDebut.Hour >= 12 ? "AM" : "PM";
            string time2 = t.dateFin.Hour >= 12 ? "AM" : "PM";
            this.tacheDuration.Text = $@"{t.dateDebut.Hour}:{t.dateDebut.Minute} {time1}_{t.dateFin.Hour}:{t.dateFin.Minute} {time2}";
            Dictionary<string, string> colorDict = new Dictionary<string, string>();
            colorDict["Urgente"] = "#DDFF0707";
            colorDict["Normale"] = "#FFEB3B";
            colorDict["Basse"] = "#DD64CEFF";
            priorityTitle.Text = t.priorite;
            priorityIndicator.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorDict[t.priorite]));
            this.t = t;
            try
            {
                documentTitle.Text = r.Match(t.Fichiers[0]).Value;
                alarmText.Text = t.Alarms[0].time.ToString();
            }
            catch (Exception e) { }
            task_activity_tag.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataSupervisor.ds.user.Activities[t.Activitee]));
            TaskStateToggle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataSupervisor.ds.user.Activities[t.Activitee]));
        }

        private void Modifier_event(object sender, RoutedEventArgs e)
        {
            TacheModificationDialog d = new TacheModificationDialog();
            d.FillWithTache(t);
            d.ShowDialog();
        }

        private void Supprimer_event(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Voulez vous vraiment supprimer la tâche?";
            string sCaption = "Suppression de la Tâche";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNo;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    DataSupervisor.ds.SupprimerTache(this.t);
                    MainWindow.UpdateFields();
                    break;

                case MessageBoxResult.No:
                    break;
            }
        }

        private void documentTitle_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(t.Fichiers[0]);
            }
            catch (Exception ee) { }
        }
    }
}
