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

namespace MonCine.Vues
{
    /// <summary>
    ///     Logique d'interaction pour APreferences.xaml
    /// </summary>
    public partial class APreferences : Page
    {
        private const string MSG_SELECTION = "Vous devez sélectionner";
        private const string CATEGORIE_A_SELECTIONNEE = "une des catégories";
        private const string ACTEUR_A_SELECTIONNE = "un des acteurs";
        private const string REALISATEUR_A_SELECTIONNE = "un des réalisateurs";
        private const string MSG_DISPO_DANS_LST = "disponibles dans la liste de";

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

            ChargerLsts(LstCategoriesDispos, LstCategoriesChoisies, _categories, _categoriesChoisies);
            ChargerLsts(LstActeursDispos, LstActeursChoisis, _acteurs, _acteursChoisis);
            ChargerLsts(LstRealisateursDispos, LstRealisateursChoisis, _realisateurs, _realisateursChoisis);
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

        private void LstCategoriesDispos_SelectionChanged(object pSender, SelectionChangedEventArgs pE)
        {
            ActionChangementSelectionSurLst
            (
                LstCategoriesDispos,
                LstCategoriesChoisies,
                BtnAjouterCategorie,
                BtnRetirerCategorie,
                BtnRetirerRealisateur,
                BtnAjouterRealisateur,
                BtnAjouterActeur,
                BtnRetirerActeur
            );
        }

        private void LstActeursDispos_SelectionChanged(object pSender, SelectionChangedEventArgs pE)
        {
            ActionChangementSelectionSurLst
            (
                LstActeursDispos,
                LstActeursChoisis,
                BtnAjouterActeur,
                BtnRetirerActeur,
                BtnRetirerRealisateur,
                BtnAjouterRealisateur,
                BtnAjouterCategorie,
                BtnRetirerCategorie
            );
        }

        private void LstRealisateursDispos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActionChangementSelectionSurLst
            (
                LstRealisateursDispos,
                LstRealisateursChoisis,
                BtnAjouterRealisateur,
                BtnRetirerRealisateur,
                BtnAjouterCategorie,
                BtnRetirerCategorie,
                BtnAjouterActeur,
                BtnRetirerActeur
            );
        }

        private void LstCategoriesChoisies_SelectionChanged(object pSender, SelectionChangedEventArgs pE)
        {
            ActionChangementSelectionSurLst
            (
                LstCategoriesChoisies,
                LstCategoriesDispos,
                BtnRetirerCategorie,
                BtnAjouterCategorie,
                BtnRetirerRealisateur,
                BtnAjouterRealisateur,
                BtnAjouterActeur,
                BtnRetirerActeur
            );
        }

        private void LstActeursChoisis_SelectionChanged(object pSender, SelectionChangedEventArgs pE)
        {
            ActionChangementSelectionSurLst
            (
                LstActeursChoisis,
                LstActeursDispos,
                BtnRetirerActeur,
                BtnAjouterActeur,
                BtnRetirerRealisateur,
                BtnAjouterRealisateur,
                BtnAjouterCategorie,
                BtnRetirerCategorie
            );
        }

