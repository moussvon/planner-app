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
using MaterialDesignThemes.Wpf;

namespace Projet2Cpi
{
    /// <summary>
    /// Logique d'interaction pour FormulaireAddEvent.xaml
    /// </summary>
    public partial class FormulaireAddEvent : UserControl
    {
        static int cpt_files; // Ce compteur est pour compter le nombre de fichiers ajoutés

        OpenFileDialog file;
        public WindowFormulaires windowParent;
        public FormulaireAddEvent()
        {
            InitializeComponent();
        }

        //Cette fonction, est pour importer des fichiers à l'évènement , en cliquant sur l icon "ImportFile"
        private void Importfile_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();
            file.Filter = "Pdf Files (*.pdf)|*.pdf|Excel Files (*.xls)|*.xls|Word Files (*.docx)|*.docx|All files (*.*)|*.*";
            file.InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            file.Multiselect = true; //On peut selectionner multiple fichiers

            if (file.ShowDialog() == true)
            {

                //Si l'utilisatuer selectionne aucun fichier
                if (file.FileNames.Length == 0)
                {
                    cpt_files = 0;   //un compteur qui compte le nombre des fichiers selectionnés
                    AddfilesCard.Visibility = Visibility.Visible;
                    FilesChipsCard.Visibility = Visibility.Hidden;
                }
                //si il selectionne un ficher
                else if (file.FileNames.Length == 1)
                {
                    AddfilesCard.Visibility = Visibility.Hidden;    //On cache la carte d'ajout et on affiche la carte des chips
                    FilesChipsCard.Visibility = Visibility.Visible;

                    cpt_files = 1;   //un compteur qui compte le nombre des fichiers selectionnés
                    File1.Visibility = Visibility.Visible;          //On affiche le premiers CHIPS , le seul fichier selectionné
                    File1.Content = file.SafeFileName;              //On nomme le chips, par le nom du fichier

                }
                // si il selectionne deux fichiers
                else if (file.FileNames.Length == 2)
                {
                    AddfilesCard.Visibility = Visibility.Hidden;
                    FilesChipsCard.Visibility = Visibility.Visible;

                    cpt_files = 2;   //un compteur qui compte le nombre des fichiers selectionnés
                    File1.Visibility = Visibility.Visible;
                    File2.Visibility = Visibility.Visible;

                    //On GET les noms des deux fichiers selectionnés
                    String[] MergedFile = new String[2];
                    int cpt = 0;
                    foreach (string filename in file.SafeFileNames)
                    {
                        MergedFile[cpt] = filename;
                        cpt++;
                    }

                    //On nomme les chips et leurs TOOLTIP par les noms des fichiers
                    File1.Content = MergedFile[0];
                    File1.ToolTip = MergedFile[0];
                    File2.Content = MergedFile[1];
                    File2.ToolTip = MergedFile[1];
                }
                // si il selectionne trois fichiers
                else if (file.FileNames.Length == 3)
                {
                    AddfilesCard.Visibility = Visibility.Hidden;
                    FilesChipsCard.Visibility = Visibility.Visible;

                    cpt_files = 3;   //un compteur qui compte le nombre des fichiers selectionnés
                    File1.Visibility = Visibility.Visible;  //On affiche tous les CHIPS 
                    File2.Visibility = Visibility.Visible;
                    File3.Visibility = Visibility.Visible;

                    //On GET les noms des deux fichiers selectionnés
                    String[] MergedFile = new String[3];
                    int cpt = 0;
                    foreach (string filename in file.SafeFileNames)
                    {
                        MergedFile[cpt] = filename;
                        cpt++;
                    }
                    //On nomme les chips et leurs TOOLTIP par les noms des fichiers
                    File1.Content = MergedFile[0];
                    File1.ToolTip = MergedFile[0];
                    File2.Content = MergedFile[1];
                    File2.ToolTip = MergedFile[1];
                    File3.Content = MergedFile[2];
                    File3.ToolTip = MergedFile[2];
                }
                // s'il selectionne plus de trois fichiers
                else if (file.FileNames.Length > 3)
                {
                    MessageBox.Show("You have to select just 3 files");
                    cpt_files = 0;   //un compteur qui compte le nombre des fichiers selectionnés

                }
            }
        }


        private void Chip_Delete(object sender, RoutedEventArgs e)
        {

        }


        //Cette fonction est pour l'ajout des contactes en tappant sur clavier
        //Still needs to verify if the name typed is one of the contacts or no 
        //still needs to make the list of the contacts beggins with the first letter appear while typing
        //Still needs to do the function of the delete click on it !
        //Still needs to adapt the width of the chip  according to the length of the contact' name
        private void CommentTextBox_KeyDown(object sender, KeyEventArgs e)
        {

            if (e.Key == Key.Enter)
            {
                Chip ch = new Chip();
                
                ch.Height = 30;
                ch.Width = 120;
                Thickness margin = ch.Margin;
                margin.Right += 5;
                ch.Margin = margin;

                ch.Content = ((TextBox)sender).Text;
                try
                {
                    //string ic = new String(((TextBox)sender).Text[0], 1);
                    //ch.Icon = ic.ToUpper();
                    ch.IsDeletable = true;
                    ((TextBox)sender).Text = "";
                    chips.Children.Add(ch);
                }
                catch (Exception)
                {
                }
            }

        }


        
        //Les trois fonctions ci dessous sont pour supprimer les Chips des fichiers selectionnés
        //En effet, pour le moment cettes fonctions manipulent seulement l affichage 
        //Je propose lors la création de l objet event , on vérifie d'abord les CHIPS qui sont VISIBLES,puis on enregistre leurs données 
        
