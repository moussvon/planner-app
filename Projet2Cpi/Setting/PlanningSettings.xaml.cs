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
using MaterialDesignThemes.Wpf;

namespace Projet2Cpi
{
    /// <summary>
    /// Interaction logic for PlanningSetting.xaml
    /// </summary>
    public partial class PlanningSettings : UserControl
    {
        TextBox[] descriptionBoxes = new TextBox[7];
        TextBox[] titlesBoxes = new TextBox[7];
        ComboBox[] priorityCBs = new ComboBox[7];
        TimePicker[] beginPickers = new TimePicker[7];
        TimePicker[] endPickers = new TimePicker[7];

        public PlanningSettings()
        {
            InitializeComponent();
            this.UpdateSeancesFields();
            this.LoadPlanningFieleds();
            this.LoadJoursFeries();
            daySelect.SelectedIndex = 0;
        }

        public void LoadSeancesFieldsVisuals()
        {
            for (int i = 0; i < 7; ++i)
            {
                StackPanel sp = new StackPanel()
                {
                    VerticalAlignment = VerticalAlignment.Center,
                    Orientation = Orientation.Horizontal,
                    Margin = new Thickness(20, 0, 0, 0)
                };
                Grid.SetRow(sp, i + 2);
                TextBlock t = new TextBlock()
                {
                    Text = $"Séance {i + 1}:",
                    FontSize = 16,
                    FontWeight = FontWeights.Medium,
                    Margin = new Thickness(20, 0, 0, 0),
                    Height = 25,
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Width = 70
                };
                sp.Children.Add(t);
                titlesBoxes[i] = new TextBox()
                {
                    Margin = new Thickness(10, -20, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 35,
                    TextWrapping = TextWrapping.Wrap,
                    Width = 150,
                    CaretBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6866F7")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6866F7"))
                };
                HintAssist.SetHint(titlesBoxes[i], "Nom de la séance");
                sp.Children.Add(titlesBoxes[i]);
                descriptionBoxes[i] = new TextBox()
                {
                    Margin = new Thickness(10, -20, 0, 0),
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Height = 35,
                    TextWrapping = TextWrapping.Wrap,
                    Width = 200,
                    CaretBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6866F7")),
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6866F7"))
                };
                HintAssist.SetHint(descriptionBoxes[i], "Description de la séance");
                sp.Children.Add(descriptionBoxes[i]);
                beginPickers[i] = new TimePicker()
                {
                    Margin = new Thickness(10, -5, 0, 0),
                    Width = 100,
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6866F7"))
                };
                HintAssist.SetHint(beginPickers[i], "Début :");
                sp.Children.Add(beginPickers[i]);
                endPickers[i] = new TimePicker()
                {
                    Margin = new Thickness(10, -5, 0, 0),
                    Width = 100,
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6866F7"))
                };
                HintAssist.SetHint(endPickers[i], "Fin :");
                sp.Children.Add(endPickers[i]);
                this.seancesFields.Children.Insert(this.seancesFields.Children.Count - 1, sp);
                priorityCBs[i] = new ComboBox()
                {
                    Margin = new Thickness(10, -15, 0, 0),
                    Width = 100,
                    BorderBrush = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#FF6866F7"))
                };
                HintAssist.SetHint(priorityCBs[i], "Priorité");
                ComboBoxItem cbi1 = new ComboBoxItem()
                {
                    Height = 30,
                    DataContext = "Urgente"
                };
                ComboBoxItem cbi2 = new ComboBoxItem()
                {
                    Height = 30,
                    DataContext = "Normale"
                };
                ComboBoxItem cbi3 = new ComboBoxItem()
                {
                    Height = 30,
                    DataContext = "Basse"
                };
                WrapPanel wp1 = new WrapPanel();
                WrapPanel wp2 = new WrapPanel();
                WrapPanel wp3 = new WrapPanel();
                wp1.Children.Add(new PackIcon()
                {
                    Kind = PackIconKind.CheckboxBlankCircle,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDFF0707")),
                    Width = 10
                });
                wp2.Children.Add(new PackIcon()
                {
                    Kind = PackIconKind.CheckboxBlankCircle,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DDF4FF07")),
                    Width = 10
                });
                wp3.Children.Add(new PackIcon()
                {
                    Kind = PackIconKind.CheckboxBlankCircle,
                    Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#DD64CEFF")),
                    Width = 10
                });
                wp1.Children.Add(new TextBlock()
                {
                    Text = "Urgente",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(5, 0, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 12
                });
                wp2.Children.Add(new TextBlock()
                {
                    Text = "Normale",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(5, 0, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 12
                });
                wp3.Children.Add(new TextBlock()
                {
                    Text = "Basse",
                    HorizontalAlignment = HorizontalAlignment.Left,
                    Margin = new Thickness(5, 0, 0, 0),
                    TextWrapping = TextWrapping.Wrap,
                    FontSize = 12
                });
                cbi1.Content = wp1;
                cbi2.Content = wp2;
                cbi3.Content = wp3;
                cbi1.DataContext = "Urgente";
                cbi2.DataContext = "Normale";
                cbi3.DataContext = "Basse";
                priorityCBs[i].Items.Add(cbi1);
                priorityCBs[i].Items.Add(cbi2);
                priorityCBs[i].Items.Add(cbi3);
                sp.Children.Add(priorityCBs[i]);
            }
            dimanche.DataContext = DayOfWeek.Sunday;
            lundi.DataContext = DayOfWeek.Monday;
            mardi.DataContext = DayOfWeek.Tuesday;
            mercredi.DataContext = DayOfWeek.Wednesday;
            jeudi.DataContext = DayOfWeek.Thursday;
        }

        public void UpdateSeancesFields()
        {
            this.LoadSeancesFieldsVisuals();
        }
        private void RetainSeances()
        {
            for (int i = 0; i < 7; ++i)
            {
                if (titlesBoxes[i].Text == "" || priorityCBs[i].SelectedIndex == -1 || beginPickers[i].SelectedTime == null || endPickers[i].SelectedTime == null) continue;
                Seance seance = new Seance()
                {
                    name = titlesBoxes[i].Text,
                    description = descriptionBoxes[i].Text,
                    priority = (string)((ComboBoxItem)priorityCBs[i].SelectedItem).DataContext,
                    begin = beginPickers[i].SelectedTime.Value,
                    end = endPickers[i].SelectedTime.Value
                };
                DataSupervisor.ds.SetSeance((DayOfWeek)((ComboBoxItem)daySelect.SelectedItem).DataContext, i + 1, seance);
            }
        }

        public void Next(object sender, RoutedEventArgs e)
        {
            this.daySelect.SelectedIndex = (this.daySelect.SelectedIndex + 1) % 5;

        }
        public void Last(object sender, RoutedEventArgs e)
        {
            if (this.daySelect.SelectedIndex == -1)
            {
                this.daySelect.SelectedIndex = 4;
                return;
            }
            this.daySelect.SelectedIndex = this.daySelect.SelectedIndex == 0 ? 4 : this.daySelect.SelectedIndex - 1;
        }
        private DayOfWeek GetCurrentDay()
        {
            switch (this.daySelect.SelectedIndex)
            {
                case 0:
                    return DayOfWeek.Sunday;
                case 1:
                    return DayOfWeek.Monday;
                case 2:
                    return DayOfWeek.Tuesday;
                case 3:
                    return DayOfWeek.Wednesday;
                case 4:
                    return DayOfWeek.Thursday;
            }
            throw new Exception("Invalid week day selection");
        }

        private void ModifySeancesBoxes(int i, Seance s)
        {
            if (s == null)
            {
                descriptionBoxes[i].Text = "";
                titlesBoxes[i].Text = "";
                priorityCBs[i].SelectedIndex = -1;
                beginPickers[i].SelectedTime = null;
                endPickers[i].SelectedTime = null;
                return;
            }
            descriptionBoxes[i].Text = s.description;
            titlesBoxes[i].Text = s.name;
            switch (s.priority)
            {
                case "Urgente":
                    priorityCBs[i].SelectedIndex = 0;
                    break;
                case "Normale":
                    priorityCBs[i].SelectedIndex = 1;
                    break;
                case "Basse":
                    priorityCBs[i].SelectedIndex = 2;
                    break;
            }
            beginPickers[i].SelectedTime = s.begin;
            endPickers[i].SelectedTime = s.end;
        }

        private void daySelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (daySelect.SelectedIndex == -1) return;
            Dictionary<int, Seance> seancesd = DataSupervisor.ds.GetJourneeSeances((DayOfWeek)((ComboBoxItem)daySelect.SelectedItem).DataContext);
            if (seancesd == null)
            {
                for (int i = 0; i < 7; ++i) this.ModifySeancesBoxes(i, null);
                return;
            }
            foreach (KeyValuePair<int, Seance> kv in seancesd)
            {
                this.ModifySeancesBoxes(kv.Key - 1, kv.Value);
            }
        }

