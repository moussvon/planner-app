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
using System.IO;

namespace Projet2Cpi
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static MainWindow mw;
        public static CalendarControl CalendarField = null;
        public static Taches TachesEventsField = null;
        public static Contacts ContactesField = null;
        public static PlanningSettings planningField = null;
        public static AppSettings settingsField = null;
        public void exit(object o, ExitEventArgs e)
        {
            OtherThread.stop();
        }
        public MainWindow()
        {
            InitializeComponent();
            Application.Current.Exit += new ExitEventHandler(exit);
            mw = this;
            this.Icon = new BitmapImage(new Uri(Environment.CurrentDirectory + @"/images/icone.png"));
        }

        public static WrapPanel CreateActivitieTag(String name, String c)
        {
            WrapPanel wp = new WrapPanel()
            {
                Orientation = Orientation.Horizontal,
                HorizontalAlignment = HorizontalAlignment.Left,
                VerticalAlignment = VerticalAlignment.Top,
                Margin = new Thickness(10, 5, 0, 0)
            };
            SolidColorBrush color = new SolidColorBrush((Color) ColorConverter.ConvertFromString(c));
            System.Windows.Media.Color colorb = (Color)System.Windows.Media.ColorConverter.ConvertFromString("#FFFFFFFF");
            Brush br = new SolidColorBrush(colorb);
            Ellipse ring = new Ellipse()
            {
                Width = 15,
                Height = 15,
                Fill = color,
                Margin = new Thickness(2, 2, 10, 2)
            };
            wp.Children.Add(ring);
            wp.Children.Add(new TextBlock()
            {
                Text = name,
                VerticalAlignment = VerticalAlignment.Center,
                Foreground = br,
                FontSize = 15
            });
            return wp;
        }
        public void LoadActivities()
        {
            activities.Children.Clear();
            if (DataSupervisor.ds.user == null) return;
            activities.Children.Add(CreateActivitieTag("Jour férier", "#555555"));
            foreach (KeyValuePair<String, String> kv in DataSupervisor.ds.user.Activities)
            {
                activities.Children.Add(CreateActivitieTag(kv.Key, kv.Value));
            }
        }

        private void ButtonPopUpLogout_Click(object sender,RoutedEventArgs e)
        {
            DataSupervisor.ds.LogOut();
            signField.Visibility = Visibility.Visible;
            board.Visibility = Visibility.Collapsed;
            signField.Signin(null, null);
        }

        private void Button_MouseEnter(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush (Color.FromRgb(94,92,223));//Modifie la couleur lors de l'entrée de la souris
        }
        private void Button_MouseLeave(object sender, MouseEventArgs e)
        {
            ((Border)sender).Background = new SolidColorBrush (Color.FromRgb(104,102,247));//Modifie la couleur lors de la sortie de la souris
        }

        private void tachesItem_Selected(object sender, RoutedEventArgs e)
        {
            if (gridPagePrincipale.Children.Count < 1 || !(gridPagePrincipale.Children[0] is Taches))
            {
                gridPagePrincipale.Children.Clear();
                gridPagePrincipale.Children.Add(MainWindow.TachesEventsField);
            }
            
        }

        private void calendrierItem_Selected(object sender, RoutedEventArgs e)
        {
            if (gridPagePrincipale.Children.Count < 1 || !(gridPagePrincipale.Children[0] is CalendarControl))
            {
                gridPagePrincipale.Children.Clear();
                gridPagePrincipale.Children.Add(MainWindow.CalendarField);
            }
        }

        private void PlanningTab_Selected(object sender, RoutedEventArgs e)
        {
            if (gridPagePrincipale.Children.Count < 1 || !(gridPagePrincipale.Children[0] is PlanningSettings))
            {
                gridPagePrincipale.Children.Clear();
                gridPagePrincipale.Children.Add(MainWindow.planningField);
            }
        }

        private void contactItem_Selected(object sender, RoutedEventArgs e)
        {
            if (gridPagePrincipale.Children.Count < 1 || !(gridPagePrincipale.Children[0] is Contacts))
            {
                gridPagePrincipale.Children.Clear();
                gridPagePrincipale.Children.Add(MainWindow.ContactesField);
            }
        }

        private void configurationsItem_Selected(object sender, RoutedEventArgs e)
        {
            gridPagePrincipale.Children.Clear();
            gridPagePrincipale.Children.Add(MainWindow.settingsField);
        }

        private void ButtonAjouterContact_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            WindowFormulaires windF = new WindowFormulaires();
            FormulaireAddContact f = new FormulaireAddContact();
            try
            {
                windF.gridPrincWinForm.Children.Clear();
            }
            catch { }
            windF.gridPrincWinForm.Children.Add(f);
            f.windowFather = windF;
            windF.ShowDialog();
        }

        private void ButtonAjouterEvenement_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FormulaireAddEvent form = new FormulaireAddEvent();
            WindowFormulaires winF = new WindowFormulaires();
            form.windowParent = winF;
            winF.gridPrincWinForm.Children.Add(form);
            winF.ShowDialog();
        }

        private void ButtonAjouterTache_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FormulaireAddTache form = new FormulaireAddTache();
            WindowFormulaires winF = new WindowFormulaires();
            form.windowParent = winF;
            winF.gridPrincWinForm.Children.Add(form);
            winF.ShowDialog();
        }

        private void ButtonAjouterActivite_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            FormulaireAddActivity form = new FormulaireAddActivity();
            WindowFormulaires win = new WindowFormulaires();
            form.windowFather = win;
            win.gridPrincWinForm.Children.Add(form);
            win.Height = 403;
            win.ShowDialog();
        }
        public void SignIn()
        {
            if (CalendarField == null) CalendarField = new CalendarControl();
            else CalendarField.UpdateCalendar();
            if (TachesEventsField == null) TachesEventsField = new Taches();
            else TachesEventsField.Update();
            if (ContactesField == null) ContactesField = new Contacts();
            if (planningField == null) planningField = new PlanningSettings();
            if (settingsField == null) settingsField = new AppSettings();
            else ContactesField.LoadContactes();
            this.LoadUserName();
            this.LoadUserPicture();
            this.LoadActivities();
            tachesItem_Selected(null, null);
            signField.Visibility = Visibility.Collapsed;
            board.Visibility = Visibility.Visible;
        }

        public  void LoadUserPicture()
        {
            ImageSource imgsrc;
            try
            {
                imgsrc = new BitmapImage(new Uri(DataSupervisor.ds.user.pathOfPicture));
            } catch(UriFormatException)
            {
                imgsrc = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"/images/photoProfilDefault.jpg"));
            }
            catch (ArgumentNullException)
            {
                imgsrc = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"/images/photoProfilDefault.jpg"));
            }
            this.userPicture.Source = imgsrc;
        }

        public void LoadUserName()
        {
            this.UserPseudo.Text = DataSupervisor.ds.user.userName;
        }
        public static void UpdateFields()
        {
            CalendarField.UpdateCalendar();
            TachesEventsField.Update();
        }

        private void signField_Loaded(object sender, RoutedEventArgs e)
        {

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindowWindow.WindowState = WindowState.Minimized;
        }

        private void Button_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void MainWindowWindow_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.F1)) {
                MessageBox.Show("Help");
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.F2))
            {
                MessageBox.Show("About");
            }
            if (Keyboard.IsKeyDown(Key.LeftAlt) && Keyboard.IsKeyDown(Key.F4))
            {
                Application.Current.Shutdown();
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.D))
            {
                DataSupervisor.ds.LogOut();
                signField.Visibility = Visibility.Visible;
                board.Visibility = Visibility.Collapsed;
                signField.Signin(null, null);
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.T))
            {
                FormulaireAddTache form = new FormulaireAddTache();
                WindowFormulaires winF = new WindowFormulaires();
                form.windowParent = winF;
                winF.gridPrincWinForm.Children.Add(form);
                winF.ShowDialog();
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.E))
            {
                FormulaireAddEvent form = new FormulaireAddEvent();
                WindowFormulaires winF = new WindowFormulaires();
                form.windowParent = winF;
                winF.gridPrincWinForm.Children.Add(form);
                winF.ShowDialog();
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.C))
            {
                WindowFormulaires windF = new WindowFormulaires();
                FormulaireAddContact f = new FormulaireAddContact();
                try
                {
                    windF.gridPrincWinForm.Children.Clear();
                }
                catch { }
                windF.gridPrincWinForm.Children.Add(f);
                f.windowFather = windF;
                windF.ShowDialog();
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.A))
            {
                FormulaireAddActivity form = new FormulaireAddActivity();
                WindowFormulaires win = new WindowFormulaires();
                form.windowFather = win;
                win.gridPrincWinForm.Children.Add(form);
                win.Height = 403;
                win.ShowDialog();
            }
            if (Keyboard.IsKeyDown(Key.LeftCtrl) && Keyboard.IsKeyDown(Key.R))
            {
                MainWindowWindow.WindowState = WindowState.Minimized;
            }
        }

        private void MainWindowWindow_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ChangedButton == MouseButton.Left && e.LeftButton == MouseButtonState.Pressed) this.DragMove();
        }

        private void AideInvoke(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + @"\Manuel\manuel.html");
            } catch
            {

            }
        }

        private void AboutInvoke(object sender, MouseButtonEventArgs e)
        {
            try
            {
                System.Diagnostics.Process.Start(Directory.GetCurrentDirectory() + @"\Site\index.html");
            }
            catch
            {

            }
        }
    }
}
