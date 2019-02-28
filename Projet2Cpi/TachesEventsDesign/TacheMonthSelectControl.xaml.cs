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
    /// Interaction logic for MonthSelectControl.xaml
    /// </summary>
    public partial class TacheMonthSelectControl : UserControl
    {
        public int Month;
        public int Year;
        public int MaxYear;
        public int MinYear = 2016;
        public bool jourActive = true;
        private bool selectionChangedEnabled = true;
        public int selectedDay = DateTime.Now.Day;
        public int selectedWeek = CurrentWeek();
        public TacheMonthSelectControl()
        {
            InitializeComponent();
            DateTime today = DateTime.Now;
            this.Month = today.Month;
            this.Year = today.Year;
            this.MaxYear = today.Year + 10;
            for (int i = this.MinYear; i < today.Year + 10; ++i)
            {
                ComboBoxItem ci = new ComboBoxItem()
                {
                    Content = $"{i}",
                    Height = 36,
                    HorizontalContentAlignment = HorizontalAlignment.Center
                };
                yearSelect.Items.Add(ci);
            }
            monthSelect.SelectedIndex = today.Month - 1;
            yearSelect.SelectedIndex = today.Year - 2016;
            yearSelect.SelectionChanged += new SelectionChangedEventHandler(Changed);
            monthSelect.SelectionChanged += new SelectionChangedEventHandler(Changed);
            this.ChangeToJours();
            this.SemaineJourSelect.SelectedIndex = selectedDay - 1;
        }

        private void ReloadCBs()
        {
            if (jourActive)
            {
                SemaineJourSelect.Items.Clear();
                int n = DateTime.DaysInMonth(Year, Month);
                for (int i = 1; i <= n; ++i)
                {
                    ComboBoxItem cbi = new ComboBoxItem()
                    {
                        Content = $"Jour {i}"
                    };
                    SemaineJourSelect.Items.Add(cbi);
                }
                this.SemaineJourSelect.SelectedIndex = selectedDay - 1;
                this.monthSelect.SelectedIndex = Month - 1;
                this.yearSelect.SelectedIndex = Year - MinYear;
            } else
            {
                SemaineJourSelect.Items.Clear();
                for (int i = 1; i <= AffichageParSemaineControl.DivideToWeeks(Month, Year).Values.Count; ++i)
                {
                    ComboBoxItem cbi = new ComboBoxItem()
                    {
                        Content = $"Semaine {i}"
                    };
                    SemaineJourSelect.Items.Add(cbi);
                }
                SemaineJourSelect.SelectedIndex = selectedWeek - 1;
                this.monthSelect.SelectedIndex = Month - 1;
                this.yearSelect.SelectedIndex = Year - MinYear;
            }

        }

        private void Next(object sender, MouseEventArgs e)
        {
            this.selectionChangedEnabled = false;
            if (this.jourActive)
            {
                if (selectedDay == this.SemaineJourSelect.Items.Count)
                {
                    if (Month == 12)
                    {
                        if (Year != yearSelect.Items.Count - 1)
                        {
                            this.Year += 1;
                            this.Month = 1;
                            this.selectedDay = 1;
                        }
                    } else
                    {
                        this.selectedDay = 1;
                    }
                } else
                {
                    this.selectedDay += 1;
                }
            } else
            {
                if (selectedWeek == this.SemaineJourSelect.Items.Count)
                {
                    if (Month == 12)
                    {
                        if (Year != this.yearSelect.SelectedIndex + MaxYear)
                        {
                            Year += 1;
                            Month = 1;
                            selectedWeek = 1;
                        }
                    } else
                    {
                        Month += 1;
                        selectedWeek = 1;
                    }
                } else
                {
                    selectedWeek += 1;
                }
            }
            this.ReloadCBs();
            this.selectionChangedEnabled = true;
            MainWindow.TachesEventsField.Update();
        }

        private void Last(object sender, MouseEventArgs e)
        {
            this.selectionChangedEnabled = false;
            if (this.jourActive)
            {
                if (selectedDay == 1)
                {
                    if (this.Month == 1)
                    {
                        if (this.yearSelect.SelectedIndex != 0)
                        {
                            this.Year -= 1;
                            this.Month = 1;
                            this.selectedDay = 1;
                        }
                    } else
                    {
                        this.Month -= 1;
                        this.selectedDay = 1;
                    }
                } else
                {
                    selectedDay -= 1;
                }
            }
            else
            {
                if (selectedWeek == 1)
                {
                    if (Month == 1)
                    {
                        if (Year != MinYear)
                        {
                            Year -= 1;
                            Month = 1;
                            selectedWeek = 1;
                        }
                    } else
                    {
                        Month -= 1;
                        selectedWeek = AffichageParSemaineControl.DivideToWeeks(Month, Year).Values.Count;
                    }
                } else
                {
                    selectedWeek -= 1;
                }
            }
            this.selectionChangedEnabled = true;
            this.ReloadCBs();
            MainWindow.TachesEventsField.Update();
        }

        private void Changed(object sender, RoutedEventArgs e)
        {
            if (!selectionChangedEnabled) return;
            this.Month = monthSelect.SelectedIndex + 1;
            this.Year = this.MinYear + yearSelect.SelectedIndex;
            if (jourActive)
            {
                selectedDay = this.SemaineJourSelect.SelectedIndex + 1;
            } else
            {

            }
            MainWindow.TachesEventsField.Update();
        }
        
        public void ChangeToJours()
        {
            updatingVisuals = true;
            ComboBox cb = this.SemaineJourSelect;
            jourActive = true;
            cb.Items.Clear();
            for (int i = 1; i <= DateTime.DaysInMonth(Year, Month); ++i)
            {
                ComboBoxItem cbi = new ComboBoxItem()
                {
                    Content = $"Jour {i}",
                    DataContext = i
                };
                cbi.Selected += new RoutedEventHandler(SelectDay);
                cb.Items.Add(cbi);
            }
            cb.SelectedIndex = selectedDay - 1;
            updatingVisuals = false;
        }

        public void SelectDay(object sender, RoutedEventArgs e)
        {
            if (updatingVisuals) return;
            this.selectedDay = (int)((ComboBoxItem)sender).DataContext;
            if (MainWindow.TachesEventsField != null) MainWindow.TachesEventsField.Update();
        }

        static private int CurrentWeek()
        {
            int day = DateTime.Now.Day;
            DateTime d = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            int i = 1;
            while (d.Day != day)
            {
                if ((int)d.DayOfWeek == 0) i += 1;
                d = d.AddDays(1);
            }
            return i;
        }
        private bool updatingVisuals = false;
        public void ChangeToSemaines()
        {
            updatingVisuals = true;
            jourActive = false;
            ComboBox cb = this.SemaineJourSelect;
            cb.Items.Clear();
            int weeksCount = AffichageParSemaineControl.DivideToWeeks(Month, Year).Values.Count;
            for (int i = 1; i <= weeksCount; ++i)
            {
                ComboBoxItem cbi = new ComboBoxItem()
                {
                    Content = $"Semaine {i}",
                    DataContext = i
                };
                cbi.Selected += new RoutedEventHandler(SelectWeek);
                cb.Items.Add(cbi);
            }
            cb.SelectedIndex = selectedWeek - 1;
            updatingVisuals = false;
        }

        public void SelectWeek(object sender, RoutedEventArgs e)
        {
            if (updatingVisuals) return;
            this.selectedDay = (int)((ComboBoxItem)sender).DataContext;
            MainWindow.TachesEventsField.Update();
        }
    }
}
