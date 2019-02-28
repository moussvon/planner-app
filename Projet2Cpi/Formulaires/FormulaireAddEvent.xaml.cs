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
using Microsoft.Win32;
using MaterialDesignThemes.Wpf;

namespace Projet2Cpi
{
    /// <summary>
    /// Logique d'interaction pour FormulaireAddEvent.xaml
    /// </summary>
    /// 
    public partial class FormulaireAddEvent : UserControl
    {

        static int cpt_files; // Ce compteur est pour compter le nombre de fichiers ajoutés
        private Dictionary<Chip, Notif> NotificationDict = new Dictionary<Chip, Notif>();

        OpenFileDialog file;
        public WindowFormulaires windowParent;
        public FormulaireAddEvent()
        {
            InitializeComponent();
            LoadActivities();
        }

        //Cette fonction, est pour importer des fichiers à l'évènement , en cliquant sur l icon "ImportFile"
        private void Importfile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Pdf Files (*.pdf)|*.pdf|Excel Files (*.xls)|*.xls|Word Files (*.docx)|*.docx|All files (*.*)|*.*";
            file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            file.Multiselect = true; //On peut selectionner multiple fichiers

            if (file.ShowDialog() == true)
            {

                //Si l'utilisatuer selectionne aucun fichier
                if (file.FileNames.Length == 0)
                {
                    cpt_files = 0;   //un compteur qui compte le nombre des fichiers selectionnés
                    AddfilesCard.Visibility = Visibility.Visible;
                    FilesChipsCard.Visibility = Visibility.Hidden;
                }
                //si il selectionne un ficher
                else if (file.FileNames.Length == 1)
                {
                    AddfilesCard.Visibility = Visibility.Hidden;    //On cache la carte d'ajout et on affiche la carte des chips
                    FilesChipsCard.Visibility = Visibility.Visible;

                    cpt_files = 1;   //un compteur qui compte le nombre des fichiers selectionnés
                    File1.Visibility = Visibility.Visible;          //On affiche le premiers CHIPS , le seul fichier selectionné
                    File1.Content = file.FileName;              //On nomme le chips, par le nom du fichier

                }
                // si il selectionne deux fichiers
                else if (file.FileNames.Length == 2)
                {
                    AddfilesCard.Visibility = Visibility.Hidden;
                    FilesChipsCard.Visibility = Visibility.Visible;

                    cpt_files = 2;   //un compteur qui compte le nombre des fichiers selectionnés
                    File1.Visibility = Visibility.Visible;
                    File2.Visibility = Visibility.Visible;

                    //On GET les noms des deux fichiers selectionnés
                    String[] MergedFile = new String[2];
                    int cpt = 0;
                    foreach (string filename in file.SafeFileNames)
                    {
                        MergedFile[cpt] = filename;
                        cpt++;
                    }

                    //On nomme les chips et leurs TOOLTIP par les noms des fichiers
                    File1.Content = MergedFile[0];
                    File1.ToolTip = MergedFile[0];
                    File2.Content = MergedFile[1];
                    File2.ToolTip = MergedFile[1];
                }
                // si il selectionne trois fichiers
                else if (file.FileNames.Length == 3)
                {
                    AddfilesCard.Visibility = Visibility.Hidden;
                    FilesChipsCard.Visibility = Visibility.Visible;

                    cpt_files = 3;   //un compteur qui compte le nombre des fichiers selectionnés
                    File1.Visibility = Visibility.Visible;  //On affiche tous les CHIPS 
                    File2.Visibility = Visibility.Visible;
                    File3.Visibility = Visibility.Visible;

                    //On GET les noms des deux fichiers selectionnés
                    String[] MergedFile = new String[3];
                    int cpt = 0;
                    foreach (string filename in file.SafeFileNames)
                    {
                        MergedFile[cpt] = filename;
                        cpt++;
                    }
                    //On nomme les chips et leurs TOOLTIP par les noms des fichiers
                    File1.Content = MergedFile[0];
                    File1.ToolTip = MergedFile[0];
                    File2.Content = MergedFile[1];
                    File2.ToolTip = MergedFile[1];
                    File3.Content = MergedFile[2];
                    File3.ToolTip = MergedFile[2];
                }
                // s'il selectionne plus de trois fichiers
                else if (file.FileNames.Length > 3)
                {
                    MessageBox.Show("Veuillez selectionner seulement trois fichiers");
                    cpt_files = 0;   //un compteur qui compte le nombre des fichiers selectionnés

                }
            }
        }



