#region MÉTADONNÉES

// Nom du fichier : DALReservation.cs
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
    public class DALReservation : DAL, ICRUD<Reservation>
    {
        #region ATTRIBUTS

        private readonly DALFilm _dalFilm;

        #endregion

        #region CONSTRUCTEURS

        public DALReservation(DALCategorie pDalCategorie, DALActeur pDalActeur, DALRealisateur pDalRealisateur,
            IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
            _dalFilm = new DALFilm(pDalCategorie, pDalActeur, pDalRealisateur, MongoDbClient, Db);
        }

        public DALReservation(DALFilm pDalFilm, IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient,
            pDb)
        {
            _dalFilm = pDalFilm;
        }

        #endregion

        #region MÉTHODES

        public int ObtenirNbReservations<TField>(Expression<Func<Reservation, TField>> pField, List<TField> pObjects)
        {
            return MongoDbContext.ObtenirDocumentsFiltres(Db, pField, pObjects).Count;
        }

        public void InsererUne(Reservation pReservation)
        {
            _dalFilm.MAJProjections(pReservation.Film);
            MongoDbContext.InsererUnDocument(Db, pReservation);
        }

        #endregion

        public List<Reservation> ObtenirTout()
        {
            return ObtenirObjetsDansLst(MongoDbContext.ObtenirCollectionListe<Reservation>(Db));
        }

        public List<Reservation> ObtenirPlusieurs<TField>(Expression<Func<Reservation, TField>> pField,
            List<TField> pObjects)
        {
            return ObtenirObjetsDansLst(MongoDbContext.ObtenirDocumentsFiltres(Db, pField, pObjects));
        }

        public List<Reservation> ObtenirObjetsDansLst(List<Reservation> pReservations)
        {
            List<ObjectId> filmIds = new List<ObjectId>();
            foreach (Reservation reservation in pReservations)
            {
                if (!filmIds.Contains(reservation.FilmId))
                {
                    filmIds.Add(reservation.FilmId);
                }
            }

            List<Film> films = _dalFilm.ObtenirPlusieurs(x => x.Id, filmIds);
            foreach (Reservation reservation in pReservations)
            {
                reservation.Film = films.Find(x => x.Id == reservation.FilmId);
            }

            return pReservations;
        }

        public bool InsererPlusieurs(List<Reservation> pDocuments)
        {
            throw new NotImplementedException();
        }

        public bool MAJUn<TField>(Expression<Func<Reservation, bool>> pFiltre,
            List<(Expression<Func<Reservation, TField>> field, TField value)> pMajDefinitions)
        {
            throw new NotImplementedException();
        }
    }
}