        private void SaveButton(object sender, MouseButtonEventArgs e)
        {
            this.RetainSeances();
            MainWindow.UpdateFields();
        }

        private void LoadPlanningFieleds()
        {
            Tuple<DateTime?, DateTime?> p1 = DataSupervisor.ds.user.PlanningPeriod1;
            Tuple<DateTime?, DateTime?> p2 = DataSupervisor.ds.user.PlanningPeriod2;
            Tuple<DateTime?, DateTime?> ci = DataSupervisor.ds.user.ControlesInt;
            Tuple<DateTime?, DateTime?> ef = DataSupervisor.ds.user.ExmensFinals;
            if (p1 != null)
            {
                this.PickerDatePeriodeonePremierjour.SelectedDate = p1.Item1;
                this.PickerDatePeriodeoneDernierjour.SelectedDate = p1.Item2;
            }
            if (p2 != null)
            {
                this.PickerDatePeriodetwoPremierjour.SelectedDate = p2.Item1;
                this.PickerDatePeriodetwoDernierjour.SelectedDate = p2.Item2;
            }
            if (ci != null)
            {
                this.PickerDateControleinterPremierjour.SelectedDate = ci.Item1;
                this.PickerDateControleinterDernierjour.SelectedDate = ci.Item2;
            }
            if (ef != null)
            {
                this.PickerDateExamenSemestPremierjour.SelectedDate = ef.Item1;
                this.PickerDateExamenSemestDernierjour.SelectedDate = ef.Item2;
            }
        }

