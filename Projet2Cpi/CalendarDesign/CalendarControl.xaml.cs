using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using MaterialDesignThemes.Wpf;

namespace Projet2Cpi
{
    /// <summary>
    /// Interaction logic for CalendarControl.xaml
    /// </summary>
    public partial class CalendarControl : UserControl
    {
        private bool loaded = false;
        private static Thickness taskWrapPanelMargin = new Thickness(0, 2, 0, 2);
        private static SolidColorBrush taskWrapPanelTaskBackground = Brushes.White;
        private static Thickness indicatorMargin = new Thickness(3, 0, 10, 0);
        private static Double TextFontSize = 12;
        // Static method that creates a Task (evenement/tache) wrap panel from a title and priority strings
        public static WrapPanel CreateTaskWrapPanel(string taskTitle, String activity)
        {
            SolidColorBrush color;
            if (DataSupervisor.ds.user.Activities.ContainsKey(activity))
                color = new SolidColorBrush((Color)ColorConverter.ConvertFromString(DataSupervisor.ds.user.Activities[activity]));
            else color = Brushes.FloralWhite;
            WrapPanel wp = new WrapPanel()
            {
                Width = caseWidth,
                Height = caseRowHeight,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                Background = taskWrapPanelTaskBackground,
                Margin = taskWrapPanelMargin,
            };

            Rectangle indicator = new Rectangle()
            {
                Width = 6,
                Height = caseRowHeight,
                Fill = color,
                Margin = indicatorMargin,
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 10,
                    ShadowDepth = 0,
                    Opacity = 0.5,
                    Color = color.Color
                }
            };
            wp.Children.Add(indicator);
            TextBlock caption = new TextBlock()
            {
                Height = caseRowHeight,
                Text = taskTitle,
                FontSize = TextFontSize,
                TextAlignment = TextAlignment.Left,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Left,
                TextTrimming = TextTrimming.CharacterEllipsis,
                TextWrapping = TextWrapping.NoWrap
            };
            caption.Effect = new DropShadowEffect()
            {
                BlurRadius = 10,
                ShadowDepth = 0,
                Opacity = 0.5,
                Color = color.Color
            };
            wp.Children.Add(caption);
            return wp;
        }
        // Static method that creat the label of a special day with its number
        public static Grid CreateOneDayCaption(String dayTitle, int nb, SolidColorBrush color)
        {
            Grid grid = new Grid()
            {
                Width = caseWidth,
                Height = caseRowHeight,
                HorizontalAlignment = HorizontalAlignment.Right,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(1, 4, 1, 8)
            };
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = new GridLength(1, GridUnitType.Star)
            });
            grid.ColumnDefinitions.Add(new ColumnDefinition()
            {
                Width = GridLength.Auto
            });
            WrapPanel dayHeader = CreateDayHeader(dayTitle, color);
            TextBlock dayNumber = new TextBlock()
            {
                Text = nb.ToString(),
                FontSize = TextFontSize, /**/
                TextAlignment = TextAlignment.Right,
                HorizontalAlignment = HorizontalAlignment.Stretch,
                Margin = new Thickness(3)
            };
            Grid.SetColumn(dayHeader, 0);
            Grid.SetColumn(dayNumber, 1);
            grid.Children.Add(dayHeader);
            grid.Children.Add(dayNumber);
            return grid;
        }
        // Static method tat create the special day header
        public static WrapPanel CreateDayHeader(String dayTitle, SolidColorBrush color)
        {
            WrapPanel wp = new WrapPanel();

            Ellipse indicator = new Ellipse()
            {
                Height = 9,
                Width = 9,
                Fill = color,
                Margin = indicatorMargin,
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 3,
                    ShadowDepth = 0,
                    Opacity = 0.5,
                    Color = color.Color
                }
            };

            wp.Children.Add(indicator);

            TextBlock caption = new TextBlock()
            {
                Text = dayTitle,
                FontSize = TextFontSize,
                TextAlignment = TextAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                HorizontalAlignment = HorizontalAlignment.Center,
                Effect = new DropShadowEffect()
                {
                    BlurRadius = 20,
                    ShadowDepth = 0,
                    Opacity = 1,
                    Color = color.Color
                }
            };
            wp.Children.Add(caption);
            return wp;
        }
        
        // The font siae of the text labels
        /// private int numbersFontSize = 14;
        // The array that contains all the cases of the board
        private Grid[,] calCases = new Grid[6, 7];
        // The array of the special days that can be modified using SetSpecialDay method
        private WrapPanel[,] specialDays = new WrapPanel[6, 7];
        // Contains the last day of the calendar board 
        private DateTime lastDay;
        // Contains the first day of the calendar board 
        private DateTime firstDay;
        // The day numeric labels generated by SetMonth method
        private int[,] dayNumbers = new int[6, 7]; //**//
        // Bools array represents the state of calendar cases (Hidden or not)
        // true for Hidden and false otherwise
        private bool[,] casesStates = new bool[6, 7]; //**//
        
        
        private static double caseRowHeight = 15;/**/
        private static double caseWidth = 170;/**/

        private Grid[,] dayCaptions = new Grid[6, 7];
        private WrapPanel[,] dayHeaders = new WrapPanel[6, 7];
        private TextBlock[,] dayNumLabels = new TextBlock[6, 7];
        private StackPanel[,] TasksStackPanel = new StackPanel[6, 7]; 
        // the method that prepare for the calendar update by removing the existing fields
        private void ResetControlFields()
        {
            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 7; ++j)
                {
                    TasksStackPanel[i, j].Children.Clear();
                    dayHeaders[i, j].Children.Clear();
                }
            }
        }
        // The UserControl's constructor, contains all the pre-use initialization needed stuff
        public CalendarControl()
        {
            InitializeComponent();
            
            foreach (Border b in calGrid.Children)
            {
                if ((string)b.Tag == "calCase")
                {
                    calCases[Grid.GetRow(b) - 2, Grid.GetColumn(b)] = (Grid) b.Child;
                }
            }

            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 7; ++j)
                {
                    dayNumLabels[i, j] = new TextBlock()
                    {
                        TextAlignment = TextAlignment.Right,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        Margin = new Thickness(0,3, 3, 0),
                        FontSize = TextFontSize
                    };
                    Grid.SetColumn(dayNumLabels[i, j], 1);
                    dayCaptions[i, j] = new Grid();
                    dayCaptions[i, j].ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = new GridLength(1, GridUnitType.Star)
                    });
                    dayCaptions[i, j].ColumnDefinitions.Add(new ColumnDefinition()
                    {
                        Width = GridLength.Auto
                    });
                    dayCaptions[i, j].Children.Add(dayNumLabels[i, j]);

                    dayHeaders[i, j] = new WrapPanel();
                    dayCaptions[i, j].Children.Add(dayHeaders[i, j]);

                    TasksStackPanel[i, j] = new StackPanel()
                    {
                        HorizontalAlignment = HorizontalAlignment.Left,
                        Width = caseWidth
                    };
                    calCases[i, j].RowDefinitions.Add(new RowDefinition()
                    {
                        Height = GridLength.Auto
                    });

                    calCases[i, j].RowDefinitions.Add(new RowDefinition()
                    {
                        Height = new GridLength(1, GridUnitType.Star)
                    });

                    Grid.SetRow(dayCaptions[i, j], 0);
                    Grid.SetRow(TasksStackPanel[i, j], 1);
                    calCases[i, j].Children.Add(dayCaptions[i, j]);
                    calCases[i, j].Children.Add(TasksStackPanel[i, j]);
                }
            }

            this.UpdateCalendar();
            this.LoadActivities();

            loaded = true;
        }
        private string selectedActivitie = "Toutes les activités";
        // Load the content of the calendar with data
        public void ParseMonthTachesData(CalendarInfo monthDataProvider)
        {
            string order = (string)((ComboBoxItem)OrderSelectionComboBox.SelectedValue).Content;
            Dictionary<DateTime, List<Tache>> taches = monthDataProvider.getMonthTaches();
            DateTime m = monthDataProvider.Month;
            SetMonth(year: m.Year, month: m.Month);
            foreach (KeyValuePair<DateTime, List<Tache>> kv in taches)
            {
                List<Tache> lt = kv.Value;
                if (selectedActivitie != "Toutes les activités")
                {
                    lt = DataSupervisor.FilterByActivities(lt, selectedActivitie);
                }
                switch (order)
                {
                    case "Date":
                        lt.Sort(new TacheTimeComparer());
                        break;
                    case "Priorité":
                        lt.Sort(new TacheUrgenceComparer());
                        break;
                }
                this.SetDayTaches(lt, kv.Key);
            }
            Dictionary<DateTime, Dictionary<string, Object>> dict = new Dictionary<DateTime, Dictionary<string, Object>>();
            foreach (KeyValuePair<DateTime, string> kv in DataSupervisor.ds.user.JoursFeries)
            {
                Dictionary<string, Object> dict2 = new Dictionary<string, Object>();
                string color = "#555555";
                dict2["color"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                dict2["dayTitle"] = kv.Value;
                dict[kv.Key] = dict2;
            }
            this.SetDaysCaptions(dict);
        }

        public void ParseMonthEvenementsData(CalendarInfo monthDataProvider)
        {
            Dictionary<DateTime, List<Evenement>> events = monthDataProvider.getMonthEvenements();
            string order = (string)((ComboBoxItem)OrderSelectionComboBox.SelectedValue).Content;
            DateTime m = monthDataProvider.Month;
            SetMonth(year: m.Year, month: m.Month);
            foreach (KeyValuePair<DateTime, List<Evenement>> kv in events)
            {
                List<Evenement> lt = kv.Value;
                if (selectedActivitie != "Toutes les activités")
                {
                    lt = DataSupervisor.FilterByActivities(lt, selectedActivitie);
                }
                switch (order)
                {
                    case "Date":
                        lt.Sort(new EventTimeComparer());
                        break;
                    case "Priorité":
                        lt.Sort(new EventUrgenceComparer());
                        break;
                }
                this.SetDayEvenements(kv.Value, kv.Key);
            }
            Dictionary<DateTime, Dictionary<string, Object>> dict = new Dictionary<DateTime, Dictionary<string, Object>>();
            foreach (KeyValuePair<DateTime, string> kv in DataSupervisor.ds.user.JoursFeries)
            {
                Dictionary<string, Object> dict2 = new Dictionary<string, Object>();
                string color = "#555555";
                dict2["color"] = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color));
                dict2["dayTitle"] = kv.Value;
                dict[kv.Key] = dict2;
            }
            this.SetDaysCaptions(dict);
        }


        private int currentYear = 0;
        private int currentMonth = 0;
        // The method that update the calendar after the modification of data
        public void UpdateCalendar()
        {
            ResetControlFields();
            if (monthYearSelectRegion.Year != this.currentYear || monthYearSelectRegion.Month != this.currentMonth)
            {
                this.currentYear = monthYearSelectRegion.Year;
                this.currentMonth = monthYearSelectRegion.Month;
                DataSupervisor.ds.LoadMonthData(this.currentYear, this.currentMonth);
            }
            if (Tacheactive) this.ParseMonthTachesData(DataSupervisor.ds.MonthData);
            else this.ParseMonthEvenementsData(DataSupervisor.ds.MonthData);
        }
        // The method that day with the taches list
        private void SetDayTaches(List<Tache> taches, DateTime day)
        {
            int[] arr = this.GetDayCoordinates(day);
            int i = arr[0], j = arr[1];
            int cpt = 0;
            foreach (Tache t in taches)
            {
                cpt += 1;
                AddDayTask(t, day);
            }
        }
        // The method that day with Evenements list
        private void SetDayEvenements(List<Evenement> events, DateTime day)
        {
            int[] arr = this.GetDayCoordinates(day);
            int i = arr[0], j = arr[1];
            int cpt = 0;
            foreach (Evenement t in events)
            {
                cpt += 1;
                AddDayEvenement(t, day);
            }
        }
        // The method that sets the days titles
        // dayData is an array of Dictionary<String, Object> 
        // {"dayTitle" : String, "color" : SolidColorBrush}
        public void SetDaysCaptions(Dictionary<DateTime, Dictionary<String, Object>> daysData)
        {
            foreach (DateTime d in daysData.Keys)
            {
                if (d > lastDay || d < firstDay) continue;
                int[] arr = GetDayCoordinates(d);
                int i = arr[0], j = arr[1];
                String dayTitle = (String)daysData[d]["dayTitle"];
                SolidColorBrush color = (SolidColorBrush)daysData[d]["color"];
                SetDayHeader(i, j, dayTitle, color);
            }
        }
        // The method that add tache to the specified day
        // TODO : make evenement add method
        public void AddDayTask(Tache task, DateTime date)
        {
            if (date > lastDay || date < firstDay) throw new Exception("The day is not in the grid");
            int[] arr = GetDayCoordinates(date);
            int i = arr[0], j = arr[1];
            TasksStackPanel[i, j].Children.Add(CreateTaskWrapPanel(task.title, task.Activitee));
        }
        // The method that add evenement to the specified day
        public void AddDayEvenement(Evenement evenement, DateTime date)
        {
            if (date > lastDay || date < firstDay) throw new Exception("The day is not in the grid");
            int[] arr = GetDayCoordinates(date);
            int i = arr[0], j = arr[1];
            TasksStackPanel[i, j].Children.Add(CreateTaskWrapPanel(evenement.Title, evenement.Priority));
        }
        // The method that change the month currently displayed in the calendar
        public void SetMonth(int year, int month)
        {
            DateTime d = new DateTime(year, month, 1);
            int weekDay = (int)d.DayOfWeek;
            firstDay = new DateTime(year, month, 1);
            lastDay = new DateTime(year, month, 1);
            firstDay = firstDay.AddDays(-weekDay);
            lastDay = firstDay.AddDays(7 * 6);
            int currentMonthDays = DateTime.DaysInMonth(year, month);
            for (int i = weekDay; i < currentMonthDays + weekDay; ++i)
            {
                dayNumbers[i / 7, i % 7] = i - weekDay + 1;
                casesStates[i / 7, i % 7] = false;
            }

            int recentMonth = month > 1 ? month - 1 : 12;
            if (recentMonth == 12) year -= 1;
            int monthDays = DateTime.DaysInMonth(year, recentMonth);
            for (int i = 0; i < weekDay; i++)
            {
                dayNumbers[0, i] = monthDays - weekDay + i + 1;
                casesStates[0, i] = true;
            }
            for (int i = weekDay + currentMonthDays; i < 6 * 7; i++)
            {
                dayNumbers[i / 7, i % 7] = i - weekDay - currentMonthDays + 1;
                casesStates[i / 7, i % 7] = true;
            }
            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 7; ++j)
                {
                    dayNumLabels[i, j].Text = dayNumbers[i, j].ToString();
                }
            }
            SetOpacity();
        }
        // The method that sts the specific day title (special day)
        public void SetDayHeader(int i, int j, String title, SolidColorBrush color)
        {
            WrapPanel wp = CreateDayHeader(title, color);
            UIElement[] arr = new UIElement[2];
            wp.Children.CopyTo(arr, 0);
            wp.Children.Clear();
            dayHeaders[i, j].Children.Clear();
            dayHeaders[i, j].Children.Insert(0, arr[0]);
            dayHeaders[i, j].Children.Insert(1, arr[1]);
        }

        // Hides the cases which doesn't correspond to the current month
        public void SetOpacity()
        {
            for (int i = 0; i < 6; ++i)
            {
                for (int j = 0; j < 7; ++j)
                {
                    calCases[i, j].IsEnabled = !casesStates[i, j];
                    calCases[i, j].Opacity = casesStates[i, j] ? 0.6 : 1;
                }
            }
        }
        // Returns the coordinates of the day in the calendar board
        // Throws an exception if the day is not in the calendar board
        private int[] GetDayCoordinates(DateTime date)
        {
            if (date < firstDay || date > lastDay) throw new Exception($"This date {date.ToString()} is not from the current calendar !");
            int[] lst = new int[2];
            DateTime timeDiff = new DateTime(date.Ticks - firstDay.Ticks);
            int dayDiff = timeDiff.Day - 1;
            return new int[] { (int)dayDiff / 7, dayDiff % 7 };
        }

        // Buttons Events
        // Tache event button visual event
        private void BoutonTacheEvenement_MouseEnter(object sender, MouseEventArgs e)
        {
            if (((Border)sender).Background.Equals(Brushes.White))
            {
                ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(104, 102, 247));
                ((TextBlock)(((Border)sender).Child)).Foreground = Brushes.White;
            }
        }
        // Tache event button visual event
        private void BoutonTacheEvenement_MouseLeave(object sender, MouseEventArgs e)
        {
            if (((Border)sender).Background.ToString().Equals((new SolidColorBrush(Color.FromRgb(104, 102, 247))).ToString()))
            {
                ((Border)sender).Background = Brushes.White;
                ((TextBlock)(((Border)sender).Child)).Foreground = new SolidColorBrush(Color.FromRgb(104, 102, 247));
            }
        }
        private bool Tacheactive = true;
        // Tache button clicked event
        private void BoutonGaucheTacheEvenement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((Border)sender).Background.ToString().Equals((new SolidColorBrush(Color.FromRgb(104, 102, 247))).ToString()))
            {
                ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(40, 53, 147));
                ((TextBlock)(((Border)sender).Child)).Foreground = Brushes.White;
                BoutonDroitTacheEvenement.Background = Brushes.White;
                ((TextBlock)(BoutonDroitTacheEvenement.Child)).Foreground = new SolidColorBrush(Color.FromRgb(104, 102, 247));
                //titrePage.Text = "Tâches";
            }
            // TODO : reload logic
            if (!Tacheactive)
            {
                Tacheactive = true;
                MainWindow.CalendarField.UpdateCalendar();
            }
        }
        // Evenement button clicked event
        private void BoutonDroitTacheEvenement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (((Border)sender).Background.ToString().Equals((new SolidColorBrush(Color.FromRgb(104, 102, 247))).ToString()))
            {
                ((Border)sender).Background = new SolidColorBrush(Color.FromRgb(40, 53, 147));
                ((TextBlock)(((Border)sender).Child)).Foreground = Brushes.White;
                BoutonGaucheTacheEvenement.Background = Brushes.White;
                ((TextBlock)(BoutonGaucheTacheEvenement.Child)).Foreground = new SolidColorBrush(Color.FromRgb(104, 102, 247));
                //titrePage.Text = "Evènements";
            }
            // TODO : reload logic
            if (Tacheactive)
            {
                Tacheactive = false;
                MainWindow.CalendarField.UpdateCalendar();
            }
        }

        private ComboBoxItem CreateActivityComboBox(string name, string color)
        {
            ComboBoxItem cbi = new ComboBoxItem()
            {
                Height = 30,
                DataContext = name
            };
            cbi.Selected += delegate(object sender, RoutedEventArgs e)
            {
                selectedActivitie = name;
            };
            WrapPanel sp = new WrapPanel()
            {
                Orientation = Orientation.Horizontal,
                VerticalAlignment = VerticalAlignment.Center
            };
            PackIcon icon = new PackIcon()
            {
                Kind = PackIconKind.CheckboxBlankCircle,
                Foreground = new SolidColorBrush((Color)ColorConverter.ConvertFromString(color.ToString())),
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
        public void LoadActivities()
        {
            ComboBoxItem cb0 = (ComboBoxItem) ActivitySelectionComboBox.Items.GetItemAt(0);
            ActivitySelectionComboBox.Items.Clear();
            ActivitySelectionComboBox.Items.Add(cb0);
            ActivitySelectionComboBox.SelectedIndex = 0;
            foreach (KeyValuePair<string, string> kv in DataSupervisor.ds.user.Activities)
            {
                ActivitySelectionComboBox.Items.Add(CreateActivityComboBox(kv.Key, kv.Value));
            }
        }
        private void Update(object sender, RoutedEventArgs e)
        {
            if (loaded)
            {
                this.UpdateCalendar();
            }
        }

        private void ToutActivitiesSelected(object sender, RoutedEventArgs e)
        {
            selectedActivitie = "Toutes les activités";
        }
    }
}
