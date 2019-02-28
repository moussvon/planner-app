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
using System.IO;

namespace Projet2Cpi
{
    /// <summary>
    /// Interaction logic for Signup.xaml
    /// </summary>
    public partial class Signup : UserControl
    {
        public Signup()
        {
            InitializeComponent();
        }

        private void ButtonCreercompte_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            //Exceptions
            if (TextBoxUserFirstname.Text.Length == 0)

            {   //Envoyez une notification
                //Concentrez sur ce textbox


                TextBoxUserFirstname.Focus();
            }
            else if (TextBoxUserSecondname.Text.Length == 0)
            {   //Envoyez une notification


                //Concentrez sur ce textbox
                TextBoxUserSecondname.Focus();

            }
            else if (TextBoxUsername.Text.Length == 0)
            {   //Envoyez une notification


                //Concentrez sur ce textbox
                TextBoxUsername.Focus();

            }
            else if (PasswordBoxMotdepasse.Password.Length == 0)
            {   //Envoyez une notification


                //Concentrez sur ce textbox
                PasswordBoxMotdepasse.Focus();
            }
            else if (PasswordBoxMotdepasseC.Password.Length == 0)
            {   //Envoyez une notification


                //Concentrez sur ce textbox
                PasswordBoxMotdepasseC.Focus();
            }
            else if (TextBoxEmail.Text.Length == 0)
            {
                //Envoyez une notification


                //Concentrez sur ce textbox
                TextBoxEmail.Focus();
            }
            else if (TextBoxNumerodetelephone.Text.Length == 0)
            {
                //Envoyez une notification


                //Concentrez sur ce textbox
                TextBoxNumerodetelephone.Focus();
            }

            //else if ((!Regex.IsMatch(TextBoxEmail.Text, @"^[a-zA-Z][\w\.-]*[a-zA-Z0-9]@[a-zA-Z0-9][\w\.-]*[a-zA-Z0-9]\.[a-zA-Z][a-zA-Z\.]*[a-zA-Z]$"))
            //    {   //Envoyez une notification
            //        MessageBox.Show("Enter a valid email.");

            //        TextBoxEmail.Select(0, TextBoxEmail.Text.Length);
            //        TextBoxEmail.Focus();
            //}
            //Verification des mots de passe
            else
            {
                string _password = PasswordBoxMotdepasse.Password;
                string _passwordC = PasswordBoxMotdepasseC.Password;
                if (_password != _passwordC)
                {
                    //Concentrez sur ce PasswordBox
                    PasswordBoxMotdepasse.Focus();
                    MessageBox.Show("Mot de passe différent dans le deuxième champ");
                    return;
                }

                //Création du compte 
                string UserFirstname = TextBoxUserFirstname.Text;
                string UserSecondname = TextBoxUserSecondname.Text;
                string Username = TextBoxUsername.Text;
                string mail = TextBoxEmail.Text;
                string phonenumber = TextBoxNumerodetelephone.Text;
                string password = PasswordBoxMotdepasse.Password;

                User user = new User();
                user.userFirstName = UserFirstname;
                user.userSecondName = UserSecondname;
                user.userName = Username;
                user.userMail = mail;
                user.phoneNumber = phonenumber;
                user.pathOfPicture = this.profilePicTextBox.Text;
                if (user.pathOfPicture != "" && !File.Exists(user.pathOfPicture))
                {
                    MessageBox.Show("Veuillez introduire un lien correct vers l'image");
                    return;
                }
                user.Activities["Activité scolaire"] = "#ff5722";
                bool b = DataSupervisor.ds.CreateAccount(user, password);
                if (b == false)
                {
                    MessageBox.Show("Nom d'utilisateur déjà existant");
                    return;
                }
                TextBoxNumerodetelephone.Text = "";
                TextBoxEmail.Text = "";
                TextBoxUserFirstname.Text = "";
                TextBoxUsername.Text = "";
                TextBoxUserSecondname.Text = "";
                profilePicTextBox.Text = "";
                PasswordBoxMotdepasse.Password = "";
                PasswordBoxMotdepasseC.Password = "";
                MainWindow.mw.SignIn();
            }
        }
        private void LoadProfilePictureEvent(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Multiselect = false;
            d.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (d.ShowDialog() == true)
            {
                this.profilePicTextBox.Text = d.FileName;
            }
        }

        private void TextBlock_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            UserAgreement u = new UserAgreement();
            u.ShowDialog();
        }
    }
}
