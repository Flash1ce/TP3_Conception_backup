#region MÉTADONNÉES

// Nom du fichier : Accueil.xaml.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using System;
using System.Windows;
using System.Windows.Controls;
using MongoDB.Driver;

#endregion

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class Accueil : Page
    {
        #region ATTRIBUTS

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;

        #endregion

        #region CONSTRUCTEURS

        public Accueil(IMongoClient pClient, IMongoDatabase pDb)
        {
            InitializeComponent();
            _client = pClient;
            _db = pDb;
        }

        #endregion

        #region MÉTHODES

        private void BtnConsulterAbonne_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FAbonnes(_client, _db));
        }

        private void BtnConsulterFilms_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FFilms(_client, _db));
        }

        #endregion
    }
}