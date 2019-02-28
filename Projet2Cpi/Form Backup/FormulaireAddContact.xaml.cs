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
            string nom = nomPrenomInput.Text;
            string adr = adresseInput.Text;
            string numT = numPhoneInput.Text;
            string mail = emailInput.Text;
            string work = travailInput.Text;
            string site = webSiteInput.Text;
            string fb = facebookInput.Text;
            string lin = linkedinInput.Text;
            string twitt = twitterInput.Text;

            if (nom.Equals("")) nom = "Aucun nom";
            if (adr.Equals("")) adr = "Aucune adresse";
            if (numT.Equals("")) numT = "xx xx xx xx xx";
            if (mail.Equals("")) mail = "ex : nedjima@naqqes.dz";
            if (work.Equals("")) work = "Aucun travail";
            if (site.Equals("")) site = "Aucun Site";
            if (fb.Equals("")) fb = "www.facebook.com";
            if (lin.Equals("")) lin = "www.linkedin.com";
            if (twitt.Equals("")) twitt = "www.twitter.com";

            ContactData contact = new ContactData(nom, adr, numT, mail, site, fb, lin, twitt, work);
            if (Contacts.current != null)
            {
                //contactsPage.addContact(contact);
                Contacts.current.addContact(contact);
            }
            DataSupervisor.ds.Contactes.Add(contact.id, contact);
            /*var data = new ContactInfo(Environment.CurrentDirectory+@"\Doc\Mas.json");
            data.Add(contact.id.ToString(), contact);*/
            windowFather.Close();
        }
    }
}
