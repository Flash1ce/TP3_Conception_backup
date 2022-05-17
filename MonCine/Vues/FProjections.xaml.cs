#region MÉTADONNÉES

// Nom du fichier : FProjections.xaml.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System;
using System.Windows;
using System.Windows.Controls;
using MonCine.Data.Classes;
using MonCine.Data.Classes.DAL;
using MongoDB.Bson;
using MongoDB.Driver;

#endregion

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour Projections.xaml
    /// </summary>
    public partial class FProjections : Page
    {
        #region CONSTANTES ET ATTRIBUTS STATIQUES

        private const int NB_PLACES_RESERVES = 1;

        #endregion

        #region ATTRIBUTS

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;
        private readonly ObjectId _filmId;
        private Film _film;
        private readonly Abonne _abonne;
        private readonly DALReservation _dalReservation;

        #endregion

        #region CONSTRUCTEURS

        /// <summary>
        /// 
        /// </summary>
        /// <param name="pClient"></param>
        /// <param name="pDb"></param>
        /// <param name="pFilmId"></param>
        /// <param name="pAbonne">si pAbonne est null, c'est un admin sinon c'est l'abonne connecter.</param>
        public FProjections(IMongoClient pClient, IMongoDatabase pDb, ObjectId pFilmId, DALFilm pDalFilm,
            Abonne pAbonne = null)
        {
            InitializeComponent();
            _client = pClient;
            _db = pDb;
            _filmId = pFilmId;
            _dalReservation = new DALReservation(pDalFilm, _client, _db);
            _abonne = pAbonne;

            Loaded += OnLoaded;
        }

        #endregion

        #region MÉTHODES

        private void OnLoaded(object pSender, RoutedEventArgs pE)
        {
            DALFilm dalFilm = new DALFilm(_client, _db);
            _film = dalFilm.ObtenirUn(_filmId);
            AfficheBoutonAjouterOuReserver();
        }

        private void LstProjections_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool projectionValide = LstProjections.SelectedIndex != -1 && _film.Projections.Count >= 1 
                && _film.Projections[LstProjections.SelectedIndex].EstActive;
            BtnReserver.IsEnabled = projectionValide;
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnAjouter_OnClick(object pSender, RoutedEventArgs pE)
        {
            NavigationService.Navigate(new FProgrammerProjection(_film, _client, _db));
        }

        private void BtnReserver_Click(object sender, RoutedEventArgs pE)
        {
            if (_film.Projections[LstProjections.SelectedIndex].EstActive)
            {
                // Crée la réservation et affiche le bon message dans le message box.
                AfficherMsg(ReserverProjection(LstProjections.SelectedIndex) 
                    ? "La réservation a été ajouté !!" : "La réservation n'a pas été ajouté !!", MessageBoxImage.Warning);
            }
            else
            {
                AfficherMsg("La projection ne peut pas être réservé !!", MessageBoxImage.Warning);
            }
        }

        private bool ReserverProjection(int index)
        {
            Projection projectionSelectionner = _film.Projections[index];
            bool projectionSelectionnerNonNullEtPlaceDisponible = projectionSelectionner != null && projectionSelectionner.NbPlacesRestantes >= NB_PLACES_RESERVES && projectionSelectionner.EstActive;
            if (projectionSelectionnerNonNullEtPlaceDisponible)
            {
                _dalReservation.InsererUn(
                    new Reservation(new ObjectId(), _film, index, _abonne.Id, FProjections.NB_PLACES_RESERVES));
                DALFilm dalFilm = new DALFilm();
                dalFilm.MAJProjections(_film);
                RegenererListeProjections();
            }
            return projectionSelectionnerNonNullEtPlaceDisponible;
        }

        /// <summary>
        /// Permet d'afficher un bouton ajouter projection quand c'est un admin ou
        /// ajouter un bouton réservé quand c'est un abonne de connecter.
        /// </summary>
        private void AfficheBoutonAjouterOuReserver()
        {
            bool filmEstVide = _film == null;
            BtnAjouter.IsEnabled = !filmEstVide;
            bool affichagePourAdmin = _abonne == null;
            BtnAjouter.Visibility = ObtenirVisibilite(affichagePourAdmin);
            BtnReserver.Visibility = ObtenirVisibilite(!affichagePourAdmin);
            if (filmEstVide)
            {
                AfficherMsg($"Aucun film n'a été trouvé pour l'identifiant {_filmId}", MessageBoxImage.Warning);
            }
            else
            {
                RegenererListeProjections();
            }
        }

        private Visibility ObtenirVisibilite(bool pEstVisible)
        {
            return pEstVisible ? Visibility.Visible : Visibility.Hidden;
        }

        private void RegenererListeProjections()
        {
            LstProjections.Items.Clear();
            if (_film.Projections.Count > 0)
            {
                _film.Projections.ForEach(x => LstProjections.Items.Add(x));
            }
        }

        private void AfficherMsg(string pMsg, MessageBoxImage msgBxImg)
        {
            MessageBox.Show(pMsg, "Information", MessageBoxButton.OK, msgBxImg);
        }

        #endregion
    }
}