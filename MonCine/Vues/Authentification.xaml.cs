using MonCine.Data.Classes;
using MonCine.Data.Classes.BD;
using MonCine.Data.Classes.DAL;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour Authentification.xaml
    /// </summary>
    public partial class Authentification : Page
    {
        #region ATTRIBUTS

        private DALAdministrateur _dalAdministrateur;
        private DALAbonne _dalAbonne;
        private Administrateur _administrateur;
        private List<Abonne> _abonnes;
        private IMongoClient _client;
        private IMongoDatabase _db;

        #endregion

        #region CONSTRUCTEURS

        public Authentification()
        {
            InitializeComponent();

            _dalAdministrateur = new DALAdministrateur();
            _client = _dalAdministrateur.MongoDbClient;
            _db = _dalAdministrateur.Db;

            SeedData.GenererDonnees(_client, _db);
            _dalAbonne = new DALAbonne(_client, _db);

            // Est nécessaire pour charger à nouveau les utilisateurs au cas où ils auraient été modifié
            // À REVOIR !! puisque l'on est jamais censé ajouter ou supprimer un utilisateur.
            Loaded += OnLoaded;

            // CODE À ANTOINE
            ////_utilisateurs = dalAdministrateur.ObtenirUn();
            ////_utilisateurs = _dalAbonne.ObtenirTout()[0];
            //List<Abonne> abonnes = _dalAbonne.ObtenirTout()
            //foreach (var item in abonnes)
            //{
            //    CboUtilisateurs.Items.Add(item.Nom);
            //}
            //if (_utilisateurs is Abonne)
            //{
            //    _NavigationFrame.Navigate(new Accueil(dalAdministrateur.MongoDbClient, dalAdministrateur.Db, (Abonne)_utilisateurs));
            //}
            //else
            //{
            //    AfficherMsgErreur("Vous n'êtes pas connecté en tant qu'administrateur");
            //}
        }

        #endregion

        #region MÉTHODES

        private void OnLoaded(object pSender, RoutedEventArgs pE)
        {
            _administrateur = _dalAdministrateur.ObtenirUn();
            _abonnes = _dalAbonne.ObtenirTout();

            RbConnexionAdmin.IsChecked = true;
        }

        private void RbConnexionAdmin_Checked(object pSender, RoutedEventArgs pE)
        {
            CboUtilisateurs.Items.Clear();
            CboUtilisateurs.Items.Add(_administrateur);
        }

        private void RbConnexionAbonne_Checked(object pSender, RoutedEventArgs pE)
        {
            CboUtilisateurs.Items.Clear();
            _abonnes.ForEach(x => CboUtilisateurs.Items.Add(x.Nom));
        }

        private void BtnConnexion_Click(object sender, RoutedEventArgs e)
        {
            if (CboUtilisateurs.SelectedIndex < 0)
            {
                MessageBox.Show(
                    "Il faut choisir un utilisateur dans la liste déroulante pour pouvoir se connecter !", "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning
                );
            }
            else
            {
                if (CboUtilisateurs.SelectedItem is Administrateur)
                {
                    NavigationService.Navigate(new AccueilAdmin(_client, _db));
                }
                else
                {
                    string nom = (string)CboUtilisateurs.SelectedItem;
                    NavigationService.Navigate(new AccueilAbonne(_client, _db, _abonnes.Find(x => x.Nom == nom)));
                }
            }
        }

        #endregion
    }
}