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
using Microsoft.Win32;
using MaterialDesignThemes.Wpf;

namespace Projet2Cpi
{
    /// <summary>
    /// Interaction logic for EventModificationDialog.xaml
    /// </summary>
    public partial class EventModificationDialog : Window
    {
        private Dictionary<Chip, Notif> NotificationDict = new Dictionary<Chip, Notif>();

        public EventModificationDialog()
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
            if (NotificationDict.Count < 3) addAlarm.IsEnabled = true;
        }

        private void AddAlarme(object sender, MouseButtonEventArgs e)
        {
            if (DatePickeralarm.SelectedDate == null || TimePickeralarm.SelectedTime == null || ComboBoxAlarmType.SelectedIndex == -1)
            {
                MessageBox.Show("Veuillez remplir tous les champs ");
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
                if (NotificationDict.Count == 3) addAlarm.IsEnabled = false;
            }
            else
            {
                MessageBox.Show("Alarme Dupliquée");
            }
        }

        private void ModifierEvent(object sender, MouseButtonEventArgs e)
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
            else
            {
                DateTime dt = debutDatePicker.SelectedDate.Value;
                DateTime tm = Heure.SelectedTime.Value;
                t.DateDebut = new DateTime(dt.Year, dt.Month, dt.Day, tm.Hour, tm.Minute, tm.Second);
                t.DateFin = finDatePicker.SelectedDate.Value;
                t.Title = TextboxTitre.Text;
                t.Description = TextBoxDescription.Text;
                switch (ComboBoxPriorite.SelectedIndex)
                {
                    case 0:
                        t.Priority = "Urgente";
                        break;
                    case 1:
                        t.Priority = "Normale";
                        break;
                    case 2:
                        t.Priority = "Basse";
                        break;
                }
                t.activite = (string)((ComboBoxItem)ComboBoxActivities.SelectedItem).DataContext;
                t.Alarms = new List<Notif>();
                foreach (Notif n in NotificationDict.Values)
                {
                    t.Alarms.Add(n);
                }
                DataSupervisor.ds.MonthData.StoreData();
                MainWindow.UpdateFields();
                this.Close();
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
                    MessageBox.Show("Vous devez selectionner 3 fichiers au maximum");
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

        public Evenement t;
        public void FillWithEvent(Evenement t)
        {
            this.t = t;
            TextboxTitre.Text = t.Title;
            TextBoxLieu.Text = t.place;
            TextBoxDescription.Text = t.Description;
            debutDatePicker.SelectedDate = t.DateDebut;
            finDatePicker.SelectedDate = t.DateFin;
            Heure.SelectedTime = t.DateDebut;
            int index = -1;
            foreach (ComboBoxItem cbi in ComboBoxActivities.Items)
            {
                if ((string)cbi.DataContext == t.activite)
                {
                    index = ComboBoxActivities.Items.IndexOf(cbi);
                    break;
                }
            }
            ComboBoxActivities.SelectedIndex = index;
            ComboBoxPriorite.SelectedIndex = t.Priority == "Urgente" ? 0 : t.Priority == "Normale" ? 1 : 2;
            foreach (Notif n in t.Alarms)
            {
                DateTime d = n.time;
                Chip c = CreateAlarmChip($"{d.Year}-{d.Month}-{d.Date}-{d.Hour}:{d.Minute}");
                NotificationDict[c] = n;
                alarmeChipsField.Children.Add(c);
            }
        }

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

        private void Card_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }
    }
}
