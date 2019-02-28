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
    /// Logique d'interaction pour Taches.xaml
    /// </summary>
    public partial class Taches : UserControl
    {
        private AffichageparJourControl affichageparJourControl = new AffichageparJourControl();
        private AffichageParSemaineControl affichageparSemaineControl = new AffichageParSemaineControl();
        public Taches()
        {
            InitializeComponent();
            this.jourSemainField.Content = affichageparJourControl;
            this.Update();
        }
        private int currentYear = 0;
        private int currentMonth = 0;
        private bool Tacheactive = true;
        private bool jourActive = true;

        private void UpdateVisuals()
        {
            if (jourActive)
            {
                this.TachemonthYearSelectRegion.ChangeToJours();
                this.jourSemainField.Content = affichageparJourControl;
            } else
            {
                this.TachemonthYearSelectRegion.ChangeToSemaines();
                this.jourSemainField.Content = affichageparSemaineControl;
            }
        }
        public void Update()
        {
            
            if (TachemonthYearSelectRegion.Year != this.currentYear || TachemonthYearSelectRegion.Month != this.currentMonth)
            {
                this.currentYear = TachemonthYearSelectRegion.Year;
                this.currentMonth = TachemonthYearSelectRegion.Month;
                DataSupervisor.ds.LoadMonthData(this.currentYear, this.currentMonth);
            }
            if (Tacheactive) this.ParseMonthTachesDataJour(DataSupervisor.ds.MonthData);
            else this.ParseMonthEvenementsData(DataSupervisor.ds.MonthData);
        }


        private void ParseMonthTachesDataJour(CalendarInfo data)
        {
            if (jourActive)
            {
                this.jourSemainField.Content = affichageparJourControl;
                affichageparJourControl.UpdateWithTaches(this.TachemonthYearSelectRegion.selectedDay, data);
            } else
            {
                this.jourSemainField.Content = affichageparSemaineControl;
                affichageparSemaineControl.UpdateWithTaches(this.TachemonthYearSelectRegion.selectedWeek, this.TachemonthYearSelectRegion.Month, this.TachemonthYearSelectRegion.Year);
            }
        }

        private void ParseMonthEvenementsData(CalendarInfo data)
        {
            if (jourActive)
            {
                this.jourSemainField.Content = affichageparJourControl;
                affichageparJourControl.UpdateWithEvents(this.TachemonthYearSelectRegion.selectedDay, data);
            }
            else
            {
                this.jourSemainField.Content = affichageparSemaineControl;
                affichageparSemaineControl.UpdateWithEvents(this.TachemonthYearSelectRegion.selectedWeek, this.TachemonthYearSelectRegion.Month, this.TachemonthYearSelectRegion.Year);
            }
        }
        
        private void BoutonTacheEvenement_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((Border)sender).Background.Equals(Brushes.White))
            {
                ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(104, 102, 247));
                ((TextBlock)(((Border)sender).Child)).Foreground = Brushes.White;
            }
        }

        private void BoutonTacheEvenement_MouseLeave(object sender, MouseEventArgs e)
        {
            if (((Border)sender).Background.ToString().Equals((new SolidColorBrush(Color.FromRgb(104, 102, 247))).ToString()))
            {
                ((Border)sender).Background = Brushes.White;
                ((TextBlock)(((Border)sender).Child)).Foreground = new SolidColorBrush(Color.FromRgb(104, 102, 247));
            }
        }

        private void BoutonGaucheTacheEvenement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((Border)sender).Background.ToString().Equals((new SolidColorBrush(Color.FromRgb(104, 102, 247))).ToString()))
            {
                ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(40, 53, 147));
                ((TextBlock)(((Border)sender).Child)).Foreground = Brushes.White;
                BoutonDroitTacheEvenement.Background = Brushes.White;
                ((TextBlock)(BoutonDroitTacheEvenement.Child)).Foreground = new SolidColorBrush(Color.FromRgb(104, 102, 247));
                titrePage.Text = "Tâches";
                Tacheactive = true;
                this.Update();
            }
        }

        private void BoutonDroitTacheEvenement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((Border)sender).Background.ToString().Equals((new SolidColorBrush(Color.FromRgb(104, 102, 247))).ToString()))
            {
                ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(40, 53, 147));
                ((TextBlock)(((Border)sender).Child)).Foreground = Brushes.White;
                BoutonGaucheTacheEvenement.Background = Brushes.White;
                ((TextBlock)(BoutonGaucheTacheEvenement.Child)).Foreground = new SolidColorBrush(Color.FromRgb(104, 102, 247));
                titrePage.Text = "Evénements";
                Tacheactive = false;
                this.Update();
            }
        }

        private void BoutonGaucheJourSemaine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((Border)sender).Background.ToString().Equals((new SolidColorBrush(Color.FromRgb(104, 102, 247))).ToString()))
            {
                ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(40, 53, 147));
                ((TextBlock)(((Border)sender).Child)).Foreground = Brushes.White;
                BoutonDroitJourSemaine.Background = Brushes.White;
                ((TextBlock)(BoutonDroitJourSemaine.Child)).Foreground = new SolidColorBrush(Color.FromRgb(104, 102, 247));
                this.jourActive = true;
                this.UpdateVisuals();
                this.Update();
            }
        }

        private void BoutonDroitJourSemaine_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((Border)sender).Background.ToString().Equals((new SolidColorBrush(Color.FromRgb(104, 102, 247))).ToString()))
            {
                ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(40, 53, 147));
                ((TextBlock)(((Border)sender).Child)).Foreground = Brushes.White;
                BoutonGaucheJourSemaine.Background = Brushes.White;
                ((TextBlock)(BoutonGaucheJourSemaine.Child)).Foreground = new SolidColorBrush(Color.FromRgb(104, 102, 247));
                this.jourActive = false;
                this.UpdateVisuals();
                this.Update();
            }
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(94, 92, 223));//Modifie la couleur lors de l'entrée de la souris
        }

        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(104, 102, 247));//Modifie la couleur lors de la sortie de la souris
        }

        private void orderSelect_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainWindow.TachesEventsField != null) MainWindow.TachesEventsField.Update();
        }
    }
}
