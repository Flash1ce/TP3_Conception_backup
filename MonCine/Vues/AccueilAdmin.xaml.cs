#region MÉTADONNÉES

// Nom du fichier : Accueil.xaml.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using MonCine.Data.Classes;
using MongoDB.Driver;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace MonCine.Vues
{
    public partial class AccueilAdmin : Page
    {
        #region ATTRIBUTS

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;

        #endregion

        #region CONSTRUCTEURS

        public AccueilAdmin(IMongoClient pClient, IMongoDatabase pDb)
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

        private void BtnDeconnexion_Click(object pSender, RoutedEventArgs pE)
        {
            NavigationService.GoBack();
        }

        #endregion
    }
}