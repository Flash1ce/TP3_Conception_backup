#region MÉTADONNÉES

// Nom du fichier : DALFilm.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MonCine.Data.Classes.BD;
using MonCine.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace MonCine.Data.Classes.DAL
{
    public interface IMAJ : ICRUD<Film>
    {
        #region MÉTHODES

        public bool MAJProjections(Film pFilm);

        #endregion
    }

    public class DALFilm : DAL, IMAJ
    {
        #region ATTRIBUTS

        private readonly DALCategorie _dalCategorie;
        private readonly DALActeur _dalActeur;
        private readonly DALRealisateur _dalRealisateur;
        private DALAbonne _dalAbonne;

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

        #endregion

        public List<Film> ObtenirTout()
        {
            return ObtenirObjetsDansLst(MongoDbContext.ObtenirCollectionListe<Film>(Db));
        }

        public List<Film> ObtenirPlusieurs<TField>(Expression<Func<Film, TField>> pFiltre, List<TField> pObjectIds)
        {
            return ObtenirObjetsDansLst(MongoDbContext.ObtenirDocumentsFiltres(Db, pFiltre, pObjectIds));
        }

        public List<Film> ObtenirObjetsDansLst(List<Film> pFilms)
        {
            // Conserver ce if dans la méthode pour éviter une erreur de type StackOverflow
            if (_dalAbonne == null)
            {
                _dalAbonne = new DALAbonne(_dalCategorie, _dalActeur, _dalRealisateur, this, MongoDbClient, Db);
            }

            foreach (Film film in pFilms)
            {
                List<Categorie> categories =
                    _dalCategorie.ObtenirPlusieurs(pX => pX.Id, new List<ObjectId> { film.CategorieId });
                if (categories.Count > 0)
                {
                    film.Categorie = categories[0];
                }

                film.Acteurs = _dalActeur.ObtenirPlusieurs(pX => pX.Id, film.ActeursId);
                film.Realisateurs = _dalRealisateur.ObtenirPlusieurs(pX => pX.Id, film.RealisateursId);

                List<ObjectId> abonneIds = new List<ObjectId>();
                foreach (Note filmNote in film.Notes)
                {
                    if (!abonneIds.Contains(filmNote.AbonneId))
                    {
                        abonneIds.Add(filmNote.AbonneId);
                    }
                }

                try
                {
                    List<Abonne> abonnes = _dalAbonne.ObtenirPlusieurs(pX => pX.Id, abonneIds);
                    foreach (Note filmNote in film.Notes)
                    {
                        filmNote.Abonne = abonnes.Find(pX => pX.Id == filmNote.AbonneId);
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
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