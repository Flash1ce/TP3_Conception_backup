#region MÉTADONNÉES

// Nom du fichier : FFilms.xaml.cs
// Date de création : 2022-04-24
// Date de modification : 2022-04-24

#endregion

#region USING

using MonCine.Data.Classes;
using MonCine.Data.Classes.DAL;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

#endregion

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour Films.xaml
    /// </summary>
    public partial class FFilms : Page
    {
        #region ATTRIBUTS

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;
        private readonly DALFilm _dalFilm;
        private List<Film> _films;
        private Film _filmSelectionne;
        private Abonne _abonne;

        #endregion

        #region CONSTRUCTEURS

        public FFilms(IMongoClient pClient, IMongoDatabase pDb, Abonne pAbonne = null)
        {
            InitializeComponent();

            _client = pClient;
            _db = pDb;
            _dalFilm = new DALFilm(_client, _db);
            _abonne = pAbonne;

            Loaded += OnLoaded;

            if (_abonne == null)
            {
                BtnRetirerDeAffiche.IsEnabled = true;
                BtnRetirerDeAffiche.Visibility = Visibility.Visible;
            }
            else
            {
                BtnRetirerDeAffiche.IsEnabled = false;
                BtnRetirerDeAffiche.Visibility = Visibility.Hidden;
            }
        }

        #endregion

        #region MÉTHODES

        private void OnLoaded(object pSender, RoutedEventArgs pE)
        {
            _films = _dalFilm.ObtenirTout();
            LstFilms.Items.Clear();
            RbTousLesFilms.IsChecked = true;
            if (LstFilms.Items.Count == 0 && _films.Count > 0)
            {
                ChargerLstFilms(!(bool)RbTousLesFilms.IsChecked);
            }
        }

        private void RbTousLesFilm_Checked(object sender, RoutedEventArgs e)
        {
            ChargerLstFilms(false);
        }

        private void RbEstAffiche_Checked(object sender, RoutedEventArgs e)
        {
            ChargerLstFilms(true);
        }

        private void BtnRetourAccueil_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void BtnVoirProjections_Click(object pSender, RoutedEventArgs pE)
        {
            // TODO : Passer en paramètre l'abonné et le DALFilm qui est un attribut déjà existant dans la page

            NavigationService.Navigate(new FProjections(_client, _db, _filmSelectionne.Id, _dalFilm, _abonne));
        }

        private void BtnRetirerDeAffiche_OnClick(object pSender, RoutedEventArgs pE)
        {
            if (_filmSelectionne != null)
            {
                if (_filmSelectionne.EstAffiche && _filmSelectionne.RetirerAffiche())
                {
                    try
                    {
                        _dalFilm.MAJProjections(_filmSelectionne);
                        _films[_films.FindIndex(x => x.Id == _filmSelectionne.Id)] = _filmSelectionne;
                        if (RbTousLesFilms.IsChecked == true)
                        {
                            ChargerLstFilms(!(bool)RbTousLesFilms.IsChecked);
                        }
                        else
                        {
                            ChargerLstFilms((bool)RbEstAffiche.IsChecked);
                        }
                    }
                    catch (Exception e)
                    {
                        AfficherMsgErreur(e.Message);
                    }
                }
                else
                {
                    AfficherMsgErreur("Il a été impossible de retiré le film sélectionné des films à l'affiche.");
                }
            }
        }

        private void LstFilms_OnSelectionChanged(object pSender, SelectionChangedEventArgs pE)
        {
            ActiverBtnsPourFilmSelectionneEstAffiche();
        }

        private void ChargerLstFilms(bool pAffichagePourRbEstAffiche)
        {
            LstFilms.Items.Clear();

            if (_films.Count > 0)
            {
                if (pAffichagePourRbEstAffiche)
                {
                    _films
                        .Where(film => film.EstAffiche)
                        .ToList()
                        .ForEach(film => LstFilms.Items.Add(film));
                }
                else
                {
                    _films.ForEach(film => LstFilms.Items.Add(film));
                }
            }

            ActiverBtnsPourFilmSelectionneEstAffiche();
        }

        private void ActiverBtnsPourFilmSelectionneEstAffiche()
        {
            bool itemIsSelected = LstFilms.SelectedIndex > -1;
            _filmSelectionne = itemIsSelected
                ? (Film)LstFilms.SelectedItem
                : null;

            bool btnsSontActifs = itemIsSelected;

            BtnVoirProjection.IsEnabled = itemIsSelected;

            if (_filmSelectionne != null)
            {
                btnsSontActifs &= _filmSelectionne.EstAffiche;
            }

            BtnRetirerDeAffiche.IsEnabled = btnsSontActifs;
        }

        /// <summary>
        /// Permet d'afficher le message reçu en paramètre dans un dialogue pour afficher ce dernier.
        /// </summary>
        /// <param name="pMsg">Message d'erreur à afficher</param>
        private void AfficherMsgErreur(string pMsg)
        {
            MessageBox.Show(
                "Une erreur s'est produite !!\n\n" + pMsg, "Erreur",
                MessageBoxButton.OK,
                MessageBoxImage.Error
            );
        }

        #endregion
    }
}