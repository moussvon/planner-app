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
    /// Logique d'interaction pour UnContact.xaml
    /// </summary>
    public partial class UnContact : UserControl
    {
        public UnContact()
        {
            InitializeComponent();
            Adresse.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\images\Adresse.png"));
            Monde.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\images\Monde.png"));
            Fleche.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\images\Fleche.png"));
            Telephone.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\images\Telephone.png"));
            Cartable.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\images\Cartable.png"));
            twitterLink ="twitter.com";
            facebookLink = "facebook.com";
            linkedinLink ="linkedin.com";
        }

        private void boutonAbout_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Foreground.Equals(Brushes.DarkSlateGray))
            {
                boutonAbout.Foreground = Brushes.DarkBlue;
                boutonAbout.FontWeight = FontWeights.SemiBold;
                sepAbout.Foreground = Brushes.DarkBlue;
                boutonContacts.Foreground = Brushes.DarkSlateGray;
                boutonContacts.FontWeight = FontWeights.Light;
                sepContacts.Background = Brushes.DarkSlateGray;
                CardContacts.Visibility = Visibility.Collapsed;
                CardAbout.Visibility = Visibility.Visible;
            }
        }

        private void boutonContacts_Click(object sender, RoutedEventArgs e)
        {
            if (((Button)sender).Foreground.Equals(Brushes.DarkSlateGray))
            {
                boutonContacts.Foreground = Brushes.DarkBlue;
                boutonContacts.FontWeight = FontWeights.SemiBold;
                sepContacts.Foreground = Brushes.DarkBlue;
                boutonAbout.Foreground = Brushes.DarkSlateGray;
                boutonAbout.FontWeight = FontWeights.Light;
                sepAbout.Background = Brushes.DarkSlateGray;
                CardContacts.Visibility = Visibility.Visible;
                CardAbout.Visibility = Visibility.Collapsed;
            }
        }
        public string twitterLink;
        public string facebookLink;
        public string linkedinLink;
        private void twitterButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start("www.twitter.com");
            }
            catch { }
        }

        private void facebookButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(facebookLink);
            }
            catch { }
        }

        private void linkedinButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(linkedinLink);
            }
            catch { }
        }
    }
}
