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
            _abonne = pAbonne;

            InitializeComponent();
            _client = pClient;
            _db = pDb;
            _filmId = pFilmId;
            _dalReservation = new DALReservation(pDalFilm, _client, _db);

            Loaded += OnLoaded;
            AfficheBoutonAjouterOuReserver();
        }

        #endregion

        #region MÉTHODES

        private void OnLoaded(object pSender, RoutedEventArgs pE)
        {
            DALFilm dalFilm = new DALFilm(_client, _db);
            _film = dalFilm.ObtenirUn(_filmId);
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

        private void BtnReserver_Click(object sender, RoutedEventArgs pE)
        {
            // FAIRE REFACTORING
            // Retirer estReserver | Faute d'orthographe sur estReserver
            // AJOUTER UN MESSAGE D'ERREUR POUR UNE PROJECTION DÉSACTIVÉE À L'AJOUT D'UNE RÉSERVATION
            bool estReserver = false;
            int indexProjectionSelectionne = LstProjections.SelectedIndex;
            if (_film.Projections.Count >= 1 && indexProjectionSelectionne > -1)
            {
                Projection projectionSelectionner = _film.Projections[indexProjectionSelectionne];

                if (projectionSelectionner != null &&
                    projectionSelectionner.NbPlacesRestantes >= FProjections.NB_PLACES_RESERVES &&
                    projectionSelectionner.EstActive)
                {
                    Reservation nouvelleReservation = new Reservation(ObjectId.GenerateNewId(), _film,
                        indexProjectionSelectionne, _abonne.Id, FProjections.NB_PLACES_RESERVES);
                    _dalReservation.InsererUn(nouvelleReservation);

                    DALFilm dalFilm = new DALFilm();
                    dalFilm.MAJProjections(_film);

                    LstProjections.Items.Clear();
                    if (_film.Projections.Count > 0)
                    {
                        _film.Projections.ForEach(x => LstProjections.Items.Add(x));
                    }

                    estReserver = true;
                }
            }

            if (estReserver == true)
            {
                MessageBox.Show("La réservation a été ajouté !!", "Confirmation", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("La réservation n'a pas été ajouté !!", "Confirmation", MessageBoxButton.OK,
                    MessageBoxImage.Information);
            }

            Console.WriteLine();
        }

        /// <summary>
        /// Permet d'afficher un bouton ajouter projection quand c'est un admin ou
        /// ajouter un bouton réservé quand c'est un abonne de connecter.
        /// </summary>
        private void AfficheBoutonAjouterOuReserver()
        {
            if (_abonne == null)
            {
                // Affiche le bouton ajouter car est admin
                BtnAjouter.IsEnabled = true;
                BtnAjouter.Visibility = Visibility.Visible;
                // Cache le bouton reserver car est admin
                BtnReserver.IsEnabled = false;
                BtnReserver.Visibility = Visibility.Hidden;
            }
            else
            {
                // Cache le bouton ajouter car est pas admin
                BtnAjouter.IsEnabled = false;
                BtnAjouter.Visibility = Visibility.Hidden;
                // Affiche le bouton reserver car est abonne
                BtnReserver.IsEnabled = true;
                BtnReserver.Visibility = Visibility.Visible;
            }

            // REFACTORING
            //bool affichagePourAdmin = _abonne == null;

            //BtnAjouter.Visibility = ObtenirVisibilite(affichagePourAdmin);
            //BtnReserver.Visibility = ObtenirVisibilite(!affichagePourAdmin);
        }

        #endregion

        // REFACTORING
        //private Visibility ObtenirVisibilite(bool pEstVisible)
        //{
        //    return pEstVisible ? Visibility.Visible : Visibility.Hidden;
        //}
    }
}