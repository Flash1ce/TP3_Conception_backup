#region MÉTADONNÉES

// Nom du fichier : FFilms.xaml.cs
// Date de création : 2022-04-24
// Date de modification : 2022-04-24

#endregion

#region USING

using MonCine.Data.Classes;
using MonCine.Data.Classes.DAL;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour Films.xaml
    /// </summary>
    public partial class FNote : Page
    {
        #region ATTRIBUTS

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;
        private readonly DALFilm _dalFilm;
        private readonly DALReservation _dalReservation;
        private Abonne _abonne;

        #endregion

        #region CONSTRUCTEURS

        public FNote(IMongoClient pClient, IMongoDatabase pDb, Abonne pAbonne)
        {
            InitializeComponent();

            _client = pClient;
            _db = pDb;
            _dalFilm = new DALFilm(_client, _db);
            _dalReservation = new DALReservation(_dalFilm, _client, _db);
            _abonne = pAbonne;

            Loaded += OnLoaded;
        }

        #endregion

        #region MÉTHODES

        private void OnLoaded(object pSender, RoutedEventArgs pE)
        {
            LstFilms.Items.Clear();

            // Obtient tous les films pour l'abonné connecté
            _dalReservation
                .ObtenirPlusieurs(x => x.AbonneId, new List<ObjectId>{_abonne.Id})
                .ForEach(x =>
                {
                    // Ajoute tous les films assistés de l'abonné connecté
                    if (x.Film.Projections[x.IndexProjectionFilm].DateFin < DateTime.Now)
                        LstFilms.Items.Add(x.Film);
                });
        }

        private void LstFilms_OnSelectionChanged(object pSender, SelectionChangedEventArgs pE)
        {
            TxtNote.IsEnabled = LstFilms.SelectedIndex > -1;
        }

        private void BtnRetourAccueil_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnNoter_Click(object pSender, RoutedEventArgs pE)
        {
            if (ValiderForm())
            {
                Film filmPourNote = (Film)LstFilms.SelectedItem;
                filmPourNote.Notes.Add(new Note(_abonne.Id, int.Parse(TxtNote.Text)));
                _dalFilm.MAJUn(x => x.Id == _abonne.Id, new List<(Expression<Func<Film, object>> field, object value)>
                {
                    (
                        x => x.Notes,
                        filmPourNote.Notes
                    )
                });
                AfficherMsg(
                    "Les modifications ont été enregistrées avec succès !!'",
                    MessageBoxImage.Information
                );
            }
        }

        private bool ValiderForm()
        {
            if (LstFilms.SelectedIndex < 0)
                AfficherMsg("Il vous faut sélectionner un film pour le noter.", MessageBoxImage.Error);
            else if (string.IsNullOrWhiteSpace(TxtNote.Text))
                AfficherMsg("Il vous faut saisir une note entre 1 et 10", MessageBoxImage.Error);
            else if (int.Parse(TxtNote.Text) < 1)
                AfficherMsg("Veuillez saisir une note supérieur à 0", MessageBoxImage.Error);
            else if (int.Parse(TxtNote.Text) > 10)
                AfficherMsg("Veuillez saisir une note égale ou inférieure à 10.", MessageBoxImage.Error);
            else return true;

            return false;
        }

        /// <summary>
        /// Permet d'afficher le message reçu en paramètre dans un dialogue pour afficher ce dernier.
        /// </summary>
        /// <param name="pMsg">Message d'erreur à afficher</param>
        private void AfficherMsg(string pMsg, MessageBoxImage msgBxImg)
        {
            MessageBox.Show(
                "Attention !!\n\n" + pMsg, "Message",
                MessageBoxButton.OK,
                msgBxImg
            );
        }

        #endregion
    }
}