        private void LstRealisateursChoisis_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ActionChangementSelectionSurLst
            (
                LstRealisateursChoisis,
                LstRealisateursDispos,
                BtnRetirerRealisateur,
                BtnAjouterRealisateur,
                BtnAjouterCategorie,
                BtnRetirerCategorie,
                BtnAjouterActeur,
                BtnRetirerActeur
            );
        }

        private void ActionChangementSelectionSurLst(ListBox pLstChange, ListBox pLstContraire,
            Button pBtnLstChangeActif, Button pBtnLstChangeInactif,
            Button pBtnAjouterAutre1, Button pBtnRetirerAutre1, Button pBtnAjouterAutre2, Button pBtnRetirerAutre2)
        {
            bool estSelectionne = pLstChange.SelectedIndex != -1;

            pBtnLstChangeActif.IsEnabled = estSelectionne;
            pBtnLstChangeInactif.IsEnabled = !estSelectionne;
            pBtnAjouterAutre1.IsEnabled = false;
            pBtnRetirerAutre1.IsEnabled = false;
            pBtnAjouterAutre2.IsEnabled = false;
            pBtnRetirerAutre2.IsEnabled = false;

            if (estSelectionne)
            {
                pLstContraire.SelectedIndex = -1;
            }
        }

        private void BtnAjouterCategorie_Click(object sender, RoutedEventArgs e)
        {
            ActionLstAjouter(APreferences.CATEGORIE_A_SELECTIONNEE, LstCategoriesDispos, LstCategoriesChoisies, _categories, _categoriesChoisies);
        }

        private void BtnAjouterActeur_Click(object sender, RoutedEventArgs e)
        {
            ActionLstAjouter(APreferences.ACTEUR_A_SELECTIONNE, LstActeursDispos, LstActeursChoisis, _acteurs, _acteursChoisis);
        }

        private void BtnAjouterRealisateur_Click(object sender, RoutedEventArgs e)
        {
            ActionLstAjouter(APreferences.REALISATEUR_A_SELECTIONNE, LstRealisateursDispos, LstRealisateursChoisis, _realisateurs, _realisateursChoisis);
        }

        private void ActionLstAjouter<TDocument>(string pDocumentASelectionne, ListBox pLstDocumentsDispos, ListBox pLstDocumentsChoisis, List<TDocument> pDocuments, List<TDocument> pDocumentsChoisis)
        {
            if (pLstDocumentsDispos.SelectedIndex == -1)
            {
                AfficherMsgErreur($"{APreferences.MSG_SELECTION} {pDocumentASelectionne} {APreferences.MSG_DISPO_DANS_LST} gauche.");
            }
            else
            {
                try
                {
                    TDocument documentChoisi = (TDocument)pLstDocumentsDispos.SelectedItem;
                    pDocumentsChoisis.Add(documentChoisi);
                    ChargerLsts(pLstDocumentsDispos, pLstDocumentsChoisis, pDocuments, pDocumentsChoisis);
                    pLstDocumentsChoisis.SelectedIndex = pLstDocumentsChoisis.Items.IndexOf(documentChoisi);
                }
                catch (Exception exception)
                {
                    AfficherMsgErreur(exception.Message);
                }
            }
        }

        private void BtnRetirerCategorie_Click(object sender, RoutedEventArgs e)
        {
            ActionLstRetirer(APreferences.CATEGORIE_A_SELECTIONNEE, LstCategoriesDispos, LstCategoriesChoisies, _categories, _categoriesChoisies);
        }

        private void BtnRetirerActeur_Click(object sender, RoutedEventArgs e)
        {
            ActionLstRetirer(APreferences.ACTEUR_A_SELECTIONNE, LstActeursDispos, LstActeursChoisis, _acteurs, _acteursChoisis);
        }

        private void BtnRetirerRealisateur_Click(object sender, RoutedEventArgs e)
        {
            ActionLstRetirer(APreferences.REALISATEUR_A_SELECTIONNE, LstRealisateursDispos, LstRealisateursChoisis, _realisateurs, _realisateursChoisis);
        }

        private void ActionLstRetirer<TDocument>(string pDocumentASelectionne, ListBox pLstDocumentsDispos, ListBox pLstDocumentsChoisis, List<TDocument> pDocuments, List<TDocument> pDocumentsChoisis)
        {
            if (pLstDocumentsChoisis.SelectedIndex == -1)
            {
                AfficherMsgErreur($"{APreferences.MSG_SELECTION} {pDocumentASelectionne} {APreferences.MSG_DISPO_DANS_LST} droite.");
            }
            else
            {
                try
                {
                    TDocument documentDispo = (TDocument)pLstDocumentsChoisis.SelectedItem;
                    pDocumentsChoisis.Remove(documentDispo);
                    ChargerLsts(pLstDocumentsDispos, pLstDocumentsChoisis, pDocuments, pDocumentsChoisis);
                    pLstDocumentsDispos.SelectedIndex = pLstDocumentsDispos.Items.IndexOf(documentDispo);
                }
                catch (Exception exception)
                {
                    AfficherMsgErreur(exception.Message);
                }
            }
        }

        private void ChargerLsts<TDocument>(ListBox pLstDocumentsDispos, ListBox pLstDocumentsChoisis, List<TDocument> pDocuments, List<TDocument> pDocumentsChoisis)
        {
            ObtenirLstDocumentsDispos(pLstDocumentsDispos, pDocuments, pDocumentsChoisis);
            ObtenirLstDocumentsChoisis(pLstDocumentsChoisis, pDocumentsChoisis);
        }

        private void ObtenirLstDocumentsDispos<TDocument>(ListBox pLstDocumentsDispos, List<TDocument> pDocuments,
            List<TDocument> pDocumentsChoisis)
        {
            // Affecte tous les objets n'étant pas choisis
            pLstDocumentsDispos.Items.Clear();
            pDocuments.Where(x => !pDocumentsChoisis.Contains(x))
                .ToList()
                .ForEach(x => pLstDocumentsDispos.Items.Add(x));
        }

        private void ObtenirLstDocumentsChoisis<TDocument>(ListBox pLstDocumentsChoisis,
            List<TDocument> pDocumentsChoisis)
        {
            pLstDocumentsChoisis.Items.Clear();
            pDocumentsChoisis.ForEach(x => pLstDocumentsChoisis.Items.Add(x));
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

            List<(Expression<Func<Abonne, object>> field, object value)> filtre = new List<(Expression<Func<Abonne, object>> field, object value)>();

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
        ///     Permet d'afficher le message reçu en paramètre dans un dialogue pour afficher ce dernier.
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