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
using System.Text.RegularExpressions;

namespace Projet2Cpi
{
    /// <summary>
    /// Logique d'interaction pour CarteEvent.xaml
    /// </summary>
    public partial class CarteEvent : UserControl
    {
        public CarteEvent()
        {
            InitializeComponent();
        }
        private Evenement ev = new Evenement();
        public CarteEvent(Evenement e)
        {
            InitializeComponent();
            Regex r = new Regex(@"[^/\\]+$");
            this.ev = e;
            this.eventTitle.Text = e.Title;
            this.eventDetails.Text = e.Description;
            this.eventPlace.Text = e.place;
            switch (e.DateDebut.Month)
            {
                case 1:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} Janvier";
                    break;
                case 2:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} Fevrier"; break;
                case 3:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} Mars"; break;
                case 4:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} Avril"; break;
                case 5:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} May"; break;
                case 6:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} Juin"; break;
                case 7:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} Juillet"; break;
                case 8:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} Août"; break;
                case 9:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} Septembre"; break;
                case 10:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} Octobre"; break;
                case 11:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} Novembre"; break;
                case 12:
                    this.eventDateDeb.Text = $"{e.DateDebut.Day} Decembre"; break;
            }
            switch (e.DateFin.Month)
            {
                case 1:
                    this.eventDateDeb.Text = $"{e.DateFin.Day} Janvier";
                    break;                        
                case 2:                           
                    this.eventDateFin.Text = $"{e.DateFin.Day} Fevrier"; break;
                case 3:                           
                    this.eventDateFin.Text = $"{e.DateFin.Day} Mars"; break;
                case 4:                           
                    this.eventDateFin.Text = $"{e.DateFin.Day} Avril"; break;
                case 5:                           
                    this.eventDateFin.Text = $"{e.DateFin.Day} May"; break;
                case 6:                           
                    this.eventDateFin.Text = $"{e.DateFin.Day} Juin"; break;
                case 7:                           
                    this.eventDateFin.Text = $"{e.DateFin.Day} Juillet"; break;
                case 8:                           
                    this.eventDateFin.Text = $"{e.DateFin.Day} Août"; break;
                case 9:                           
                    this.eventDateFin.Text = $"{e.DateFin.Day} Septembre"; break;
                case 10:                          
                    this.eventDateFin.Text = $"{e.DateFin.Day} Octobre"; break;
                case 11:                          
                    this.eventDateFin.Text = $"{e.DateFin.Day} Novembre"; break;
                case 12:                          
                    this.eventDateFin.Text = $"{e.DateFin.Day} Decembre"; break;
            }
            Dictionary<string, string> colorDict = new Dictionary<string, string>();
            colorDict["Urgente"] = "#DDFF0707";
            colorDict["Normale"] = "#FFEB3B";
            colorDict["Basse"] = "#DD64CEFF";
            this.priorityText.Text = e.Priority;
            this.priorityIndicator.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(colorDict[e.Priority]));
            try
            {
                documentText.Text = r.Match(e.Fichiers[0]).Value;
                alarmText.Text = e.Alarms[0].time.ToString();
            }
            catch (Exception e1) { }
            event_activity_tag.Fill = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataSupervisor.ds.user.Activities[e.activite]));
            EventStateToggle.Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataSupervisor.ds.user.Activities[e.activite]));
            eventPlace.Text = e.place;
        }

        private void Modifier_Click(object sender, RoutedEventArgs e)
        {
            EventModificationDialog d = new EventModificationDialog();
            d.FillWithEvent(ev);
            d.ShowDialog();
        }

        private void Supprimer_Click(object sender, RoutedEventArgs e)
        {
            string sMessageBoxText = "Vvoulez vous vraiment supprimer l'événement?";
            string sCaption = "Suppression de l'événement";

            MessageBoxButton btnMessageBox = MessageBoxButton.YesNoCancel;
            MessageBoxImage icnMessageBox = MessageBoxImage.Warning;

            MessageBoxResult rsltMessageBox = MessageBox.Show(sMessageBoxText, sCaption, btnMessageBox, icnMessageBox);

            switch (rsltMessageBox)
            {
                case MessageBoxResult.Yes:
                    DataSupervisor.ds.SupprimerEvent(this.ev);
                    MainWindow.UpdateFields();
                    break;

                case MessageBoxResult.No:
                    break;

                case MessageBoxResult.Cancel:
                    break;
            }
        }

        private void documentText_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(ev.Fichiers[0]);
            }
            catch (Exception ee) { }
        }
    }
}
