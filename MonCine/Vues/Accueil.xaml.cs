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
    public partial class Accueil : Page
    {
        #region ATTRIBUTS

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;
        private Abonne _abonne;

        #endregion

        #region CONSTRUCTEURS

        public Accueil(IMongoClient pClient, IMongoDatabase pDb, Abonne pAbonne = null)
        {
            InitializeComponent();
            _client = pClient;
            _db = pDb;

            bool btnsPourAdminVisibles = pAbonne == null;

            if (!btnsPourAdminVisibles)
            {
                _abonne = pAbonne;
            }

            BtnAbonnes.Visibility = ObtenirVisibilite(btnsPourAdminVisibles);
            BtnPreferences.Visibility = ObtenirVisibilite(!btnsPourAdminVisibles);
            BtnNoter.Visibility = ObtenirVisibilite(!btnsPourAdminVisibles);
        }

        #endregion

        #region MÉTHODES

        private Visibility ObtenirVisibilite(bool pEstVisible)
        {
            return pEstVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private void BtnConsulterAbonne_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FAbonnes(_client, _db));
        }

        private void BtnPreferences_Click(object pSender, RoutedEventArgs pE)
        {
            NavigationService.Navigate(new APreferences(_client, _db, _abonne));
        }

        private void BtnNoter_Click(object pSender, RoutedEventArgs pE)
        {
            NavigationService.Navigate(new FFilms(_client, _db, _abonne));
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