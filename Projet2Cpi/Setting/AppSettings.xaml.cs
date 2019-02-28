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
    /// Interaction logic for AppSettings.xaml
    /// </summary>
    public partial class AppSettings : UserControl
    {

        private string picturePath;
        public void LoadPicture(string pathOfPicture)
        {
            try
            {
                this.profilePic.Source = new BitmapImage(new Uri(pathOfPicture));
                this.picturePath = pathOfPicture;
            } catch
            {
                this.profilePic.Source = new BitmapImage(new Uri(Directory.GetCurrentDirectory() + @"//images//photoProfilDefault.jpg"));
            }
        }

        public void LoadUserData(User u)
        {
            TextBoxFirstName.Text = u.userFirstName;
            TextBoxSecondName.Text = u.userSecondName;
            TextBoxPhonenumber.Text = u.phoneNumber;
            TextBoxEmail.Text = u.userMail;
            this.LoadPicture(u.pathOfPicture);
        }

        public AppSettings()
        {
            InitializeComponent();
            if (DataSupervisor.ds.user != null) this.LoadUserData(DataSupervisor.ds.user);
        }

        private void RegisterData(object sender, MouseButtonEventArgs e)
        {
            DataSupervisor.ds.user.userFirstName = TextBoxFirstName.Text;
            DataSupervisor.ds.user.userSecondName = TextBoxSecondName.Text;
            DataSupervisor.ds.user.phoneNumber = TextBoxPhonenumber.Text;
            DataSupervisor.ds.user.userMail = TextBoxEmail.Text;
            MainWindow.mw.LoadUserName();
        }


        private void ChoosePicture(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog d = new OpenFileDialog();
            d.Multiselect = false;
            d.Filter = "Image files (*.png;*.jpeg;*.jpg)|*.png;*.jpeg;*.jpg|All files (*.*)|*.*";
            if (d.ShowDialog() == true)
            {
                picturePath = d.FileName;
            }
            this.LoadPicture(picturePath);
        }

        private void SavePicture(object sender, MouseButtonEventArgs e)
        {
            if (picturePath != null)
            {
                DataSupervisor.ds.user.pathOfPicture = picturePath;
                MainWindow.mw.LoadUserPicture();
            }
        }
    }
}