        private void File1_DeleteClick(object sender, RoutedEventArgs e)
        {
            File1.Visibility = Visibility.Hidden;   //Si le premier fichiers est supprimée, on cache son CHIP , puis on modifie le MARGIN GAUCHE du CHIP qui le suit 
            Thickness margin = File2.Margin;
            margin.Left = -100;
            File2.Margin = margin;
            cpt_files -= 1;
            if (cpt_files == 0)         //Si tous les fichiers sont supprimés, on cache la carte des fichiers ajoutés et on affiche la carte d'ajout des fichiers 
            {
                AddfilesCard.Visibility = Visibility.Visible;
                FilesChipsCard.Visibility = Visibility.Hidden;
                margin.Left += 105;
                File3.Margin = margin;
                margin = File2.Margin;
                margin.Left += 105;
                File2.Margin = margin;
            }
        }

        private void File2_DeleteClick(object sender, RoutedEventArgs e)
        {
            File2.Visibility = Visibility.Hidden;
            Thickness margin = File3.Margin;
            margin.Left = -100;
            File3.Margin = margin;
            cpt_files -= 1;
            if (cpt_files == 0)
            {
                AddfilesCard.Visibility = Visibility.Visible;
                FilesChipsCard.Visibility = Visibility.Hidden;
                margin.Left += 105;
                File3.Margin = margin;
                margin = File2.Margin;
                margin.Left += 105;
                File2.Margin = margin;
            }
        }

        private void File3_DeleteClick(object sender, RoutedEventArgs e)
        {
            File3.Visibility = Visibility.Hidden;
            cpt_files -= 1;
            Thickness margin = File3.Margin;
            if (cpt_files == 0)
            {
                AddfilesCard.Visibility = Visibility.Visible;
                FilesChipsCard.Visibility = Visibility.Hidden;
                margin.Left += 105;
                File3.Margin = margin;
                margin = File2.Margin;
                margin.Left += 105;
                File2.Margin = margin;
            }
        }


        //Cette fonction,est pour l'ajout du deuxiéme alarme , en cliquant sur l icon d'ajout à droite 
        
        private void AddAlarme1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ComboBoxAlarmType1.SelectedIndex == -1 || DatePickeralarm1.SelectedDate == null || TimePickeralarm1.SelectedTime == null)
            {
                MessageBox.Show("Vous devez d'abord introduire toutes les données du la première alarme !");
            }
            else
            {
                AddAlarme1.Visibility = Visibility.Hidden;
                DeleteAlarme1.Visibility = Visibility.Hidden;
                AlarmWrapPanel2.Visibility = Visibility.Visible;
            }
        }

        //Cette fonction , va supprimer les données déja entrés pour la première alarme 
        //Les instructions de cette fonction, peuvent être comme des conditions à la création d objet EVENEMENT aprés !
        private void DeleteAlarme1_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ComboBoxAlarmType1.SelectedIndex = -1;
            DatePickeralarm1.SelectedDate = null;
            TimePickeralarm1.SelectedTime = null;
        }

        //Cette fonction , va cacher le WRAPPANEL ayant les PICKERS de la deuxiéme alarme, et les mis à NULL
        //Lors la création d'objet EVENEMENT aprés, vous pouver également tester si le "AlarmWrapPanel2" est VISIBLE , ou si le PICKERS sont à null et le Comboboxselectedindex est à -1
        private void DeleteAlarme2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ComboBoxAlarmType2.SelectedIndex = -1;
            DatePickeralarm2.SelectedDate = null;
            TimePickeralarm2.SelectedTime = null;
            AlarmWrapPanel2.Visibility = Visibility.Hidden;
            AddAlarme1.Visibility = Visibility.Visible;
            DeleteAlarme1.Visibility = Visibility.Visible;
        }

        //Cette fonction,est pour l'ajout du troisiéme alarme , en cliquant sur l icon d'ajout à droite 
        private void AddAlarme2_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (ComboBoxAlarmType2.SelectedIndex == -1 || DatePickeralarm2.SelectedDate == null || TimePickeralarm2.SelectedTime == null)
            {
                MessageBox.Show("Vous devez d'abord introduire toutes les données du la deuxième alarme !");
            }
            else
            {
                AddAlarme2.Visibility = Visibility.Hidden;
                DeleteAlarme2.Visibility = Visibility.Hidden;
                AlarmWrapPanel3.Visibility = Visibility.Visible;
            }
        }

        //Cette fonction , va cacher le WRAPPANEL ayant les PICKERS de la troisiéme alarme, et les mis à NULL
        //Lors la création d'objet EVENEMENT aprés, vous pouver également tester si le "AlarmWrapPanel3" est HIDDEN , ou si le PICKERS sont à null et le Comboboxselectedindex est à -1
        private void DeleteAlarme3_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            ComboBoxAlarmType3.SelectedIndex = -1;
            DatePickeralarm3.SelectedDate = null;
            TimePickeralarm3.SelectedTime = null;
            AlarmWrapPanel3.Visibility = Visibility.Hidden;
            AddAlarme2.Visibility = Visibility.Visible;
            DeleteAlarme2.Visibility = Visibility.Visible;
        }

        private void ButtonAddEvent_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Evenement ev = new Evenement();
            if (debutDatePicker.SelectedDate == null)
            {
                MessageBox.Show("Please select a day");
                return;
            }
            ev.DateDebut = debutDatePicker.SelectedDate.Value;
            ev.Title = TextboxTitre.Text;
            ev.Description = TextBoxDescription.Text;
            DataSupervisor.ds.AddEvenement(ev);

            windowParent.Close();
        }

        private void ButtonCancel_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            windowParent.Close();
        }

        
    }
}
