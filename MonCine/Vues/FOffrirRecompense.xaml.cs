using MonCine.Data.Classes;
using MonCine.Data.Classes.DAL;
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
    /// Logique d'interaction pour FOffrirRecompense.xaml
    /// </summary>
    public partial class FOffrirRecompense : Page
    {
        #region ATTRIBUTS

        private bool _ticketGratuitIsChecked = true;
        private readonly IMongoClient _client;
        private readonly IMongoDatabase _db;
        private readonly DALRecompense _dalRecompense;
        private Film _filmSelectionne;
        private Abonne _abonneSelectionne;
        private Abonne _abonneSelectionneLstSelection;
        private DALFilm _dalFilm;
        private List<Abonne> _abonnes;

        #endregion
        public FOffrirRecompense(IMongoClient pClient, IMongoDatabase pDb, List<Abonne> pAbonnes)
        {
            _client = pClient;
            _abonnes = pAbonnes;
            _db = pDb;
            _dalFilm = new DALFilm(_client, _db);
            _dalRecompense = new DALRecompense(_dalFilm, _client, _db);
            InitializeComponent();
            nbAbonnes.Content = lstAbonnesSelectionner.Items.Count.ToString();
            btnSelectionner.IsEnabled = false;
            btnRetirer.IsEnabled = false;
            RafraichirListeRecompense();
        }
        private void ReinitialiserVisuel()
        {
            lstAbonnesSelectionner.Items.Clear();
            lstAbonnes.Items.Clear();
            lstRecompenses.SelectedIndex = -1;
            nbAbonnes.Content = 0;
            nbPlace.Content = 0;
            btnRetirer.IsEnabled = false;
            btnSelectionner.IsEnabled = false;
            lstRecompenses.IsEnabled = true;
        }
        private void RafraichirListeRecompense()
        {
            ReinitialiserVisuel();
            List<Film> filmsBD = _dalFilm.ObtenirTout();
            if (_ticketGratuitIsChecked)
            {
                foreach (Film film in filmsBD.Where(f=> f.ObtenirNbProjectionsPourAnneeCourante(DateTime.Now.Year)>0))
                {
                    lstRecompenses.Items.Add(film);
                }
            }
            else
            {
                foreach (Film film in filmsBD.Where(f=> f.DateSortie > DateTime.Now))
                {
                    lstRecompenses.Items.Add(film);
                }
            }
        }
        private void btnOffrirRecompense_Click(object sender, RoutedEventArgs e)
        {
            List<Recompense> recompensesAOffrir = new List<Recompense>();
            if(lstAbonnesSelectionner.Items.Count > 0)
            {
                if (_ticketGratuitIsChecked)
                {
                    foreach (Abonne abonne in lstAbonnesSelectionner.Items)
                    {
                        recompensesAOffrir.Add(new TicketGratuit(new MongoDB.Bson.ObjectId(), _filmSelectionne.Id, abonne.Id));
                    }
                    _dalRecompense.InsererPlusieursRecompenses(recompensesAOffrir);
                }
                else
                {
                    foreach (Abonne abonne in lstAbonnesSelectionner.Items)
                    {
                        recompensesAOffrir.Add(new AvantPremiere(new MongoDB.Bson.ObjectId(), _filmSelectionne.Id, abonne.Id));
                    }
                    _dalRecompense.InsererPlusieursRecompenses(recompensesAOffrir);
                }

                ReinitialiserVisuel();
                MessageBox.Show("Offrir Récompense","Récompenses Offertes !", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void lstRecompenses_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool itemIsSelected = lstRecompenses.SelectedIndex > -1;
            _filmSelectionne = itemIsSelected
                ? (Film)lstRecompenses.SelectedItem
                : null;

            if (_filmSelectionne != null)
            {
                lstAbonnes.Items.Clear();
                List<Abonne> abonnesFiltres = new List<Abonne>();
                if (_ticketGratuitIsChecked)
                {
                    abonnesFiltres = _abonnes.OrderByDescending(a => a.NbSeances).ToList().Where(a => a.Preference.Categories.Contains(_filmSelectionne.Categorie)).ToList();
                }
                else
                {
                    List<Abonne> abonnesNonFiltres = _abonnes.OrderByDescending(a => a.NbSeances).ToList();
                    foreach(Acteur acteur in _filmSelectionne.Acteurs)
                    {
                        foreach(Abonne abonne in abonnesNonFiltres.Where(a=> a.Preference.Acteurs.Contains(acteur)))
                        {
                            abonnesFiltres.Add(abonne);
                        }
                    }
                    foreach(Realisateur realisateur in _filmSelectionne.Realisateurs)
                    {
                        foreach(Abonne abonne in abonnesNonFiltres.Where(a=> a.Preference.Realisateurs.Contains(realisateur)))
                        {
                            if (!abonnesFiltres.Contains(abonne))
                            {
                                abonnesFiltres.Add(abonne);
                            }
                        }
                    }
                }
                abonnesFiltres.ForEach(abonneFiltre => lstAbonnes.Items.Add(abonneFiltre));
                try
                {
                    int nbPlaces = _filmSelectionne.Projections.Last().NbPlacesRestantes;
                    nbPlace.Content = nbPlaces.ToString();
                    if (nbPlaces == 0)
                    {
                        btnSelectionner.IsEnabled = false;
                        lstAbonnesSelectionner.IsEnabled = false;
                        lstAbonnes.IsEnabled = false;
                        btnRetirer.IsEnabled = false;
                    }
                    else
                    {
                        lstAbonnes.IsEnabled = true;
                        btnSelectionner.IsEnabled = true;
                        lstAbonnesSelectionner.IsEnabled = true;
                        btnRetirer.IsEnabled = true;
                    }
                    lstAbonnesSelectionner.Items.Clear();
                }
                catch
                {
                    lstAbonnesSelectionner.Items.Clear();
                    lstAbonnes.Items.Clear();
                    nbPlace.Content = 0;
                }
            }
        }

        private void btnSelectionner_Click(object sender, RoutedEventArgs e)
        {
            if (lstAbonnes.SelectedIndex > -1)
            {
                if (!lstAbonnesSelectionner.Items.Contains(_abonneSelectionne))
                {
                    lstAbonnesSelectionner.Items.Add(_abonneSelectionne);
                    lstAbonnes.Items.Remove(_abonneSelectionne);
                    if (_filmSelectionne.Projections.Last().NbPlacesRestantes > 0)
                    {
                        _filmSelectionne.Projections.Last().NbPlacesRestantes = _filmSelectionne.Projections.Last().NbPlacesRestantes - 1;
                    }
                    nbPlace.Content = _filmSelectionne.Projections.Last().NbPlacesRestantes;
                    _dalFilm.MAJUn(f => f.Id == _filmSelectionne.Id, new List<(Expression<Func<Film, object>> field, object value)> { (f => f.Projections, _filmSelectionne.Projections) });
                    if (lstAbonnesSelectionner.Items.Count > 0)
                    {
                        lstRecompenses.IsEnabled = false;
                    }
                }
                nbAbonnes.Content = lstAbonnesSelectionner.Items.Count.ToString();
            }
        }

        private void lstAbonnes_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool itemIsSelected = lstAbonnes.SelectedIndex > -1;
            _abonneSelectionne = itemIsSelected
                ? (Abonne)lstAbonnes.SelectedItem
                : null;

            if (_abonneSelectionne != null)
            {
                btnSelectionner.IsEnabled = true;
            }
            else
            {
                btnSelectionner.IsEnabled = false;
            }
        }

        private void btnRetirer_Click(object sender, RoutedEventArgs e)
        {
            if (lstAbonnesSelectionner.SelectedIndex > -1)
            {
                ajouterAbonnesDansListe(_abonneSelectionneLstSelection);
                lstAbonnesSelectionner.Items.Remove(_abonneSelectionneLstSelection);
                if (lstAbonnesSelectionner.Items.Count == 0)
                {
                    lstRecompenses.IsEnabled = true;
                }
                if (_filmSelectionne.Projections.Last().NbPlacesRestantes > 0)
                {
                    _filmSelectionne.Projections.Last().NbPlacesRestantes = _filmSelectionne.Projections.Last().NbPlacesRestantes + 1;
                }
                nbPlace.Content = _filmSelectionne.Projections.Last().NbPlacesRestantes;
                _dalFilm.MAJUn(f => f.Id == _filmSelectionne.Id, new List<(Expression<Func<Film, object>> field, object value)> { (f => f.Projections, _filmSelectionne.Projections) });
                nbAbonnes.Content = lstAbonnesSelectionner.Items.Count.ToString();
            }
        }
        private void ajouterAbonnesDansListe(Abonne pAbonne)
        {
            int index = 0;
            foreach(Abonne abonne in lstAbonnes.Items)
            {
                if (abonne.NbSeances > pAbonne.NbSeances)
                {
                    index++;
                }
            }
            lstAbonnes.Items.Insert(index, pAbonne);
        }
        private void lstAbonnesSelectionner_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            bool itemIsSelected = lstAbonnesSelectionner.SelectedIndex > -1;
            _abonneSelectionneLstSelection = itemIsSelected
                ? (Abonne)lstAbonnesSelectionner.SelectedItem
                : null;
            if (_abonneSelectionneLstSelection != null)
            {
                btnRetirer.IsEnabled = true;
            }
            else
            {
                btnRetirer.IsEnabled = false;
            }
        }

        private void rbTicketGratuit_Checked(object sender, RoutedEventArgs e)
        {
            _ticketGratuitIsChecked = true;
            lstRecompenses.Items.Clear();
            RafraichirListeRecompense();
        }

        private void rbAvantPremiere_Checked(object sender, RoutedEventArgs e)
        {
            _ticketGratuitIsChecked = false;
            lstRecompenses.Items.Clear();
            RafraichirListeRecompense();
        }
    }
}
