using MonCine.Data.Classes;
using MonCine.Data.Classes.BD;
using MonCine.Data.Classes.DAL;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour APreferences.xaml
    /// </summary>
    public partial class APreferences : Page
    {
        #region ATTRIBUTS

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;
        private DALCategorie _dalCategorie;
        private DALActeur _dalActeur;
        private DALRealisateur _dalRealisateur;
        private DALAbonne _dalAbonne;
        private List<Categorie> _categories;
        private List<Categorie> _categoriesChoisies;
        private List<Acteur> _acteurs;
        private List<Realisateur> _realisateurs;
        private List<Acteur> _acteursChoisis;
        private List<Realisateur> _realisateursChoisis;
        private readonly Abonne _abonne;

        #endregion

        #region CONSTRUCTEURS

        public APreferences(IMongoClient pClient, IMongoDatabase pDb, Abonne pAbonne)
        {
            InitializeComponent();
            _client = pClient;
            _db = pDb;
            _abonne = pAbonne;

            InitialiserAttributs();
            InitialiserFormulaire();
            ChargerLsts();
        }

        #endregion

        #region MÉTHODES

        public void InitialiserAttributs()
        {
            _dalCategorie = new DALCategorie(_client, _db);
            _dalActeur = new DALActeur(_client, _db);
            _dalRealisateur = new DALRealisateur(_client, _db);
            _dalAbonne = new DALAbonne(_dalCategorie, _dalActeur, _dalRealisateur, _client, _db);

            _categories = _dalCategorie.ObtenirTout();
            _acteurs = _dalActeur.ObtenirTout();
            _realisateurs = _dalRealisateur.ObtenirTout();

            _categoriesChoisies = _abonne.Preference.Categories;
            _acteursChoisis = _abonne.Preference.Acteurs;
            _realisateursChoisis = _abonne.Preference.Realisateurs;
        }

        public void InitialiserFormulaire()
        {
            BtnAjouterCategorie.IsEnabled = false;
            BtnRetirerCategorie.IsEnabled = false;
            BtnAjouterActeur.IsEnabled = false;
            BtnRetirerActeur.IsEnabled = false;
            BtnAjouterRealisateur.IsEnabled = false;
            BtnRetirerRealisateur.IsEnabled = false;
        }

        public void ChargerLsts()
        {
            // Affecte tous les acteurs n'étant pas choisi
            LstActeursDispos.Items.Clear();
            _acteurs.Where(x => !_acteursChoisis.Contains(x))
                .ToList()
                .ForEach(x => LstActeursDispos.Items.Add(x));

            // Affecte tous les réalisateurs n'étant pas choisi
            LstRealisateursDispos.Items.Clear();
            _realisateurs.Where(x => !_realisateursChoisis.Contains(x))
                .ToList()
                .ForEach(x => LstRealisateursDispos.Items.Add(x));

            // Affecte tous les acteurs choisis
            LstActeursChoisis.Items.Clear();
            _acteursChoisis.ForEach(x => LstActeursChoisis.Items.Add(x));

            // Affecte tous les réalisateurs choisis
            LstRealisateursChoisis.Items.Clear();
            _realisateursChoisis.ForEach(x => LstRealisateursChoisis.Items.Add(x));
        }

        private void LstCategoriesDispos_SelectionChanged(object pSender, SelectionChangedEventArgs pE)
        {
            bool categorieDispoEstSelectionnee = LstCategoriesDispos.SelectedIndex != -1;
            BtnAjouterCategorie.IsEnabled = categorieDispoEstSelectionnee;
            BtnRetirerCategorie.IsEnabled = !categorieDispoEstSelectionnee;
            BtnAjouterActeur.IsEnabled = false;
            BtnRetirerActeur.IsEnabled = false;
            BtnAjouterRealisateur.IsEnabled = false;
            BtnRetirerRealisateur.IsEnabled = false;

            if (categorieDispoEstSelectionnee)
            {
                LstActeursChoisis.SelectedIndex = -1;
            }
        }
        private void LstCategoriesChoisies_SelectionChanged(object pSender, SelectionChangedEventArgs pE)
        {
            bool categorieChoisieEstSelectionnee = LstCategoriesChoisies.SelectedIndex != -1;
            BtnRetirerCategorie.IsEnabled = categorieChoisieEstSelectionnee;
            BtnAjouterCategorie.IsEnabled = !categorieChoisieEstSelectionnee;
            BtnAjouterActeur.IsEnabled = false;
            BtnRetirerActeur.IsEnabled = false;
            BtnAjouterRealisateur.IsEnabled = false;
            BtnRetirerRealisateur.IsEnabled = false;

            if (categorieChoisieEstSelectionnee)
            {
                LstCategoriesDispos.SelectedIndex = -1;
            }
        }

        private void BtnAjouterCategorie_Click(object sender, RoutedEventArgs e)
        {
            if (LstCategoriesDispos.SelectedIndex == -1)
            {
                AfficherMsgErreur("Vous devez sélectionner une des catégories disponibles dans la liste de gauche.");
            }
            else
            {
                try
                {
                    Categorie categorieChoisie = (Categorie)LstCategoriesDispos.SelectedItem;
                    _categoriesChoisies.Add(categorieChoisie);
                    ChargerLsts();
                    LstCategoriesChoisies.SelectedIndex = LstCategoriesChoisies.Items.IndexOf(categorieChoisie);
                }
                catch (Exception exception)
                {
                    AfficherMsgErreur(exception.Message);
                }
            }
        }

        private void BtnRetirerCategorie_Click(object sender, RoutedEventArgs e)
        {
            if (LstCategoriesChoisies.SelectedIndex == -1)
            {
                AfficherMsgErreur("Vous devez sélectionner une des catégories choisies dans la liste de droite.");
            }
            else
            {
                try
                {
                    Categorie categorieDispo = (Categorie)LstActeursChoisis.SelectedItem;
                    _categoriesChoisies.Remove(categorieDispo);
                    ChargerLsts();
                    LstCategoriesDispos.SelectedIndex = LstCategoriesDispos.Items.IndexOf(categorieDispo);
                }
                catch (Exception exception)
                {
                    AfficherMsgErreur(exception.Message);
                }
            }
        }

        private void LstActeursDispos_SelectionChanged(object pSender, SelectionChangedEventArgs pE)
        {
            bool acteurDispoEstSelectionne = LstActeursDispos.SelectedIndex != -1;
            BtnAjouterActeur.IsEnabled = acteurDispoEstSelectionne;
            BtnRetirerActeur.IsEnabled = !acteurDispoEstSelectionne;
            BtnAjouterCategorie.IsEnabled = false;
            BtnRetirerCategorie.IsEnabled = false;
            BtnAjouterRealisateur.IsEnabled = false;
            BtnRetirerRealisateur.IsEnabled = false;

            if (acteurDispoEstSelectionne)
            {
                LstActeursChoisis.SelectedIndex = -1;
            }
        }

        private void LstActeursChoisis_SelectionChanged(object pSender, SelectionChangedEventArgs pE)
        {
            bool acteurChoisiEstSelectionne = LstActeursChoisis.SelectedIndex != -1;
            BtnRetirerActeur.IsEnabled = acteurChoisiEstSelectionne;
            BtnAjouterActeur.IsEnabled = !acteurChoisiEstSelectionne;
            BtnAjouterCategorie.IsEnabled = false;
            BtnRetirerCategorie.IsEnabled = false;
            BtnAjouterRealisateur.IsEnabled = false;
            BtnRetirerRealisateur.IsEnabled = false;

            if (acteurChoisiEstSelectionne)
            {
                LstActeursDispos.SelectedIndex = -1;
            }
        }

        private void BtnAjouterActeur_Click(object sender, RoutedEventArgs e)
        {
            if (LstActeursDispos.SelectedIndex == -1)
            {
                AfficherMsgErreur("Vous devez sélectionner un des acteurs disponibles dans la liste de gauche.");
            }
            else
            {
                try
                {
                    Acteur acteurChoisi = (Acteur)LstActeursDispos.SelectedItem;
                    _acteursChoisis.Add(acteurChoisi);
                    ChargerLsts();
                    LstActeursChoisis.SelectedIndex = LstActeursChoisis.Items.IndexOf(acteurChoisi);
                }
                catch (Exception exception)
                {
                    AfficherMsgErreur(exception.Message);
                }
            }
        }

        private void BtnRetirerActeur_Click(object sender, RoutedEventArgs e)
        {
            if (LstActeursChoisis.SelectedIndex == -1)
            {
                AfficherMsgErreur("Vous devez sélectionner un des acteurs choisis dans la liste de droite.");
            }
            else
            {
                try
                {
                    Acteur acteurDispo = (Acteur)LstActeursChoisis.SelectedItem;
                    _acteursChoisis.Remove(acteurDispo);
                    ChargerLsts();
                    LstActeursDispos.SelectedIndex = LstActeursDispos.Items.IndexOf(acteurDispo);
                }
                catch (Exception exception)
                {
                    AfficherMsgErreur(exception.Message);
                }
            }
        }

        private void LstRealisateursDispos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool realisateurDispoEstSelectionne = LstRealisateursDispos.SelectedIndex != -1;
            BtnAjouterRealisateur.IsEnabled = realisateurDispoEstSelectionne;
            BtnRetirerRealisateur.IsEnabled = !realisateurDispoEstSelectionne;
            BtnAjouterCategorie.IsEnabled = false;
            BtnRetirerCategorie.IsEnabled = false;
            BtnAjouterActeur.IsEnabled = false;
            BtnRetirerActeur.IsEnabled = false;

            if (realisateurDispoEstSelectionne)
            {
                LstRealisateursChoisis.SelectedIndex = -1;
            }
        }

        private void LstRealisateursChoisis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool realisateurChoisiEstSelectionne = LstRealisateursChoisis.SelectedIndex != -1;
            BtnRetirerRealisateur.IsEnabled = realisateurChoisiEstSelectionne;
            BtnAjouterRealisateur.IsEnabled = !realisateurChoisiEstSelectionne;
            BtnAjouterCategorie.IsEnabled = false;
            BtnRetirerCategorie.IsEnabled = false;
            BtnAjouterActeur.IsEnabled = false;
            BtnRetirerActeur.IsEnabled = false;

            if (realisateurChoisiEstSelectionne)
            {
                LstRealisateursDispos.SelectedIndex = -1;
            }
        }

        private void BtnAjouterRealisateur_Click(object sender, RoutedEventArgs e)
        {
            if (LstRealisateursDispos.SelectedIndex == -1)
            {
                AfficherMsgErreur("Vous devez sélectionner un des réalisateurs disponibles dans la liste de gauche.");
            }
            else
            {
                try
                {
                    Realisateur realisateurChoisi = (Realisateur)LstRealisateursDispos.SelectedItem;
                    _realisateursChoisis.Add(realisateurChoisi);
                    ChargerLsts();
                    LstRealisateursChoisis.SelectedIndex = LstRealisateursChoisis.Items.IndexOf(realisateurChoisi);
                }
                catch (Exception exception)
                {
                    AfficherMsgErreur(exception.Message);
                }
            }
        }

        private void BtnRetirerRealisateur_Click(object sender, RoutedEventArgs e)
        {
            if (LstRealisateursChoisis.SelectedIndex == -1)
            {
                AfficherMsgErreur("Vous devez sélectionner un des réalisateurs choisis dans la liste de droite.");
            }
            else
            {
                try
                {
                    Realisateur realisateurDispo = (Realisateur)LstRealisateursChoisis.SelectedItem;
                    _realisateursChoisis.Remove(realisateurDispo);
                    ChargerLsts();
                    LstRealisateursDispos.SelectedIndex = LstRealisateursDispos.Items.IndexOf(realisateurDispo);
                }
                catch (Exception exception)
                {
                    AfficherMsgErreur(exception.Message);
                }
            }
        }

        private void BtnRetour_Click(object pSender, RoutedEventArgs pE)
        {
            NavigationService.GoBack();
        }

        private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            List<ObjectId> categorieIds = new List<ObjectId>();
            _categoriesChoisies.ForEach(x => categorieIds.Add(x.Id));
            List<ObjectId> acteurIds = new List<ObjectId>();
            _acteursChoisis.ForEach(x => acteurIds.Add(x.Id));
            List<ObjectId> realisateurIds = new List<ObjectId>();
            _realisateursChoisis.ForEach(x => realisateurIds.Add(x.Id));

            List<(Expression<Func<Abonne, object>> field, object value)> filtre =
                new List<(Expression<Func<Abonne, object>> field, object value)>();

            if (_abonne.Preference.Categories != _categoriesChoisies)
            {
                filtre.Add((
                    x => x.Preference.CategoriesId,
                    categorieIds
                ));
            }

            if (_abonne.Preference.Acteurs != _acteursChoisis)
            {
                filtre.Add((
                    x => x.Preference.ActeursId,
                    acteurIds
                ));
            }

            if (_abonne.Preference.Realisateurs != _realisateursChoisis)
            {
                filtre.Add((
                    x => x.Preference.RealisateursId,
                    realisateurIds
                ));
            }

            try
            {
                if (filtre.Count > 0)
                {
                    _dalAbonne.MAJUn(x => x.Id == _abonne.Id, filtre);
                    NavigationService.GoBack();
                }
                else
                {
                    AfficherMsgErreur(
                        "Aucune modification n'a été apportée. Si vous souhaitez retourner à l'accueil, veuillez cliquer sur le bouton 'Retour à l'accueil'"
                    );
                }
            }
            catch (Exception exception)
            {
                AfficherMsgErreur(exception.Message);
            }
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