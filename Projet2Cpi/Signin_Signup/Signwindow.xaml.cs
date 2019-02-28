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
    /// Interaction logic for Signwindow.xaml
    /// </summary>
    public partial class Signwindow : UserControl
    {
        public Signwindow()
        {
            InitializeComponent();
            SignCardplace.Children.Clear();
            UserControl signin = new Signin();
            SignCardplace.Children.Add(signin);
            welcomimg.Source = new BitmapImage(new Uri(Environment.CurrentDirectory + @"\images\Sign_illustration.jpg"));
        }

        private void Signup(object sender, MouseButtonEventArgs e)
        {
            SignCardplace.Children.Clear();
            UserControl signup = new Signup();
            SignCardplace.Children.Add(signup);
        }

        public void Signin(object sender, MouseButtonEventArgs e)
        {
            SignCardplace.Children.Clear();
            UserControl signin = new Signin();
            SignCardplace.Children.Add(signin);
        }

        private void ExitEvent(object sender, RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }

    }
}
