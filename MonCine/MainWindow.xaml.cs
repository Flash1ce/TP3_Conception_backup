#region MÉTADONNÉES

// Nom du fichier : MainWindow.xaml.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

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

        private readonly Utilisateur _utilisateurCourant;

        #endregion

        #region CONSTRUCTEURS

        public MainWindow()
        {
            InitializeComponent();

            DALAdministrateur dalAdministrateur = new DALAdministrateur();
            SeedData.GenererDonnees(dalAdministrateur.MongoDbClient, dalAdministrateur.Db);

            _utilisateurCourant = dalAdministrateur.ObtenirUn();

            if (_utilisateurCourant is Administrateur)
            {
                _NavigationFrame.Navigate(new Accueil(dalAdministrateur.MongoDbClient, dalAdministrateur.Db));
            }
            else
            {
                AfficherMsgErreur("Vous n'êtes pas connecté en tant qu'administrateur");
            }
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
    }
}