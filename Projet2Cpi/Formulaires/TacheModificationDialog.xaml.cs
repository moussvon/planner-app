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
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;
using Microsoft.Win32;

namespace Projet2Cpi
{
    /// <summary>
    /// Interaction logic for TacheModificationDialog.xaml
    /// </summary>
    public partial class TacheModificationDialog : Window
    {
        private Dictionary<Chip, Notif> NotificationDict = new Dictionary<Chip, Notif>();

        public TacheModificationDialog()
        {
            InitializeComponent();
            this.LoadActivities();
        }

        private void Annuler(object sender, MouseButtonEventArgs e)
        {
            this.Close();
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
            if (NotificationDict.Count < 3) AddAlarme.IsEnabled = true;
        }

        private void AddAlarmeEvent(object sender, MouseButtonEventArgs e)
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
                if (NotificationDict.Count == 3) AddAlarme.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Alarme Dupliquée");
            }
        }

        private void ModifierTache(object sender, MouseButtonEventArgs e)
        {
            if (TextBoxTitle.Text == "")
            {
                TextBoxTitle.Focus();
            }
            else if (PickerDate.SelectedDate == null || PickerHeureDebut.SelectedTime == null || PickerHeureFin.SelectedTime == null)
            {
                MessageBox.Show("Veuillez choisir la date exacte");
                return;
            }
            else if ((ComboBoxPriorité.SelectedIndex != 0) && (ComboBoxPriorité.SelectedIndex != 1) && (ComboBoxPriorité.SelectedIndex != 2))
            {
                MessageBox.Show("Veuillez choisir la priorité");
            }
            else
            {
                DateTime? selectedDay = PickerDate.SelectedDate;
                DateTime dateDebut = new DateTime(selectedDay.Value.Year, selectedDay.Value.Month, selectedDay.Value.Day, PickerHeureDebut.SelectedTime.Value.Hour, PickerHeureDebut.SelectedTime.Value.Minute, PickerHeureDebut.SelectedTime.Value.Second);
                DateTime dateFin = new DateTime(selectedDay.Value.Year, selectedDay.Value.Month, selectedDay.Value.Day, PickerHeureFin.SelectedTime.Value.Hour, PickerHeureFin.SelectedTime.Value.Minute, PickerHeureFin.SelectedTime.Value.Second);
                t.dateDebut = dateDebut;
                t.dateFin = dateFin;
                foreach (String s in this.filesdict.Keys) t.Fichiers.Add(s);
                t.title = TextBoxTitle.Text;
                t.Details = TextBoxDescription.Text;
                switch (ComboBoxPriorité.SelectedIndex)
                {
                    case 0:
                        t.priorite = "Urgente";
                        break;
                    case 1:
                        t.priorite = "Normale";
                        break;
                    case 2:
                        t.priorite = "Basse";
                        break;
                    default:
                        return;
                }
                t.Activitee = (string)((ComboBoxItem)ComboBoxActivities.SelectedItem).DataContext;
                t.Alarms = new List<Notif>();
                foreach (KeyValuePair<Chip, Notif> n in NotificationDict)
                {
                    t.Alarms.Add(n.Value);
                }
                //DataSupervisor.ds.AddTache(t);
                this.Close();
                MainWindow.UpdateFields();
                DataSupervisor.ds.MonthData.StoreData();
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


        private Dictionary<string, Chip> filesdict = new Dictionary<string, Chip>();
        private void Importfile(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Pdf Files (*.pdf)|*.pdf|Excel Files (*.xls)|*.xls|Word Files (*.docx)|*.docx|All files (*.*)|*.*";
            file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            file.Multiselect = true;
            redo:
            if (file.ShowDialog() == true)
            {
                if (filesdict.Count + file.FileNames.Length > 3)
                {
                    MessageBox.Show("Vous devez selectionner trois fichiers au maximum");
                    goto redo;
                }
                foreach (string s in file.SafeFileNames)
                {
                    Chip c = new Chip()
                    {
                        Content = s,
                        IsDeletable = true
                    };
                    c.DeleteClick += new RoutedEventHandler(DeleteChip);
                    this.filesdict[s] = c;
                    filesStack.Children.Add(c);
                }
                if (filesStack.Children.Count > 0)
                {
                    AddfilesCard.Visibility = Visibility.Hidden;
                    FilesChipsCard.Visibility = Visibility.Visible;
                }
                
            }
        }

        private void DeleteChip(object sender, RoutedEventArgs e)
        {
            Chip c = (Chip)sender;
            this.filesdict.Remove((string)c.Content);
            filesStack.Children.Remove(c);
        }

        public Tache t;
        public void FillWithTache(Tache t)
        {
            this.t = t;
            TextBoxTitle.Text = t.title;
            TextBoxDescription.Text = t.Details;
            PickerDate.SelectedDate = t.dateDebut;
            PickerHeureDebut.SelectedTime = t.dateDebut;
            PickerHeureFin.SelectedTime = t.dateFin;
            int index = -1;
            foreach (ComboBoxItem cbi in ComboBoxActivities.Items)
            {
                if ((string)cbi.DataContext == t.Activitee)
                {
                    index = ComboBoxActivities.Items.IndexOf(cbi);
                    break;
                }
            }
            ComboBoxActivities.SelectedIndex = index;
            ComboBoxPriorité.SelectedIndex = t.priorite == "Urgente" ? 0 : t.priorite == "Normale" ? 1 : 2;
            foreach (Notif n in t.Alarms)
            {
                DateTime d = n.time;
                Chip c = CreateAlarmChip($"{d.Year}-{d.Month}-{d.Date}-{d.Hour}:{d.Minute}");
                NotificationDict[c] = n;
                alarmeChipsField.Children.Add(c);
            }
        }

        private void ComboBoxPriorité_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }
    }
}
