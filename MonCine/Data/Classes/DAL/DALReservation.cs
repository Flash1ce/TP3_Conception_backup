#region MÉTADONNÉES

// Nom du fichier : DALReservation.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System;
using System.Collections.Generic;
using MonCine.Data.Classes.BD;
using MonCine.Data.Interfaces;
using MonCine.Data.Interfaces.Obtenir;
using MongoDB.Bson;
using MongoDB.Driver;

#endregion

namespace MonCine.Data.Classes.DAL
{
    public interface IGererReservations : IObtenir<Reservation>,
        IObtenirDocumentsComplexes<Reservation>, IInsererUn<Reservation>
    {
        #region MÉTHODES

        public int ObtenirNbReservations(ObjectId pAbonneId);

        #endregion
    }

    public class DALReservation : DAL, IGererReservations
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

        public int ObtenirNbReservations(ObjectId pAbonneId)
        {
            return MongoDbContext.ObtenirDocumentsFiltres<Reservation>(Db, x => x.AbonneId == pAbonneId).Count;
        }

        public Reservation ObtenirUn(ObjectId pReservationId)
        {
            return ObtenirPlusieurs(x => x.Id == pReservationId)[0];
        }

        public List<Reservation> ObtenirPlusieurs(List<ObjectId> pReservationsId)
        {
            return ObtenirPlusieurs(x => pReservationsId.Contains(x.Id));
        }

        public List<Reservation> ObtenirPlusieurs(Func<Reservation, bool> pPredicate)
        {
            return ObtenirDocumentsComplexes(MongoDbContext.ObtenirDocumentsFiltres(Db, pPredicate));
        }

        public List<Reservation> ObtenirTout()
        {
            return ObtenirDocumentsComplexes(MongoDbContext.ObtenirCollectionListe<Reservation>(Db));
        }

        public List<Reservation> ObtenirDocumentsComplexes(List<Reservation> pReservations)
        {
            List<ObjectId> filmIds = new List<ObjectId>();
            foreach (Reservation reservation in pReservations)
            {
                if (!filmIds.Contains(reservation.FilmId))
                {
                    filmIds.Add(reservation.FilmId);
                }
            }

            List<Film> films = _dalFilm.ObtenirPlusieurs(filmIds);
            foreach (Reservation reservation in pReservations)
            {
                reservation.Film = films.Find(x => x.Id == reservation.FilmId);
            }

            return pReservations;
        }

        public bool InsererUn(Reservation pReservation)
        {
            _dalFilm.MAJProjections(pReservation.Film);
            return MongoDbContext.InsererUnDocument(Db, pReservation);
        }
    }
}