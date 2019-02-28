using Microsoft.Win32;
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

namespace Projet2Cpi
{
    /// <summary>
    /// Logique d'interaction pour FormulaireAddContact.xaml
    /// </summary>
    public partial class FormulaireAddContact : UserControl
    {
        public FormulaireAddContact()
        {
            InitializeComponent();
        }
        public WindowFormulaires windowFather;
        public Contacts contactsPage;
        private void bouttonAnnulerAddContact_Click(object sender, RoutedEventArgs e)
        {
            windowFather.Close();
        }

        private void bouttonAddContact_Click(object sender, RoutedEventArgs e)
        {
            if (nomPrenomInput.Text == "")
            {
                nomPrenomInput.Focus();
            }
            else if (numPhoneInput.Text == "")
            {
                numPhoneInput.Focus();
            }
            else if (emailInput.Text == "")
            {
                emailInput.Focus();
            }
            else
            {
                string nom = nomPrenomInput.Text;
                string adr = adresseInput.Text;
                string numT = numPhoneInput.Text;
                string mail = emailInput.Text;
                string work = travailInput.Text;
                string site = webSiteInput.Text;
                string fb = facebookInput.Text;
                string lin = linkedinInput.Text;
                string twitt = twitterInput.Text;

                if (adr.Equals("")) mail = "Pas d'adresse";
                if (work.Equals("")) work = "Aucun travail";
                if (site.Equals("")) site = "Aucun Site";
                if (fb.Equals("")) fb = "www.facebook.com";
                if (lin.Equals("")) lin = "www.linkedin.com";
                if (twitt.Equals("")) twitt = "www.twitter.com";

                ContactData contact = new ContactData(nom, adr, numT, mail, site, fb, lin, twitt, work, imgPath);
                if (Contacts.current != null)
                {
                    Contacts.current.addContact(contact);
                }
                DataSupervisor.ds.Contactes.Data.Add(contact.id, contact);
                DataSupervisor.ds.Contactes.StoreData();
                windowFather.Close();
            }
        }

        private string imgPath = Directory.GetCurrentDirectory() + @"//images//photoProfilContactDefault.jpg";

        private void PackIcon_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Image Files|*.jpg;*.png";
            file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            file.Multiselect = false; //On peut selectionner multiple fichiers

            if (file.ShowDialog() == true)
            {
                ImgName.Text = file.SafeFileName;
                imgPath = file.FileName;
            }

        }
    }
}
