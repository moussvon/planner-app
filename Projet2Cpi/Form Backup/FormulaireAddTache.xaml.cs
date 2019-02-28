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
    /// Logique d'interaction pour FormulaireAddTache.xaml
    /// </summary>
    public partial class FormulaireAddTache : UserControl
    {
        public WindowFormulaires windowParent;

        private int cpt_files = 0;
        private List<String> files = new List<string>();
        private Dictionary<Chip, Notification> NotificationDict = new Dictionary<Chip, Notification>();
        //Define a class of activity

        public FormulaireAddTache()
        {
            InitializeComponent();
            this.LoadActivities();
            ComboBoxActivities.SelectedItem = new TextBox() { Text = "123" };
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
        }

        private void AddAlarmeEvent(object sender, MouseButtonEventArgs e)
        {
            if (DatePickeralarm.SelectedDate == null || TimePickeralarm.SelectedTime == null || ComboBoxAlarmType.SelectedIndex == -1)
            {
                MessageBox.Show("Please fill in all fields");
                return;
            }
            DateTime d = DatePickeralarm.SelectedDate.Value;
            DateTime t = TimePickeralarm.SelectedTime.Value;
            Notification n = new Notification()
            {
                time = new DateTime(d.Year, d.Month, d.Day, t.Hour, t.Minute, 0),
                type = ComboBoxAlarmType.SelectedIndex == 1 ? "Email" : "SMS",
                NotificatonMsg = "${tache/event dat}"
            };
            Chip c = CreateAlarmChip($"{d.Year}-{d.Month}-{d.Date}-{t.Hour}:{t.Minute}");
            NotificationDict[c] = n;
            alarmeChipsField.Children.Add(c);
        }

        private void ButtonAddTask_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Tache t = new Tache();
            DateTime? selectedDay = PickerDate.SelectedDate;
            if (selectedDay == null || PickerHeureDebut.SelectedTime == null || PickerHeureFin.SelectedTime == null)
            {
                MessageBox.Show("Please select a day");
                return;
            }
            DateTime dateDebut = new DateTime(selectedDay.Value.Year, selectedDay.Value.Month, selectedDay.Value.Day, PickerHeureDebut.SelectedTime.Value.Hour, PickerHeureDebut.SelectedTime.Value.Minute, PickerHeureDebut.SelectedTime.Value.Second);
            DateTime dateFin = new DateTime(selectedDay.Value.Year, selectedDay.Value.Month, selectedDay.Value.Day, PickerHeureFin.SelectedTime.Value.Hour, PickerHeureFin.SelectedTime.Value.Minute, PickerHeureFin.SelectedTime.Value.Second);
            t.dateDebut = dateDebut;
            t.dateFin = dateFin;
            foreach (String s in this.files) t.Fichiers.Add(s);
            t.title = TextBoxTitle.Text;
            t.designation = TextBoxDescription.Text;
            switch (ComboBoxPriorité.SelectedIndex)
            {
                case 0:
                    t.priorite = "Urgent";
                    break;
                case 1:
                    t.priorite = "Normal";
                    break;
                case 2:
                    t.priorite = "Bas";
                    break;
                default:
                    MessageBox.Show("Veuillez choisir la degree d'urgence !");
                    return;
                    break;
            }
            t.Activitee = ComboBoxActivities.SelectedIndex != -1 ? (string)((ComboBoxItem)ComboBoxActivities.SelectedItem).DataContext : "Not specified";
            /*switch (ComboBoxEtatdutache.SelectedIndex)
            {
                case 1:
                    t.etat = "Non réalisé";
                    break;
                case 2:
                    t.etat = "En cours";
                    break;
                case 3:
                    t.etat = "Terminé";
                    break;
                default:
                    MessageBox.Show("Veuillez choisir la degree d'urgence !");
                    return;
                    break;
            }*/
            // TODO : include Alarms in Tache
            DataSupervisor.ds.AddTache(t);
            windowParent.Close();
        }
        
        // Files :
        private void Importfile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Pdf Files (*.pdf)|*.pdf|Excel Files (*.xls)|*.xls|Word Files (*.docx)|*.docx|All files (*.*)|*.*";
            file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            file.Multiselect = true; //On peut selectionner multiple fichiers

            if ( file.ShowDialog() == true)
            {         
                
                //Si l'utilisatuer selectionne aucun fichier
                if ( file.FileNames.Length == 0)
                {
                    cpt_files = 0;   //un compteur qui compte le nombre des fichiers selectionnés
                    files.Clear();
                    AddfilesCard.Visibility = Visibility.Visible;
                    FilesChipsCard.Visibility = Visibility.Hidden;
                }
                //si il selectionne un ficher
                else if (file.FileNames.Length == 1)
                {
                    AddfilesCard.Visibility = Visibility.Hidden;
                    FilesChipsCard.Visibility = Visibility.Visible;


                    cpt_files = 1;   //un compteur qui compte le nombre des fichiers selectionnés
                    File1.Visibility = Visibility.Visible;
                    File1.Content = file.SafeFileNames[0];
                    files.Add(file.SafeFileNames[0]);
                    Thickness margin = WrapPanelAfterfiles.Margin;
                    margin.Top = -100;
                    WrapPanelAfterfiles.Margin = margin;
                }
                // si il selectionne deux fichiers
                else if (file.FileNames.Length == 2)
                {
                    AddfilesCard.Visibility = Visibility.Hidden;
                    FilesChipsCard.Visibility = Visibility.Visible;

                    cpt_files = 2;   //un compteur qui compte le nombre des fichiers selectionnés
                    File1.Visibility = Visibility.Visible;
                    File2.Visibility = Visibility.Visible;
                    files.Clear();
                    foreach (string filename in file.SafeFileNames) files.Add(filename);
                    File1.Content = files[0];
                    File1.ToolTip = files[0];
                    File2.Content = files[1];
                    File2.ToolTip = files[1];
                }
                // si il selectionne trois fichiers
                else if (file.FileNames.Length == 3)
                {
                    AddfilesCard.Visibility = Visibility.Hidden;
                    FilesChipsCard.Visibility = Visibility.Visible;

                    cpt_files = 3;   //un compteur qui compte le nombre des fichiers selectionnés
                    File1.Visibility = Visibility.Visible;
                    File2.Visibility = Visibility.Visible;
                    File3.Visibility = Visibility.Visible;
                    files.Clear();
                    foreach (string filename in file.SafeFileNames) files.Add(filename);
                    File1.Content = files[0];
                    File1.ToolTip = files[0];
                    File2.Content = files[1];
                    File2.ToolTip = files[1];
                    File3.Content = files[2];
                    File3.ToolTip = files[2];
                }
                // s'il selectionne plus de trois fichiers
                else if (file.FileNames.Length > 3)
                {
                    MessageBox.Show("You have to select just 3 files");
                    cpt_files = 0;   //un compteur qui compte le nombre des fichiers selectionnés
                  
                }
                
            }

        }
         
        //Si l'utilisateur veut supprimer qlqs fichiers
       
        private void File1_DeleteClick(object sender, RoutedEventArgs e)
        {
           
            File1.Visibility = Visibility.Hidden;
            Thickness margin = File2.Margin;
            margin.Left = -100;
            File2.Margin = margin;
            cpt_files -= 1;           
            if (cpt_files == 0)
            {
                AddfilesCard.Visibility = Visibility.Visible;
                FilesChipsCard.Visibility = Visibility.Hidden;
                margin.Left += 105;
                File3.Margin = margin;
                margin = File2.Margin;
                margin.Left +=105;
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
