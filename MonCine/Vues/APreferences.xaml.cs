#region MÉTADONNÉES

// Nom du fichier : APreferences.xaml.cs
// Auteur :  (1933760)
// Date de création : 2022-05-06
// Date de modification : 2022-05-06

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
    ///     Logique d'interaction pour APreferences.xaml
    /// </summary>
    public partial class APreferences : Page
    {
        #region CONSTANTES ET ATTRIBUTS STATIQUES

        private const string CATEGORIE = "catégorie";
        private const string ACTEUR = "acteur";
        private const string REALISATEUR = "réalisateur";

        #endregion

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

            if (LstCategoriesChoisies.Items.Count == Preference.NB_MAX_CATEGORIES_PREF)
            {
                BtnAjouterCategorie.IsEnabled = false;
            }
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

            if (LstActeursChoisis.Items.Count == Preference.NB_MAX_ACTEURS_PREF)
            {
                BtnAjouterActeur.IsEnabled = false;
            }
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

            if (LstRealisateursChoisis.Items.Count == Preference.NB_MAX_REALISATEURS_PREF)
            {
                BtnAjouterRealisateur.IsEnabled = false;
            }
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
            pBtnLstChangeInactif.IsEnabled = pLstContraire.SelectedIndex > -1 && !estSelectionne;
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
            ActionLstAjouter(
                true,
                APreferences.CATEGORIE,
                Preference.NB_MAX_CATEGORIES_PREF,
                LstCategoriesDispos,
                LstCategoriesChoisies,
                BtnAjouterCategorie,
                _categories,
                _categoriesChoisies
            );
        }

        private void BtnAjouterActeur_Click(object sender, RoutedEventArgs e)
        {
            ActionLstAjouter(
                false,
                APreferences.ACTEUR,
                Preference.NB_MAX_ACTEURS_PREF,
                LstActeursDispos,
                LstActeursChoisis,
                BtnAjouterActeur,
                _acteurs,
                _acteursChoisis
            );
        }

        private void BtnAjouterRealisateur_Click(object sender, RoutedEventArgs e)
        {
            ActionLstAjouter(
                false,
                APreferences.REALISATEUR,
                Preference.NB_MAX_REALISATEURS_PREF,
                LstRealisateursDispos,
                LstRealisateursChoisis,
                BtnAjouterRealisateur,
                _realisateurs,
                _realisateursChoisis
            );
        }

        private void ActionLstAjouter<TDocument>(bool pGenreStrDocumentEstMasculin, string pDocumentASelectionne,
            int pMaxSelectionDocument,
            ListBox pLstDocumentsDispos, ListBox pLstDocumentsChoisis, Button pBtnAjouter, List<TDocument> pDocuments,
            List<TDocument> pDocumentsChoisis)
        {
            if (pLstDocumentsDispos.SelectedIndex == -1)
            {
                AfficherMsgErrDocumentNonSelectionne(pGenreStrDocumentEstMasculin, pDocumentASelectionne, "gauche");
            }
            else if (pLstDocumentsChoisis.Items.Count == pMaxSelectionDocument)
            {
                AfficherMsgErrMaxSelectionDocuments(pGenreStrDocumentEstMasculin, pDocumentASelectionne,
                    pMaxSelectionDocument);
            }
            else
            {
                try
                {
                    TDocument documentChoisi = (TDocument)pLstDocumentsDispos.SelectedItem;
                    pDocumentsChoisis.Add(documentChoisi);
                    ChargerLsts(pLstDocumentsDispos, pLstDocumentsChoisis, pDocuments, pDocumentsChoisis);
                    pLstDocumentsChoisis.SelectedIndex = pLstDocumentsChoisis.Items.IndexOf(documentChoisi);

                    if (pLstDocumentsChoisis.Items.Count == pMaxSelectionDocument)
                    {
                        AfficherMsgMaxSelectionDocuments(pDocumentASelectionne, pMaxSelectionDocument);
                        pBtnAjouter.IsEnabled = false;
                    }
                }
                catch (Exception exception)
                {
                    AfficherMsg(exception.Message, MessageBoxImage.Error);
                }
            }
        }

        private void BtnRetirerCategorie_Click(object sender, RoutedEventArgs e)
        {
            ActionLstRetirer(false, APreferences.CATEGORIE, LstCategoriesDispos, LstCategoriesChoisies,
                _categories, _categoriesChoisies);
        }

        private void BtnRetirerActeur_Click(object sender, RoutedEventArgs e)
        {
            ActionLstRetirer(true, APreferences.ACTEUR, LstActeursDispos, LstActeursChoisis, _acteurs,
                _acteursChoisis);
        }

        private void BtnRetirerRealisateur_Click(object sender, RoutedEventArgs e)
        {
            ActionLstRetirer(true, APreferences.REALISATEUR, LstRealisateursDispos, LstRealisateursChoisis,
                _realisateurs, _realisateursChoisis);
        }

        private void ActionLstRetirer<TDocument>(bool pGenreStrDocumentEstMasculin, string pDocumentASelectionne,
            ListBox pLstDocumentsDispos, ListBox pLstDocumentsChoisis, List<TDocument> pDocuments,
            List<TDocument> pDocumentsChoisis)
        {
            if (pLstDocumentsChoisis.SelectedIndex == -1)
            {
                AfficherMsgErrDocumentNonSelectionne(pGenreStrDocumentEstMasculin, pDocumentASelectionne, "droite");
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
                    AfficherMsg(exception.Message, MessageBoxImage.Error);
                }
            }
        }

        private void ChargerLsts<TDocument>(ListBox pLstDocumentsDispos, ListBox pLstDocumentsChoisis,
            List<TDocument> pDocuments, List<TDocument> pDocumentsChoisis)
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

            List<(Expression<Func<Abonne, object>> field, object value)> filtre =
                new List<(Expression<Func<Abonne, object>> field, object value)>();

            filtre.Add((
                x => x.Preference.CategoriesId,
                categorieIds
            ));

            filtre.Add((
                x => x.Preference.ActeursId,
                acteurIds
            ));

            filtre.Add((
                x => x.Preference.RealisateursId,
                realisateurIds
            ));

            try
            {
                if (filtre.Count > 0)
                {
                    _dalAbonne.MAJUn(x => x.Id == _abonne.Id, filtre);
                    AfficherMsg(
                        "Les modifications ont été enregistrées avec succès !!'",
                        MessageBoxImage.Information
                    );
                }
                else
                {
                    AfficherMsg(
                        "Aucune modification n'a été apportée. Si vous souhaitez retourner à l'accueil, veuillez cliquer sur le bouton 'Retour à l'accueil'",
                        MessageBoxImage.Warning
                    );
                }
            }
            catch (Exception exception)
            {
                AfficherMsg(exception.Message, MessageBoxImage.Error);
            }
        }

        private void AfficherMsgErrDocumentNonSelectionne(bool pGenreStrDocumentEstMasculin, string pDocument,
            string pPositionListe) =>
            AfficherMsg(
                $"Veuillez sélectionner {(pGenreStrDocumentEstMasculin ? "un" : "une")} des {pDocument}s disponibles dans la liste de {pPositionListe}.",
                MessageBoxImage.Warning
            );

        private void AfficherMsgMaxSelectionDocuments(string pDocument, int pMaxDocuments) =>
            AfficherMsg($"Le maximum de {pMaxDocuments} {pDocument}s est atteint.", MessageBoxImage.Information);

        private void AfficherMsgErrMaxSelectionDocuments(bool pGenreStrDocumentEstMasculin, string pDocument,
            int pMaxDocuments) =>
            AfficherMsg(
                $"Impossible d'ajouter {(pGenreStrDocumentEstMasculin ? "un" : "une")} {pDocument} ! Le maximum de {pMaxDocuments} est atteint.",
                MessageBoxImage.Warning
            );

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