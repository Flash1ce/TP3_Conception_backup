#region MÉTADONNÉES

// Nom du fichier : DALAbonne.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

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
    /// <summary>
    /// Classe représentant une couche d'accès aux données pour les objets de type <see cref="Abonne"/>.
    /// </summary>
    public class DALAbonne : DAL, ICRUD<Abonne>
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
        /// Couche d'accès aux données pour les réservations
        /// </summary>
        private readonly DALReservation _dalReservation;

        #endregion

        #region CONSTRUCTEURS

        /// <summary>
        /// Permet la création de la couche d'accès aux données pour les objets de type <see cref="Abonne"/>.
        /// </summary>
        /// <param name="pClient">L'interface client vers MongoDB</param>
        /// <param name="pDb">Base de données MongoDB utilisée</param>
        public DALAbonne(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = new DALCategorie(MongoDbClient, Db);
            _dalActeur = new DALActeur(MongoDbClient, Db);
            _dalRealisateur = new DALRealisateur(MongoDbClient, Db);
            _dalReservation = new DALReservation(_dalCategorie, _dalActeur, _dalRealisateur, MongoDbClient, Db);
        }

        /// <summary>
        /// Permet la création de la couche d'accès aux données pour les objets de type <see cref="Abonne"/> selon les couches d'accès aux données spécifiés en paramètre.
        /// </summary>
        /// <param name="pDalCategorie">Couche d'accès aux données pour les catégories</param>
        /// <param name="pDalActeur">Couche d'accès aux données pour les acteurs</param>
        /// <param name="pDalRealisateur">Couche d'accès aux données pour les réalisateurs</param>
        /// <param name="pDalFilm">Couche d'accès aux données pour les films</param>
        /// <param name="pClient">L'interface client vers MongoDB</param>
        /// <param name="pDb">Base de données MongoDB utilisée</param>
        /// <remarks>Remarque : <em>Ce constructeur doit être utilisé lorsqu'il existe déjà une instance pour les couches d'accès aux données des catégories, des acteurs et des réalisateurs.</em></remarks>
        public DALAbonne(DALCategorie pDalCategorie, DALActeur pDalActeur, DALRealisateur pDalRealisateur, IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = pDalCategorie;
            _dalActeur = pDalActeur;
            _dalRealisateur = pDalRealisateur;
            _dalReservation = new DALReservation(_dalCategorie, _dalActeur, _dalRealisateur, MongoDbClient, Db);
        }

        /// <summary>
        /// Permet la création de la couche d'accès aux données pour les objets de type <see cref="Abonne"/> selon les couches d'accès aux données spécifiés en paramètre.
        /// </summary>
        /// <param name="pDalCategorie">Couche d'accès aux données pour les catégories</param>
        /// <param name="pDalActeur">Couche d'accès aux données pour les acteurs</param>
        /// <param name="pDalRealisateur">Couche d'accès aux données pour les réalisateurs</param>
        /// <param name="pDalFilm">Couche d'accès aux données pour les films</param>
        /// <param name="pClient">L'interface client vers MongoDB</param>
        /// <param name="pDb">Base de données MongoDB utilisée</param>
        /// <remarks>Remarque : <em>Ce constructeur doit être utilisé lorsqu'il existe déjà une instance pour les couches d'accès aux données des catégories, des acteurs, des réalisateurs et des films.</em></remarks>
        public DALAbonne(DALCategorie pDalCategorie, DALActeur pDalActeur, DALRealisateur pDalRealisateur,
            DALFilm pDalFilm, IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = pDalCategorie;
            _dalActeur = pDalActeur;
            _dalRealisateur = pDalRealisateur;
            _dalReservation = new DALReservation(pDalFilm, MongoDbClient, Db);
        }

        /// <summary>
        /// Permet la création de la couche d'accès aux données pour les objets de type <see cref="Abonne"/> selon les couches d'accès aux données spécifiés en paramètre.
        /// </summary>
        /// <param name="pDalCategorie">Couche d'accès aux données pour les catégories</param>
        /// <param name="pDalActeur">Couche d'accès aux données pour les acteurs</param>
        /// <param name="pDalRealisateur">Couche d'accès aux données pour les réalisateurs</param>
        /// <param name="pReservation">Couche d'accès aux données pour les réservations</param>
        /// <param name="pClient">L'interface client vers MongoDB</param>
        /// <param name="pDb">Base de données MongoDB utilisée</param>
        /// <remarks>Remarque : <em>Ce constructeur doit être utilisé lorsqu'il existe déjà une instance pour les couches d'accès aux données des catégories, des acteurs, des réalisateurs et des réservations.</em></remarks>
        public DALAbonne(DALCategorie pDalCategorie, DALActeur pDalActeur, DALRealisateur pDalRealisateur,
            DALReservation pReservation, IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = pDalCategorie;
            _dalActeur = pDalActeur;
            _dalRealisateur = pDalRealisateur;
            _dalReservation = pReservation;
        }

        #endregion
        
        /// <summary>
        /// Permet d'obtenir la liste des abonnés contenue dans la base de données de la cinémathèque.
        /// </summary>
        /// <returns>La liste des abonnés contenue dans la base de données de la cinémathèque.</returns>
        public List<Abonne> ObtenirTout()
        {
            return ObtenirObjetsDansLst(MongoDbContext.ObtenirCollectionListe<Abonne>(Db));
        }

        
        /// <summary>
        /// Permet de filtrer les abonnés contenus dans la base de données de la cinémathèque selon le champs et les valeurs spécifiés en paramètre.
        /// </summary>
        /// <typeparam name="TField">Type du champs sur lequel le filtrage sera effectué</typeparam>
        /// <param name="pField">Champs sur lequel le filtrage sera effectué</param>
        /// <param name="pObjects">Liste des valeurs à filtrer</param>
        /// <returns>La liste des abonnés filtrée selon le champs et les valeurs spécifiés en paramètre.</returns>
        public List<Abonne> ObtenirPlusieurs<TField>(Expression<Func<Abonne, TField>> pField, List<TField> pObjects)
        {
            return ObtenirObjetsDansLst(MongoDbContext.ObtenirDocumentsFiltres(Db, pField, pObjects));
        }

        /// <summary>
        /// Permet d'obtenir les objets pour les attributs d'un <see cref="Abonne"/> faisant référence à une autre
        /// collection dans la base de données de la cinémathèque pour toute la liste de abonnés spécifiée en paramètre.
        /// </summary>
        /// <param name="pAbonnes">Liste des abonnés</param>
        /// <returns>
        /// La liste des abonnés dont les attributs faisant référence à une autre collection
        /// dans la base de données de la cinémathèque sont à présent définis par des objets non nul.
        /// </returns>
        public List<Abonne> ObtenirObjetsDansLst(List<Abonne> pAbonnes)
        {
            foreach (Abonne abonne in pAbonnes)
            {
                abonne.Preference.Categories =
                    _dalCategorie.ObtenirPlusieurs(x => x.Id, abonne.Preference.CategoriesId);
                abonne.Preference.Acteurs = _dalActeur.ObtenirPlusieurs(x => x.Id, abonne.Preference.ActeursId);
                abonne.Preference.Realisateurs =
                    _dalRealisateur.ObtenirPlusieurs(x => x.Id, abonne.Preference.RealisateursId);
                abonne.NbSeances =
                    _dalReservation.ObtenirNbReservations(x => x.AbonneId, new List<ObjectId> { abonne.Id });
            }
            return pAbonnes;
        }

        /// <summary>
        /// Permet d'insérer la liste des abonnés reçue en paramètre dans la base de données de la cinémathèque.
        /// </summary>
        /// <param name="pAbonnes">Liste des abonnés à insérer dans la base de données</param>
        public bool InsererPlusieurs(List<Abonne> pAbonnes)
        {
            return MongoDbContext.InsererPlusieursDocuments(Db, pAbonnes);
        }

        public bool MAJUn<TField>(Expression<Func<Abonne, bool>> pFiltre, List<(Expression<Func<Abonne, TField>> field, TField value)> pMajDefinitions)
        {
            return MongoDbContext.MAJUn(Db, pFiltre, pMajDefinitions);
        }
    }
}