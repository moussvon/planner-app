using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Projet2Cpi
{
    /// <summary>
    /// Logique d'interaction pour App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            DataSupervisor.ds = new DataSupervisor();
            /*User u = new User();
            u.userName = "EQUIPE30";
            Dictionary<String, String> act = new Dictionary<string, string>();
            act.Add("Something", "#B0C4DE");
            u.Activities = act;
            DataSupervisor.ds.CreateAccount(u, "WORST PROJECT");*/
            /*? b = DataSupervisor.ds.LogIn("EQUIPE30", "WORST PROJECT");
            switch (b)
            {
                case null:
                    MessageBox.Show("Wrong user name");
                    break;
                case false:
                    MessageBox.Show("Wrog user name");
                    break;
            }*/
        }
    }
}
