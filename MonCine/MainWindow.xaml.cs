#region MÉTADONNÉES

// Nom du fichier : MainWindow.xaml.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using MonCine.Data.Classes;
using MonCine.Data.Classes.BD;
using MonCine.Data.Classes.DAL;
using MonCine.Vues;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Windows;

#endregion

namespace MonCine
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region CONSTRUCTEURS

        public MainWindow()
        {
            InitializeComponent();

            _NavigationFrame.Navigate(new Authentification());
        }

        #endregion
    }
}