#region MÉTADONNÉES

// Nom du fichier : FNote.xaml.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Windows;
using System.Windows.Controls;
using MonCine.Data.Classes;
using MonCine.Data.Classes.DAL;
using MongoDB.Driver;

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
        private readonly List<Film> _films;
        private readonly Abonne _abonne;
        private int _indexAbonneNote = -1;

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
            TxtNote.IsEnabled = false;
            BtnNoter.IsEnabled = false;
            _films = new List<Film>();

            Loaded += OnLoaded;
        }

        #endregion

        #region MÉTHODES

        private void OnLoaded(object pSender, RoutedEventArgs pE)
        {
            // Obtient tous les films pour l'abonné connecté
            List<Reservation> reservations = _dalReservation.ObtenirPlusieurs(x => x.AbonneId == _abonne.Id);
            if (reservations.Count > 0)
                reservations.ForEach(x =>
                {
                    List<Film> filmsAssistes = new List<Film>();
                    // Ajoute tous les films assistés de l'abonné connecté
                    if (x.Film.Projections[x.IndexProjectionFilm].DateFin < DateTime.Now &&
                        !filmsAssistes.Contains(x.Film))
                    {
                        _films.Add(x.Film);
                        filmsAssistes.Add(x.Film);
                    }
                });

            RegenererLstFilms();
        }

        private void LstFilms_OnSelectionChanged(object pSender, SelectionChangedEventArgs pE)
        {
            int indexAucuneSelection = -1;
            bool filmEstSelectionne = LstFilms.SelectedIndex > indexAucuneSelection;
            TxtNote.IsEnabled = filmEstSelectionne;
            BtnNoter.IsEnabled = filmEstSelectionne;
            _indexAbonneNote = indexAucuneSelection;
            if (filmEstSelectionne)
            {
                List<Note> notes = ((Film)LstFilms.SelectedItem).Notes;
                int i = 0;
                while (i < notes.Count && _indexAbonneNote == indexAucuneSelection)
                {
                    if (notes[i].AbonneId == _abonne.Id)
                    {
                        _indexAbonneNote = i;
                        TxtNote.Text = notes[i].NoteFilm.ToString();
                    }

                    i++;
                }
            }
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
                int note = int.Parse(TxtNote.Text);
                if (_indexAbonneNote > -1)
                {
                    filmPourNote.Notes[_indexAbonneNote].NoteFilm = note;
                }
                else
                {
                    filmPourNote.Notes.Add(new Note(_abonne.Id, note));
                }

                _dalFilm.MAJUn(x => x.Id == filmPourNote.Id,
                    new List<(Expression<Func<Film, object>> field, object value)>
                    {
                        (
                            x => x.Notes,
                            filmPourNote.Notes
                        )
                    });
                RegenererLstFilms();
                AfficherMsg(
                    "Les modifications ont été enregistrées avec succès !!'",
                    MessageBoxImage.Information
                );
            }
        }

        private void RegenererLstFilms()
        {
            LstFilms.Items.Clear();
            TxtNote.Text = "";
            _films.ForEach(x => LstFilms.Items.Add(x));
        }

        private bool ValiderForm()
        {
            if (LstFilms.SelectedIndex < 0)
            {
                AfficherMsg("Il vous faut sélectionner un film pour le noter.", MessageBoxImage.Warning);
            }
            else if (string.IsNullOrWhiteSpace(TxtNote.Text))
            {
                AfficherMsg("Il vous faut saisir une note entre 1 et 10", MessageBoxImage.Warning);
            }
            else if (!int.TryParse(TxtNote.Text, out int _))
            {
                AfficherMsg("Il vous faut saisir valeur numérique", MessageBoxImage.Warning);
            }
            else if (int.Parse(TxtNote.Text) < 1)
            {
                AfficherMsg("Veuillez saisir une note supérieur à 0", MessageBoxImage.Warning);
            }
            else if (int.Parse(TxtNote.Text) > 10)
            {
                AfficherMsg("Veuillez saisir une note égale ou inférieure à 10.", MessageBoxImage.Warning);
            }
            else
            {
                return true;
            }

            return false;
        }

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