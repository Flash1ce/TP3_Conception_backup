#region MÉTADONNÉES

// Nom du fichier : MainWindow.xaml.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using System.Collections.Generic;
using System.Windows;
using MonCine.Data.Classes;
using MonCine.Data.Classes.BD;
using MonCine.Data.Classes.DAL;
using MonCine.Vues;

#endregion

namespace MonCine
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        #region ATTRIBUTS

        private Utilisateur _utilisateurCourant;
        private DALAdministrateur _dalAdministrateur;
        private DALAbonne _dalAbonne;

        #endregion

        #region CONSTRUCTEURS

        public MainWindow()
        {
            InitializeComponent();

            DALAdministrateur dalAdministrateur = new DALAdministrateur();
            _dalAdministrateur = dalAdministrateur;
            SeedData.GenererDonnees(dalAdministrateur.MongoDbClient, dalAdministrateur.Db);
            DALAbonne dalAbonne = new DALAbonne(dalAdministrateur.MongoDbClient, dalAdministrateur.Db);
            _dalAbonne = dalAbonne;
            //_utilisateurCourant = dalAdministrateur.ObtenirUn();
            //_utilisateurCourant = _dalAbonne.ObtenirTout()[0];

            List<Abonne> abonnes = _dalAbonne.ObtenirTout();
            foreach (var item in abonnes)
            {
                lstAbonnes.Items.Add(item.Nom);
            }
            //if (_utilisateurCourant is Abonne)
            //{
            //    _NavigationFrame.Navigate(new Accueil(dalAdministrateur.MongoDbClient, dalAdministrateur.Db, (Abonne)_utilisateurCourant));
            //}
            //else
            //{
            //    AfficherMsgErreur("Vous n'êtes pas connecté en tant qu'administrateur");
            //}
        }

        #endregion

        #region MÉTHODES

        /// <summary>
        /// Permet d'afficher le message reçu en paramètre dans un dialogue pour afficher ce dernier.
        /// </summary>
        /// <param name="pMsg">Message d'erreur à afficher</param>
        private void AfficherMsgErreur(string pMsg)
        {
            MessageBox.Show("Une erreur s'est produite !!\n\n" + pMsg, "Erreur", MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        #endregion

        private void BtnConnecterAdmin_Click(object sender, RoutedEventArgs e)
        {
            _NavigationFrame.Navigate(new Accueil(_dalAdministrateur.MongoDbClient, _dalAdministrateur.Db));
        }

        private void BtnConnecterAbonne_Click(object sender, RoutedEventArgs e)
        {
            if (_utilisateurCourant != null)
            {
                _NavigationFrame.Navigate(new Accueil(_dalAdministrateur.MongoDbClient, _dalAdministrateur.Db, (Abonne)_utilisateurCourant));
            }
        }

        private void lstRecompenses_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            string nom = (string)lstAbonnes.SelectedItem;
            if (!string.IsNullOrEmpty(nom))
            {
                List<Abonne> abonnes = _dalAbonne.ObtenirTout();
                _utilisateurCourant = (Abonne)abonnes.Find(x => x.Nom == nom);
            }
        }
    }
}