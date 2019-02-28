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
    /// Interaction logic for Signin.xaml
    /// </summary>
    public partial class Signin : UserControl
    {

        public Signin()
        {
            InitializeComponent();
            
        }

        
        private void SigninEvent(object sender, MouseButtonEventArgs e)
        {
            //Exceptions
            if (TextBoxNomdutilisateur.Text == "")
            {
                TextBoxNomdutilisateur.Focus();
                return;
            }
            bool passwordVisibile = passwordVisibilityTB.IsChecked.Value;
            string password = passwordVisibile ? PasswordBoxMotdepasseVisible.Text : PasswordBoxMotdepasseInvisible.Password;
            if (password == "")
            {
                PasswordBoxMotdepasseInvisible.Focus();

                return;
            }
            string username = TextBoxNomdutilisateur.Text;
            bool? b = DataSupervisor.ds.LogIn(username, password);
            if (b == null)
            {
                MessageBox.Show("Utilisateur non enregistré sur cette machine.\nVeuillez Introduire un aute nom d'utilisateur ou créer un nouveau compte");
                if (passwordVisibile) PasswordBoxMotdepasseVisible.Text = "";
                else PasswordBoxMotdepasseInvisible.Password = "";
                TextBoxNomdutilisateur.Text = "";
                TextBoxNomdutilisateur.Focus();
            }
            else if (!b.Value)
            {
                MessageBox.Show("Mot de passe incorrecte !");
                if (passwordVisibile) PasswordBoxMotdepasseVisible.Text = "";
                else PasswordBoxMotdepasseInvisible.Password = "";
                if (passwordVisibile) PasswordBoxMotdepasseVisible.Focus();
                else PasswordBoxMotdepasseInvisible.Focus();
            }
            else
            {
                MainWindow.mw.SignIn();
            }
        }

        private void ToggleButton_Checked(object sender, RoutedEventArgs e)
        {
            this.PasswordBoxMotdepasseInvisible.Visibility = Visibility.Hidden;
            this.PasswordBoxMotdepasseVisible.Visibility = Visibility.Visible;
            PasswordBoxMotdepasseVisible.Text = PasswordBoxMotdepasseInvisible.Password;
        }

        private void ToggleButton_Unchecked(object sender, RoutedEventArgs e)
        {
            this.PasswordBoxMotdepasseInvisible.Visibility = Visibility.Visible;
            this.PasswordBoxMotdepasseVisible.Visibility = Visibility.Hidden;
            PasswordBoxMotdepasseInvisible.Password = PasswordBoxMotdepasseVisible.Text;
        }

        private void PasswordBoxMotdepasseVisible_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {

                if (TextBoxNomdutilisateur.Text == "")
                {
                    TextBoxNomdutilisateur.Focus();
                    return;
                }
                bool passwordVisibile = passwordVisibilityTB.IsChecked.Value;
                string password = passwordVisibile ? PasswordBoxMotdepasseVisible.Text : PasswordBoxMotdepasseInvisible.Password;
                if (password == "")
                {
                    PasswordBoxMotdepasseInvisible.Focus();

                    return;
                }
                string username = TextBoxNomdutilisateur.Text;
                bool? b = DataSupervisor.ds.LogIn(username, password);
                if (b == null)
                {
                    MessageBox.Show("Utilisateur non enregistré sur cette machine.\nVeuillez Introduire un aute nom d'utilisateur ou créer un nouveau compte");
                    if (passwordVisibile) PasswordBoxMotdepasseVisible.Text = "";
                    else PasswordBoxMotdepasseInvisible.Password = "";
                    TextBoxNomdutilisateur.Text = "";
                    TextBoxNomdutilisateur.Focus();
                }
                else if (!b.Value)
                {
                    MessageBox.Show("Mot de passe incorrect !");
                    if (passwordVisibile) PasswordBoxMotdepasseVisible.Text = "";
                    else PasswordBoxMotdepasseInvisible.Password = "";
                    if (passwordVisibile) PasswordBoxMotdepasseVisible.Focus();
                    else PasswordBoxMotdepasseInvisible.Focus();
                }
                else
                {
                    MainWindow.mw.SignIn();
                    PasswordBoxMotdepasseVisible.Text = "";
                    PasswordBoxMotdepasseInvisible.Password = "";
                }
            }
        }

    }
}
