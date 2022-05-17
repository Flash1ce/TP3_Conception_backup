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
    /// <summary>
    /// Logique d'interaction pour Accueil.xaml
    /// </summary>
    public partial class AccueilAbonne : Page
    {
        #region ATTRIBUTS

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;
        private readonly Abonne _abonne;

        #endregion

        #region CONSTRUCTEURS

        public AccueilAbonne(IMongoClient pClient, IMongoDatabase pDb, Abonne pAbonne)
        {
            InitializeComponent();
            _client = pClient;
            _db = pDb;
            _abonne = pAbonne;
        }

        #endregion

        #region MÉTHODES

        private void BtnPreferences_Click(object pSender, RoutedEventArgs pE)
        {
            NavigationService.Navigate(new APreferences(_client, _db, _abonne));
        }

        private void BtnNoter_Click(object pSender, RoutedEventArgs pE)
        {
            NavigationService.Navigate(new FNote(_client, _db, _abonne));
        }

        private void BtnConsulterFilms_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FFilms(_client, _db, _abonne));
        }

        private void BtnDeconnexion_Click(object pSender, RoutedEventArgs pE)
        {
            NavigationService.GoBack();
        }

        #endregion
    }
}