        private void LoadJoursFeries()
        {
            this.joursFerComboBox.Items.Clear();
            foreach (KeyValuePair<DateTime, string> kv in DataSupervisor.ds.GetJoursFeries())
            {
                ComboBoxItem tb = new ComboBoxItem()
                {
                    Content = kv.Value,
                    Height = 36,
                    HorizontalAlignment = HorizontalAlignment.Stretch,
                    DataContext = kv.Key
                };
                this.joursFerComboBox.Items.Add(tb);
            }
        }

        private void RegisterPlanningPeriods(object sender, MouseButtonEventArgs e)
        {
            DateTime? sem1b = this.PickerDatePeriodeonePremierjour.SelectedDate;
            DateTime? sem1e = this.PickerDatePeriodeoneDernierjour.SelectedDate;
            DateTime? sem2b = this.PickerDatePeriodetwoPremierjour.SelectedDate;
            DateTime? sem2e = this.PickerDatePeriodetwoDernierjour.SelectedDate;
            if (sem1b == null || sem1e == null || sem2b == null || sem2e == null)
            {
                MessageBox.Show("Vous devez selectionner les 2 périodes semestrielles");
                return;
            }
            if (sem1b > sem1e || sem2b > sem2e)
            {
                MessageBox.Show("Les bornes des périodes semesteriels doivent être justes");
                return;
            }
            DateTime? cib = this.PickerDateControleinterPremierjour.SelectedDate;
            DateTime? cie = this.PickerDateControleinterDernierjour.SelectedDate;
            if (cib != null && cie != null && cib > cie)
            {
                MessageBox.Show("Les bornes des examens intermédiaires doivent être justes");
                return;
            }
            DateTime? efb = this.PickerDateExamenSemestPremierjour.SelectedDate;
            DateTime? efe = this.PickerDateExamenSemestDernierjour.SelectedDate;
            if (efb != null && efe != null && efb > efe)
            {
                MessageBox.Show("Les bornes des examens finales doivent être justes");
                return;
            }
            Tuple<DateTime?, DateTime?> t1 = new Tuple<DateTime?, DateTime?>(sem1b, sem1e);
            Tuple<DateTime?, DateTime?> t2 = new Tuple<DateTime?, DateTime?>(sem2b, sem2e);
            DataSupervisor.ds.user.PlanningPeriod1 = t1;
            DataSupervisor.ds.user.PlanningPeriod2 = t2;
            DataSupervisor.ds.user.ControlesInt = new Tuple<DateTime?, DateTime?>(cib, cie);
            DataSupervisor.ds.user.ExmensFinals = new Tuple<DateTime?, DateTime?>(efb, efe);
            MainWindow.UpdateFields();
        }

        private void AddJourFerie(object sender, MouseButtonEventArgs e)
        {
            string title = this.TextBoxAjouterjourferie.Text;
            if (this.PickerDateJourferie.SelectedDate == null || title == "") return;
            DateTime day = this.PickerDateJourferie.SelectedDate.Value;
            DataSupervisor.ds.AddJourFerie(day, title);
            int k = this.joursFerComboBox.SelectedIndex;
            this.LoadJoursFeries();
            this.joursFerComboBox.SelectedIndex = k;
            MainWindow.UpdateFields();
        }

        private void DelJourFerie(object sender, MouseButtonEventArgs e)
        {
            if (this.joursFerComboBox.SelectedIndex == -1) return;
            DataSupervisor.ds.DeleteJourFerie((DateTime)((ComboBoxItem)this.joursFerComboBox.SelectedItem).DataContext);
            this.LoadJoursFeries();
            MainWindow.UpdateFields();
        }
    }
}
