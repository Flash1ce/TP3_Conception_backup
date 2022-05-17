#region MÉTADONNÉES

// Nom du fichier : DALFilm.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MonCine.Data.Classes.BD;
using MonCine.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

#endregion

namespace MonCine.Data.Classes.DAL
{
    public interface IGererFilms : IObtenir<Film>, IObtenirDocumentsComplexes<Film>, IInsererPlusieur<Film>,
        IMAJUn<Film>
    {
        #region MÉTHODES

        public bool MAJProjections(Film pFilm);

        #endregion
    }

    public class DALFilm : DAL, IGererFilms
    {
        #region ATTRIBUTS

        private readonly DALCategorie _dalCategorie;
        private readonly DALActeur _dalActeur;
        private readonly DALRealisateur _dalRealisateur;
        private DALAbonne _dalAbonneCourant;
        private readonly DALAbonne _dalAbonne;
        private readonly IMongoClient _clientReservation;

        #endregion

        #region CONSTRUCTEURS

        public DALFilm(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = new DALCategorie(MongoDbClient, Db);
            _dalActeur = new DALActeur(MongoDbClient, Db);
            _dalRealisateur = new DALRealisateur(MongoDbClient, Db);
        }

        public DALFilm(DALCategorie pDalCategorie, DALActeur pDalActeur, DALRealisateur pDalRealisateur,
            IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = pDalCategorie;
            _dalActeur = pDalActeur;
            _dalRealisateur = pDalRealisateur;
        }

        public DALFilm(DALCategorie pDalCategorie, DALActeur pDalActeur, DALRealisateur pDalRealisateur,
            DALAbonne pDalAbonne, IMongoClient pClientReservation,
            IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = pDalCategorie;
            _dalActeur = pDalActeur;
            _dalRealisateur = pDalRealisateur;
            _dalAbonne = pDalAbonne;
            _clientReservation = pClientReservation;
        }

        #endregion

        public Film ObtenirUn(ObjectId pFilmId)
        {
            return ObtenirPlusieurs(x => x.Id == pFilmId)[0];
        }

        public List<Film> ObtenirPlusieurs(List<ObjectId> pFilmsId)
        {
            return ObtenirPlusieurs(x => pFilmsId.Contains(x.Id));
        }

        public List<Film> ObtenirPlusieurs(Func<Film, bool> pPredicate)
        {
            return ObtenirDocumentsComplexes(MongoDbContext.ObtenirDocumentsFiltres(Db, pPredicate));
        }

        public List<Film> ObtenirTout()
        {
            return ObtenirDocumentsComplexes(MongoDbContext.ObtenirCollectionListe<Film>(Db));
        }

        public List<Film> ObtenirDocumentsComplexes(List<Film> pFilms)
        {
            // Conserver ce if dans la méthode pour éviter une erreur de type StackOverflow
            if (_dalAbonneCourant == null)
            {
                _dalAbonneCourant = _dalAbonne == null
                    ? new DALAbonne(_dalCategorie, _dalActeur, _dalRealisateur, this, MongoDbClient, Db)
                    : new DALAbonne(_dalCategorie, _dalActeur, _dalRealisateur,
                        new DALReservation(this, _clientReservation), _dalAbonne.MongoDbClient);
            }

            List<ObjectId> categorieIds = new List<ObjectId>();
            List<Categorie> categoriesTrouvees = new List<Categorie>();
            List<ObjectId> acteurIds = new List<ObjectId>();
            List<Acteur> acteursTrouves = new List<Acteur>();
            List<ObjectId> realisateursId = new List<ObjectId>();
            List<Realisateur> realisateursTrouves = new List<Realisateur>();
            List<ObjectId> abonneIds = new List<ObjectId>();
            List<Abonne> abonnesTrouves = new List<Abonne>();
            foreach (Film film in pFilms)
            {
                ObjectId categorieId = film.CategorieId;
                int index = categorieIds.IndexOf(categorieId);
                if (index > -1)
                {
                    film.Categorie = categoriesTrouvees[index];
                }
                else
                {
                    film.Categorie = _dalCategorie.ObtenirUn(categorieId);
                    categorieIds.Add(categorieId);
                    categoriesTrouvees.Add(film.Categorie);
                }

                film.Acteurs = new List<Acteur>();
                foreach (ObjectId acteurId in film.ActeursId)
                {
                    index = acteurIds.IndexOf(acteurId);
                    if (index > -1)
                    {
                        film.Acteurs.Add(acteursTrouves[index]);
                    }
                    else
                    {
                        Acteur acteur = _dalActeur.ObtenirUn(acteurId);
                        film.Acteurs.Add(acteur);
                        acteurIds.Add(acteurId);
                        acteursTrouves.Add(acteur);
                    }
                }

                film.Realisateurs = new List<Realisateur>();
                foreach (ObjectId realisateurId in film.RealisateursId)
                {
                    index = realisateursId.IndexOf(realisateurId);
                    if (index > -1)
                    {
                        film.Realisateurs.Add(realisateursTrouves[index]);
                    }
                    else
                    {
                        Realisateur realisateur = _dalRealisateur.ObtenirUn(realisateurId);
                        film.Realisateurs.Add(realisateur);
                        realisateursId.Add(realisateurId);
                        realisateursTrouves.Add(realisateur);
                    }
                }

                foreach (Note filmNote in film.Notes)
                {
                    index = abonneIds.IndexOf(filmNote.AbonneId);
                    if (index > -1)
                    {
                        filmNote.Abonne = abonnesTrouves[index];
                    }
                    else
                    {
                        Abonne abonne = _dalAbonneCourant.ObtenirUn(filmNote.AbonneId);
                        filmNote.Abonne = abonne;
                        abonneIds.Add(filmNote.AbonneId);
                        abonnesTrouves.Add(abonne);
                    }
                }

                int nbDatesFinsAffiche = film.DatesFinsAffiche.Count;
                if (!film.EstAffiche && nbDatesFinsAffiche != film.DatesFinsAffiche.Count)
                {
                    MAJProjections(film);
                }
            }

            return pFilms;
        }

        public bool InsererPlusieurs(List<Film> pFilms)
        {
            return MongoDbContext.InsererPlusieursDocuments(Db, pFilms);
        }

        public bool MAJProjections(Film pFilm)
        {
            foreach (Projection projection in pFilm.Projections)
            {
                if (projection.EstActive && !pFilm.EstAffiche)
                {
                    projection.EstActive = false;
                }
            }

            return MAJUn(
                x => x.Id == pFilm.Id,
                new List<(Expression<Func<Film, object>> field, object value)>
                {
                    (x => x.Projections, pFilm.Projections), (x => x.DatesFinsAffiche, pFilm.DatesFinsAffiche)
                });
        }

        public bool MAJUn<TField>(Expression<Func<Film, bool>> pFiltre,
            List<(Expression<Func<Film, TField>> field, TField value)> pMajDefinitions)
        {
            return MongoDbContext.MAJUnDocument(Db, pFiltre, pMajDefinitions);
        }
    }
}