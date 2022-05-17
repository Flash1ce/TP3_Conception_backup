#region MÉTADONNÉES

// Nom du fichier : FAbonnes.xaml.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using MonCine.Data.Classes;
using MonCine.Data.Classes.DAL;
using MongoDB.Driver;

#endregion

namespace MonCine.Vues
{
    /// <summary>
    /// Logique d'interaction pour FAbonnes.xaml
    /// </summary>
    public partial class FAbonnes : Page
    {
        #region ATTRIBUTS

        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;
        private readonly List<Abonne> _abonnes;
        private readonly DALAbonne _dalAbonne;

        #endregion

        #region CONSTRUCTEURS

        public FAbonnes(IMongoClient pClient, IMongoDatabase pDb)
        {
            InitializeComponent();
            _client = pClient;
            _db = pDb;
            _dalAbonne = new DALAbonne(_client, _db);
            _abonnes = _dalAbonne.ObtenirTout();

            LstAbonnes.Items.Clear();

            _abonnes.OrderByDescending(x => x.NbSeances).ToList().ForEach(film => LstAbonnes.Items.Add(film));
        }

        #endregion

        #region MÉTHODES

        private void btnRetour_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void btnOffrirRecompense_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new FOffrirRecompense(_client, _db, _abonnes));
        }
        #endregion
    }
}