        //Cette fonction est pour l'ajout des contactes en tappant sur clavier
        //still needs to make the list of the contacts beggins with the first letter appear while typing
        private void CommentTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                int trouv = -1;
                foreach (KeyValuePair<string, ContactData> i in DataSupervisor.ds.Contactes.Data)
                {
                    if (i.Value.name == ((TextBox)sender).Text) trouv = 1;

                }
                if (trouv != -1)
                {

                    if (((TextBox)sender).Text.Length <= 0)
                    {
                        MessageBox.Show("Veuillez indiquer un nom valide");
                    }
                    else
                    {
                        Chip ch = new Chip();
                        ch.Height = 30;
                        ch.Width = 100;
                        Thickness margin = ch.Margin;
                        margin.Right += 5;
                        margin.Right = 0;
                        ch.Margin = margin;
                        ch.DeleteClick += new RoutedEventHandler(Chip_DeleteClick1);
                        ch.Content = ((TextBox)sender).Text;
                        ch.ToolTip = ((TextBox)sender).Text;
                        System.Windows.Media.Color color = (Color)System.Windows.Media.ColorConverter.ConvertFromString("#FF6866F7");
                        Brush br = new SolidColorBrush(color);
                        ch.IconBackground = br;
                        try
                        {
                            string ic = ((TextBox)sender).Text.ToCharArray()[0].ToString();
                            ch.Icon = ic.ToUpper();
                            ch.IsDeletable = true;
                            ((TextBox)sender).Text = "";
                            chips.Children.Add(ch);

                        }
                        catch (Exception)
                        {
                        }
                    }
                }
                else MessageBox.Show("Contact Inexistant");
            }

        }

        private void Chip_DeleteClick1(object sender, RoutedEventArgs e)
        {
            chips.Children.Remove((Chip)sender);
        }

        //Les trois fonctions ci dessous sont pour supprimer les Chips des fichiers selectionnés
        //En effet, pour le moment cettes fonctions manipulent seulement l affichage 
        //Je propose lors la création de l objet event , on vérifie d'abord les CHIPS qui sont VISIBLES,puis on enregistre leurs données 

        private void File1_DeleteClick(object sender, RoutedEventArgs e)
        {
            File1.Visibility = Visibility.Hidden;   //Si le premier fichiers est supprimée, on cache son CHIP , puis on modifie le MARGIN GAUCHE du CHIP qui le suit 
            Thickness margin = File2.Margin;
            margin.Left = -100;
            File2.Margin = margin;
            cpt_files -= 1;
            if (cpt_files == 0)         //Si tous les fichiers sont supprimés, on cache la carte des fichiers ajoutés et on affiche la carte d'ajout des fichiers 
            {
                AddfilesCard.Visibility = Visibility.Visible;
                FilesChipsCard.Visibility = Visibility.Hidden;
                margin.Left += 105;
                File3.Margin = margin;
                margin = File2.Margin;
                margin.Left += 105;
                File2.Margin = margin;
            }
        }

        private void File2_DeleteClick(object sender, RoutedEventArgs e)
        {
            File2.Visibility = Visibility.Hidden;
            Thickness margin = File3.Margin;
            margin.Left = -100;
            File3.Margin = margin;
            cpt_files -= 1;
            if (cpt_files == 0)
            {
                AddfilesCard.Visibility = Visibility.Visible;
                FilesChipsCard.Visibility = Visibility.Hidden;
                margin.Left += 105;
                File3.Margin = margin;
                margin = File2.Margin;
                margin.Left += 105;
                File2.Margin = margin;
            }
        }

        private void File3_DeleteClick(object sender, RoutedEventArgs e)
        {
            File3.Visibility = Visibility.Hidden;
            cpt_files -= 1;
            Thickness margin = File3.Margin;
            if (cpt_files == 0)
            {
                AddfilesCard.Visibility = Visibility.Visible;
                FilesChipsCard.Visibility = Visibility.Hidden;
                margin.Left += 105;
                File3.Margin = margin;
                margin = File2.Margin;
                margin.Left += 105;
                File2.Margin = margin;
            }
        }

        private void AddEvent(object sender, MouseButtonEventArgs e)
        {
            if (TextboxTitre.Text == "")
            {
                TextboxTitre.Focus();
            }
            else if (TextBoxLieu.Text == "")
            {
                TextBoxLieu.Focus();
            }
            else if (debutDatePicker.SelectedDate == null || finDatePicker.SelectedDate == null || Heure.SelectedTime == null)
            {
                MessageBox.Show("Veuillez choisir la date exacte");
                return;
            }
            else if ((ComboBoxPriorite.SelectedIndex != 0) && (ComboBoxPriorite.SelectedIndex != 1) && (ComboBoxPriorite.SelectedIndex != 2))
            {
                MessageBox.Show("Veuillez choisir la priorité");
            }
            else if (((string)((ComboBoxItem)ComboBoxActivities.SelectedItem).DataContext) == "Activité scolaire" && DataSupervisor.ds.user.JoursFeries.Keys.Contains(debutDatePicker.SelectedDate.Value))
            {
                MessageBox.Show("Le jour choisit est ferie");
            }
            else
            {
                Evenement ev = new Evenement();
                DateTime d = new DateTime(debutDatePicker.SelectedDate.Value.Year, debutDatePicker.SelectedDate.Value.Month, debutDatePicker.SelectedDate.Value.Day, Heure.SelectedTime.Value.Hour, Heure.SelectedTime.Value.Minute, Heure.SelectedTime.Value.Second);
                ev.DateDebut = d;
                ev.DateFin = finDatePicker.SelectedDate.Value;
                ev.Title = TextboxTitre.Text;
                ev.Description = TextBoxDescription.Text;
                ev.place = TextBoxLieu.Text;
                switch (cpt_files) {
                    case 1:
                        ev.Fichiers.Add((String)File1.Content);
                        break;
                    case 2:
                        ev.Fichiers.Add((String)File1.ContentStringFormat);
                        ev.Fichiers.Add((String)File2.ContentStringFormat);
                        break;
                    case 3:
                        ev.Fichiers.Add((String)File1.ContentStringFormat);
                        ev.Fichiers.Add((String)File2.ContentStringFormat);
                        ev.Fichiers.Add((String)File3.ContentStringFormat);
                        break;
                    default:
                        break;

                }
                switch (ComboBoxPriorite.SelectedIndex)
                {
                    case 0:
                        ev.Priority = "Urgente";
                        break;
                    case 1:
                        ev.Priority = "Normale";
                        break;
                    case 2:
                        ev.Priority = "Basse";
                        break;
                }
                ev.activite = (string)((ComboBoxItem)ComboBoxActivities.SelectedItem).DataContext;
                ev.Alarms = new List<Notif>();
                foreach (Notif n in NotificationDict.Values)
                {
                    ev.Alarms.Add(n);
                }
                DataSupervisor.ds.AddEvenement(ev);
                windowParent.Close();
            }
        }

        private void ButtonCancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            windowParent.Close();
        }

        private Chip CreateAlarmChip(String title)
        {
            Chip c = new Chip()
            {
                Content = title,
                IsDeletable = true,
                Margin = new Thickness(4),
            };
            c.DeleteClick += new RoutedEventHandler(Chip_DeleteClick);
            return c;
        }


        private void Chip_DeleteClick(object sender, RoutedEventArgs e)
        {
            alarmeChipsField.Children.Remove((Chip)sender);
            NotificationDict.Remove((Chip)sender);
            if (NotificationDict.Count < 3) AddAlarme1.IsEnabled = true;

        }
        private void AddAlarme(object sender, MouseButtonEventArgs e)
        {
            if (NotificationDict.Count == 3)
            {
                MessageBox.Show("trois notifications au max");
            }
            else
            {
                if (DatePickeralarm.SelectedDate == null || TimePickeralarm.SelectedTime == null || ComboBoxAlarmType.SelectedIndex == -1)
                {
                    MessageBox.Show("Veuillez remplir tous les champs");
                    return;
                }
                DateTime d = DatePickeralarm.SelectedDate.Value;
                DateTime t = TimePickeralarm.SelectedTime.Value;
                Notif n = new Notif()
                {
                    time = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0),
                    Type = ComboBoxAlarmType.SelectedIndex == 0 ? "email" : "sms",
                };
                Chip c = CreateAlarmChip($"{d.Year}-{d.Month}-{d.Date}-{t.Hour}:{t.Minute}");
                bool b = false;
                foreach (Notif notif in NotificationDict.Values)
                {
                    if (notif.time == n.time && notif.Type == n.Type)
                    {
                        b = true;
                        break;
                    }
                }

                if (!b)
                {

                    NotificationDict[c] = n;
                    alarmeChipsField.Children.Add(c);
                    if (NotificationDict.Count == 3) AddAlarme1.IsEnabled = false;
                }
                else
                {
                    MessageBox.Show("Alarme Dupliquée");
                }
            }
        }


        private static ComboBoxItem CreateActivityComboBox(string name, string color)
        {
            ComboBoxItem cbi = new ComboBoxItem()
            {
                Height = 30,
                DataContext = name
            };
            WrapPanel sp = new WrapPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };
            MaterialDesignThemes.Wpf.PackIcon icon = new PackIcon()
            {
                Kind = PackIconKind.CheckboxBlankCircle,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color)),
                Width = 15
            };
            TextBlock tb = new TextBlock()
            {
                Text = name,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextWrapping = TextWrapping.Wrap,
                Margin = new Thickness(5, 0, 0, 0),
                VerticalAlignment = VerticalAlignment.Center,
                FontSize = 11
            };
            sp.Children.Add(icon);
            sp.Children.Add(tb);
            cbi.Content = sp;
            return cbi;
        }

        private void LoadActivities()
        {
            foreach (KeyValuePair<string, string> kv in DataSupervisor.ds.user.Activities)
            {
                ComboBoxActivities.Items.Add(CreateActivityComboBox(kv.Key, kv.Value));
            }
        }
    }
}