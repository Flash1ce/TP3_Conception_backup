#region MÉTADONNÉES

// Nom du fichier : FProjections.xaml.cs
// Date de création : 2022-04-24
// Date de modification : 2022-04-24

#endregion

#region USING

using MonCine.Data.Classes;
using MonCine.Data.Classes.DAL;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour Projections.xaml
    /// </summary>
    public partial class FProjections : Page
    {
        #region ATTRIBUTS

        private IMongoClient _client;
        private IMongoDatabase _db;
        private readonly ObjectId _filmId;
        private Film _film;

        #endregion

        #region CONSTRUCTEURS

        public FProjections(IMongoClient pClient, IMongoDatabase pDb, ObjectId pFilmId)
        {
            InitializeComponent();
            _client = pClient;
            _db = pDb;
            _filmId = pFilmId;

            Loaded += OnLoaded;
        }

        #endregion

        #region MÉTHODES

        private void OnLoaded(object pSender, RoutedEventArgs pE)
        {
            DALFilm dalFilm = new DALFilm(_client, _db);
            _film = dalFilm.ObtenirPlusieurs(x => x.Id, new List<ObjectId> { _filmId }).Find(x => x.Id == _filmId);
            bool filmEstVide = _film == null;
            BtnAjouter.IsEnabled = !filmEstVide;
            if (filmEstVide)
            {
                MessageBox.Show(
                    $"Une erreur s'est produite !!\n\n Aucun film n'a été trouvé pour l'identifiant {_filmId}",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error
                );
            }
            else
            {
                if (_film.Projections.Count > 0)
                {
                    _film.Projections.ForEach(x => LstProjections.Items.Add(x));
                }
            }
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnAjouter_OnClick(object pSender, RoutedEventArgs pE)
        {
            NavigationService.Navigate(new FProgrammerProjection(_film, _client, _db));
        }

        #endregion
    }
}