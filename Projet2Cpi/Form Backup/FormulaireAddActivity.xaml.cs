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
    /// Logique d'interaction pour FormulaireAddActivity.xaml
    /// </summary>

    public partial class FormulaireAddActivity : UserControl
    {
        //static String[] ColorList = new string[] { "#7FFFD4", "#B0C4DE" };
        public WindowFormulaires windowFather;
        public FormulaireAddActivity()
        {
            InitializeComponent();
        }

        //On crée une nouvelle activité
        private void ButtonAddActivity_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            string Activityname = TextBoxActivityname.Text;
            if (Activityname.Length == 0)
            {   //Envoyez une notification
                MessageBox.Show("Activityname is empty");

                //Concentrez sur ce textbox
                TextBoxActivityname.Focus();
                return;
            }

            // TODO : Disable already used activities
           

            string Activitycolor = "#ffffff";
            if (ComboitemCouleur0.IsSelected == true )
            {
                Activitycolor = "#7FFFD4";
                ComboboxActivitycolor.Items.Remove(ComboitemCouleur0); // il faut supprimer la couleur choisi de la liste
                

            }
            else if (ComboitemCouleur1.IsSelected)
            {
                Activitycolor = "#B0C4DE";
                ComboitemCouleur1.IsEnabled = true;
              
            }
            else if (ComboitemCouleur2.IsSelected)
            {
                Activitycolor = "#87CEFA";
                ComboitemCouleur2.IsEnabled = false;
            }
            else if (ComboitemCouleur3.IsSelected)
            {
                Activitycolor = "#8A2BE2";
                ComboitemCouleur3.IsEnabled = false;
            }
            else if (ComboitemCouleur4.IsSelected)
            {
                Activitycolor = "#FFD700";
                ComboitemCouleur4.IsEnabled = false;
            }
            else if (ComboitemCouleur5.IsSelected)
            {
                Activitycolor = "#32CD32";
                ComboitemCouleur5.IsEnabled = false;
            }
            else if (ComboitemCouleur6.IsSelected)
            {
                Activitycolor = "#DC143C";
                ComboitemCouleur6.IsEnabled = false;
            }
            else if (ComboitemCouleur7.IsSelected)
            {
                Activitycolor = "#FFA500";
                ComboitemCouleur7.IsEnabled = false;
            }
            else if (ComboitemCouleur8.IsSelected)
            {
                Activitycolor = "#FFE4B5";
                ComboitemCouleur8.IsEnabled = false;
            }
            else if (ComboitemCouleur9.IsSelected)
            {
                Activitycolor = "#FF7F50";
                ComboitemCouleur9.IsEnabled = false;
            }

            //l'instruction ci-dessous est pour supprimer une couleur qui représente déja une autre activité
            //ComboboxActivitycolor.Items.RemoveAt(0); // This instruction delete the item 0 from the combobox
            // By Nedjima : Add activitie and close the activities Form
            DataSupervisor.ds.user.Activities.Add(Activityname, Activitycolor);
            windowFather.Close();
        }
        private void ButtonCancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            windowFather.Close();
        }
    }
}
