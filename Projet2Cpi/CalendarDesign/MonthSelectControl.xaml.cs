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
    public partial class MonthSelectControl : UserControl
    {
        public int Month;
        public int Year;
        public int MaxYear;
        public int MinYear = 2016;
        private bool selectionChangedEnabled = true;
        public MonthSelectControl()
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
            yearSelect.SelectionChanged += new SelectionChangedEventHandler(MonthChanged);
            monthSelect.SelectionChanged += new SelectionChangedEventHandler(MonthChanged);
        }

        private void ChangeMonthVisial()
        {
            monthSelect.SelectedIndex = Month - 1;
            yearSelect.SelectedIndex = Year - 2016;
        }

        private void NextMonth(object sender, MouseEventArgs e)
        {
            this.selectionChangedEnabled = false;
            if (this.Month == 12)
            {
                if (this.Year < this.MaxYear)
                {
                    this.Year += 1;
                    this.Month = 1;
                }
            }
            else Month += 1;
            this.ChangeMonthVisial();
            MainWindow.CalendarField.UpdateCalendar();
            this.selectionChangedEnabled = true;
        }

        private void LastMonth(object sender, MouseEventArgs e)
        {
            this.selectionChangedEnabled = false;
            if (this.Month == 1)
            {
                if (this.Year > this.MinYear)
                {
                    this.Year -= 1;
                    this.Month = 12;
                }
            }
            else Month -= 1;
            this.ChangeMonthVisial();
            MainWindow.CalendarField.UpdateCalendar();
            this.selectionChangedEnabled = true;
        }

        private void MonthChanged(object sender, RoutedEventArgs e)
        {
            if (!selectionChangedEnabled) return;
            this.Month = monthSelect.SelectedIndex + 1;
            this.Year = this.MinYear + yearSelect.SelectedIndex;
        }
    }
}
