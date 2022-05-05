#region MÉTADONNÉES

// Nom du fichier : DALFilm.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

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
        public bool MAJProjections(Film pFilm);
    }

    /// <summary>
    /// Classe représentant une couche d'accès aux données pour les objets de type <see cref="Film"/>.
    /// </summary>
    public class DALFilm : DAL, IMAJ
    {
        #region ATTRIBUTS

        /// <summary>
        /// Couche d'accès aux données pour les catégories
        /// </summary>
        private readonly DALCategorie _dalCategorie;

        /// <summary>
        /// Couche d'accès aux données pour les acteurs
        /// </summary>
        private readonly DALActeur _dalActeur;

        /// <summary>
        /// Couche d'accès aux données pour les réalisateurs
        /// </summary>
        private readonly DALRealisateur _dalRealisateur;

        /// <summary>
        /// Couche d'accès aux données pour les abonnés
        /// </summary>
        private DALAbonne _dalAbonne;

        #endregion

        #region CONSTRUCTEURS

        /// <summary>
        /// Permet la création de la couche d'accès aux données pour les objets de type <see cref="Film"/>.
        /// </summary>
        /// <param name="pClient">L'interface client vers MongoDB</param>
        /// <param name="pDb">Base de données MongoDB utilisée</param>
        public DALFilm(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = new DALCategorie(MongoDbClient, Db);
            _dalActeur = new DALActeur(MongoDbClient, Db);
            _dalRealisateur = new DALRealisateur(MongoDbClient, Db);
        }

        /// <summary>
        /// Permet la création de la couche d'accès aux données pour les objets de type <see cref="Film"/> selon les couches d'accès aux données spécifiés en paramètre.
        /// </summary>
        /// <param name="pDalCategorie">Couche d'accès aux données pour les catégories</param>
        /// <param name="pDalActeur">Couche d'accès aux données pour les acteurs</param>
        /// <param name="pDalRealisateur">Couche d'accès aux données pour les réalisateurs</param>
        /// <param name="pClient">L'interface client vers MongoDB</param>
        /// <param name="pDb">Base de données MongoDB utilisée</param>
        /// <remarks>Remarque : <em>Ce constructeur doit être utilisé lorsqu'il existe déjà une instance pour les couches d'accès aux données des catégories, des acteurs et des réalisateurs.</em></remarks>
        public DALFilm(DALCategorie pDalCategorie, DALActeur pDalActeur, DALRealisateur pDalRealisateur,
            IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = pDalCategorie;
            _dalActeur = pDalActeur;
            _dalRealisateur = pDalRealisateur;
        }

        #endregion

        #region MÉTHODES

        /// <summary>
        /// Permet d'obtenir la liste des films contenue dans la base de données de la cinémathèque.
        /// </summary>
        /// <returns>La liste des films contenue dans la base de données de la cinémathèque.</returns>
        public List<Film> ObtenirTout()
        {
            return ObtenirObjetsDansLst(MongoDbContext.ObtenirCollectionListe<Film>(Db));
        }

        /// <summary>
        /// Permet de filtrer les films contenues dans la base de données de la cinémathèque selon le champs et les valeurs spécifiés en paramètre.
        /// </summary>
        /// <typeparam name="TField">Type du champs sur lequel le filtrage sera effectué</typeparam>
        /// <param name="pFiltre">Champs sur lequel le filtrage sera effectué</param>
        /// <param name="pObjectIds">Liste des valeurs à filtrer</param>
        /// <returns>La liste des films filtrée selon le champs et les valeurs spécifiés en paramètre.</returns>
        public List<Film> ObtenirPlusieurs<TField>(Expression<Func<Film, TField>> pFiltre, List<TField> pObjectIds)
        {
            return ObtenirObjetsDansLst(MongoDbContext.ObtenirDocumentsFiltres(Db, pFiltre, pObjectIds));
        }

        /// <summary>
        /// Permet d'obtenir les objets pour les attributs d'un <see cref="Film"/> faisant référence à une autre
        /// collection dans la base de données de la cinémathèque pour toute la liste de films spécifiée en paramètre.
        /// </summary>
        /// <param name="pFilms">Liste des films</param>
        /// <returns>
        /// La liste des films dont les attributs faisant référence à une autre collection
        /// dans la base de données de la cinémathèque sont à présent définis par des objets non nul.
        /// </returns>
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
                film.Acteurs = _dalActeur.ObtenirActeursFiltres(pX => pX.Id, film.ActeursId);
                film.Realisateurs = _dalRealisateur.ObtenirRealisateursFiltres(pX => pX.Id, film.RealisateursId);
                //Les deux boucles permettent de faire moins de requête à la base de données ce qui permet d'accélérer le temps de traitement
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
            }

            return pFilms;
        }

        /// <summary>
        /// Permet d'insérer la liste des films reçue en paramètre dans la base de données de la cinémathèque.
        /// </summary>
        /// <param name="pFilms">Liste des films à insérer dans la base de données de la cinémathèque</param>
        public bool InsererPlusieurs(List<Film> pFilms)
        {
            return MongoDbContext.InsererPlusieursDocuments(Db, pFilms);
        }

        /// <summary>
        /// Permet de mettre à jour la liste des projections du film spécifié en paramètre. 
        /// </summary>
        /// <param name="pFilm">Film à mettre la liste des projections à jour</typeparam>
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
                {(x => x.Projections,pFilm.Projections),(x=> x.DatesFinsAffiche,pFilm.DatesFinsAffiche)});
        }

        /// <summary>
        /// Permet de mettre à jour un seul film par un filtre et la liste des champs et de ses valeurs spéficiés en paramètre. 
        /// </summary>
        /// <typeparam name="TField">Type du champs</typeparam>
        /// <param name="pFiltre">Expression permettant de déterminer quel sera le film à mettre à jour</param>
        /// <param name="pMajDefinitions">Liste des champs et de ses valeurs à mettre à jour</param>
        /// <returns>Vrai si la mise à jour du film a fonctionné. Faux dans le cas contraire.</returns>
        public bool MAJUn<TField>(Expression<Func<Film, bool>> pFiltre, List<(Expression<Func<Film, TField>> field, TField value)> pMajDefinitions)
        {
            return MongoDbContext.MAJUn(Db, pFiltre, pMajDefinitions);
        }

        #endregion
    }
}