#region MÉTADONNÉES

// Nom du fichier : DALAbonne.cs
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
    public class DALAbonne : DAL, ICRUD<Abonne>
    {
        #region ATTRIBUTS

        private readonly DALCategorie _dalCategorie;
        private readonly DALActeur _dalActeur;
        private readonly DALRealisateur _dalRealisateur;
        private readonly DALReservation _dalReservation;

        #endregion

        #region CONSTRUCTEURS

        public DALAbonne(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = new DALCategorie(MongoDbClient, Db);
            _dalActeur = new DALActeur(MongoDbClient, Db);
            _dalRealisateur = new DALRealisateur(MongoDbClient, Db);
            _dalReservation = new DALReservation(_dalCategorie, _dalActeur, _dalRealisateur, MongoDbClient, Db);
        }

        public DALAbonne(DALCategorie pDalCategorie, DALActeur pDalActeur, DALRealisateur pDalRealisateur,
            IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = pDalCategorie;
            _dalActeur = pDalActeur;
            _dalRealisateur = pDalRealisateur;
            _dalReservation = new DALReservation(_dalCategorie, _dalActeur, _dalRealisateur, MongoDbClient, Db);
        }

        public DALAbonne(DALCategorie pDalCategorie, DALActeur pDalActeur, DALRealisateur pDalRealisateur,
            DALFilm pDalFilm, IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = pDalCategorie;
            _dalActeur = pDalActeur;
            _dalRealisateur = pDalRealisateur;
            _dalReservation = new DALReservation(pDalFilm, MongoDbClient, Db);
        }

        public DALAbonne(DALCategorie pDalCategorie, DALActeur pDalActeur, DALRealisateur pDalRealisateur,
            DALReservation pReservation, IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalCategorie = pDalCategorie;
            _dalActeur = pDalActeur;
            _dalRealisateur = pDalRealisateur;
            _dalReservation = pReservation;
        }

        #endregion

        public List<Abonne> ObtenirTout()
        {
            return ObtenirObjetsDansLst(MongoDbContext.ObtenirCollectionListe<Abonne>(Db));
        }

        public List<Abonne> ObtenirPlusieurs<TField>(Expression<Func<Abonne, TField>> pField, List<TField> pObjects)
        {
            return ObtenirObjetsDansLst(MongoDbContext.ObtenirDocumentsFiltres(Db, pField, pObjects));
        }

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

        public bool InsererPlusieurs(List<Abonne> pAbonnes)
        {
            return MongoDbContext.InsererPlusieursDocuments(Db, pAbonnes);
        }

        public bool MAJUn<TField>(Expression<Func<Abonne, bool>> pFiltre,
            List<(Expression<Func<Abonne, TField>> field, TField value)> pMajDefinitions)
        {
            return MongoDbContext.MAJUnDocument(Db, pFiltre, pMajDefinitions);
        }
    }
}