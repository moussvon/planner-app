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
using MaterialDesignColors;
using System.IO;

namespace Projet2Cpi
{
    /// <summary>
    /// Logique d'interaction pour Contacts.xaml
    /// Cette classe gère les contacts, leur interface
    /// </summary>
    public partial class Contacts : UserControl
    {
        //Liste (de données) ou on charge les contacts du fichier de contacts de l'utilisateur au moment du chargement de cette page, on la libère en sortant de cette page
        private Dictionary<string,List<string>> listeNomsContacts;//Liste de noms (qui admet des doublons) afin de faciliter l'operation de recherche des noms (barre search)
        private int occurence;
        public static Contacts current;
        //
        public Contacts()
        {
            InitializeComponent();
            Contacts.current = this;
            listeNomsContacts = new Dictionary<string,List<string>>();
            occurence = 0;
            this.LoadContactes();

        }
        public void LoadContactes()
        {
            contactsListView.Items.Clear();
            foreach (ContactData c in DataSupervisor.ds.Contactes.Data.Values)
            {
                this.addContact(c);
            }
            if (DataSupervisor.ds.Contactes.Data.Values.Count == 0)
            {
                TextBlock tb = new TextBlock()
                {
                    Text = "Vous n'avez pas encore ajouté des contacts",
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    FontSize = 16
                };
                contactsListView.Items.Add(tb);
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

        private void searchBarContacts_TextChanged(object sender, TextChangedEventArgs e)
        {
            occurence = 0;
            if (listeNomsContacts.ContainsKey(searchBarContacts.Text)) {
                ListViewItem foundItem = (ListViewItem)(contactsListView.Items.GetItemAt(DataSupervisor.ds.Contactes[listeNomsContacts[searchBarContacts.Text].ElementAt(0).ToString()].indexInListView)); //Recherche l'élément parmi les éléments de la list view
                if (foundItem != null)
                {
                    occurence = 1;
                    if(((ListViewItem)contactsListView.SelectedValue) != null) ((ListViewItem)contactsListView.SelectedValue).IsSelected = false;
                    foundItem.IsSelected = true;
                    ContactData cc = DataSupervisor.ds.Contactes[foundItem.Name];
                    contactCarteDetails.NomDuContact.Text = cc.name;
                    contactCarteDetails.numDeTelephone.Text = cc.numOfPhone;
                    contactCarteDetails.photoDeProfilDuContact.Source = new BitmapImage(new Uri(cc.pathPicture));
                    contactCarteDetails.siteWebContact.Text = cc.webSite;
                    contactCarteDetails.travail.Text = cc.profession;
                    contactCarteDetails.adresseDuContact.Text = cc.adress;
                    contactCarteDetails.emailContact.Text = cc.emailAdress;
                    contactCarteDetails.linkedinLink = cc.linkedInLink;
                    contactCarteDetails.facebookLink = cc.fbLink;
                    contactCarteDetails.twitterLink = cc.twitter;
                }
            }
        }
        /// <summary>
        /// Cette fonction sert à ajouter dans la liste des contacts de l'interface un élément (contact) dans la list view (je n'ai pas utilisé de user control 
        /// pour des raisons techniques. Cette fonction insère le contact dans la liste view pour qu'il soit visible
        /// Cette fonction fait appel au user control "Photo" pour le traitement de la photo du contact.
        /// </summary>
        /// <param name="c"> ce paramètre représente un objet du type contactData, qui contient toutes les informations du contact.</param>
        public void addContact(ContactData c)
        { 
            if (this.contactsListView.Items.Count == 1 && this.contactsListView.Items[0] is TextBlock)
            {
                this.contactsListView.Items.Clear();
            }
            ListViewItem lvi = new ListViewItem();
            lvi.Name = c.id.ToString();
            StackPanel sp = new StackPanel();
            sp.Orientation = Orientation.Horizontal;
            Photo p = new Photo();
            try
            {
                p.imageDeProfil.Source = new BitmapImage(new Uri(c.pathPicture));
            } catch
            {
                p.imageDeProfil.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\images\photoProfilDefault.jpg"));
            }
            sp.Children.Add(p);
            StackPanel ssp = new StackPanel();
            ssp.Orientation = Orientation.Vertical;
            TextBlock nomContact = new TextBlock();
            nomContact.VerticalAlignment = VerticalAlignment.Top;
            nomContact.HorizontalAlignment = HorizontalAlignment.Left;
            nomContact.Text = c.name;
            nomContact.FontWeight = FontWeights.DemiBold;
            nomContact.FontSize = 20;
            nomContact.Margin = new Thickness(0,3,0,0);
            nomContact.FontFamily = new FontFamily("Roboto");
            TextBlock professionContact = new TextBlock();
            professionContact.VerticalAlignment = VerticalAlignment.Top;
            professionContact.HorizontalAlignment = HorizontalAlignment.Left;
            professionContact.Text = c.profession;
            professionContact.FontWeight = FontWeights.Light;
            professionContact.FontSize = 16;
            professionContact.Margin = new Thickness(0, 3, 0, 0);
            professionContact.FontFamily = new FontFamily("Roboto");
            TextBlock numTelephone = new TextBlock();
            numTelephone.VerticalAlignment = VerticalAlignment.Top;
            numTelephone.HorizontalAlignment = HorizontalAlignment.Left;
            numTelephone.Text = c.numOfPhone;
            numTelephone.FontWeight = FontWeights.Light;
            numTelephone.FontFamily = new FontFamily("Roboto");
            numTelephone.FontSize = 16;
            numTelephone.Margin = new Thickness(0, 3, 0, 0);
            numTelephone.FontStyle = FontStyles.Italic;
            ssp.Children.Add(nomContact);
            ssp.Children.Add(professionContact);
            ssp.Children.Add(numTelephone);
            sp.Children.Add(ssp);         
            lvi.Content = sp;
            contactsListView.Items.Add(lvi);
            c.indexInListView = contactsListView.Items.IndexOf(lvi);
            try
            {
                listeNomsContacts[c.name].Add(c.id);
            }
            catch (KeyNotFoundException)
            {
                listeNomsContacts.Add(c.name, new List<string>());
                listeNomsContacts[c.name].Add(c.id);
            }
        }
       
        private void listView_Click(object sender, RoutedEventArgs e)
        {
            var item = (sender as ListView).SelectedItem as ListViewItem;
            if (item != null)
            {
                ContactData cc = DataSupervisor.ds.Contactes[item.Name];
                contactCarteDetails.NomDuContact.Text = cc.name;
                contactCarteDetails.numDeTelephone.Text = cc.numOfPhone;
                try
                {
                    contactCarteDetails.photoDeProfilDuContact.Source = new BitmapImage(new Uri(cc.pathPicture));
                }
                catch
                {
                    contactCarteDetails.photoDeProfilDuContact.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\images\photoProfilDefault.jpg"));
                }
                contactCarteDetails.siteWebContact.Text = cc.webSite;
                contactCarteDetails.travail.Text = cc.profession;
                contactCarteDetails.adresseDuContact.Text = cc.adress;
                contactCarteDetails.emailContact.Text = cc.emailAdress;
                contactCarteDetails.linkedinLink = cc.linkedInLink;
                contactCarteDetails.facebookLink = cc.fbLink;
                contactCarteDetails.twitterLink = cc.twitter;
            }
        }

        private void ButtonAjouterContact_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowFormulaires winF = new WindowFormulaires();
            try
            {
                winF.gridPrincWinForm.Children.Clear();
            }
            catch { }
            FormulaireAddContact formulaireAddContact = new FormulaireAddContact();
            winF.gridPrincWinForm.Children.Add(formulaireAddContact);
            formulaireAddContact.windowFather = winF;
            formulaireAddContact.contactsPage = this;
            winF.ShowDialog();
        }

        private void searchButton_Click(object sender, RoutedEventArgs e)
        {
            if (listeNomsContacts.ContainsKey(searchBarContacts.Text))
            {
                ListViewItem foundItem = (ListViewItem)(contactsListView.Items.GetItemAt(DataSupervisor.ds.Contactes[listeNomsContacts[searchBarContacts.Text].ElementAt(occurence% listeNomsContacts[searchBarContacts.Text].Count).ToString()].indexInListView)); //Recherche l'élément parmi les éléments de la list view
                occurence++;
                if (foundItem != null)
                {
                    if (((ListViewItem)contactsListView.SelectedValue) != null) ((ListViewItem)contactsListView.SelectedValue).IsSelected = false;
                    foundItem.IsSelected = true;
                    ContactData cc = DataSupervisor.ds.Contactes[foundItem.Name];
                    contactCarteDetails.NomDuContact.Text = cc.name;
                    contactCarteDetails.numDeTelephone.Text = cc.numOfPhone;
                    try
                    {
                        contactCarteDetails.photoDeProfilDuContact.Source = new BitmapImage(new Uri(cc.pathPicture));
                    }
                    catch
                    {
                        contactCarteDetails.photoDeProfilDuContact.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"\images\photoProfilDefault.jpg"));
                    }
                    contactCarteDetails.siteWebContact.Text = cc.webSite;
                    contactCarteDetails.travail.Text = cc.profession;
                    contactCarteDetails.adresseDuContact.Text = cc.adress;
                    contactCarteDetails.emailContact.Text = cc.emailAdress;
                    contactCarteDetails.linkedinLink = cc.linkedInLink;
                    contactCarteDetails.facebookLink = cc.fbLink;
                    contactCarteDetails.twitterLink = cc.twitter;
                }
            }
        }
    }
}
