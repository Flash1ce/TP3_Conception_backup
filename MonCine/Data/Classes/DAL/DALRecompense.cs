#region MÉTADONNÉES

// Nom du fichier : DALRecompense.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MonCine.Data.Classes.BD;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

#endregion

namespace MonCine.Data.Classes.DAL
{
    public class DALRecompense : DAL
    {
        #region ATTRIBUTS

        private readonly DALFilm _dalFilm;

        #endregion

        #region CONSTRUCTEURS

        public DALRecompense(DALFilm pDalFilm, IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient,
            pDb)
        {
            _dalFilm = pDalFilm;
        }

        #endregion

        #region MÉTHODES

        public List<Recompense> ObtenirRecompenses()
        {
            List<Recompense> recompenses = new List<Recompense>();
            recompenses.AddRange(ObtenirObjetsDansRecompenses(
                new List<Recompense>(
                    MongoDbContext.ObtenirCollection<Recompense>(Db)
                        .Aggregate()
                        .OfType<TicketGratuit>()
                        .ToList())
            ));
            return recompenses;
        }

        private List<Recompense> ObtenirObjetsDansRecompenses(List<Recompense> pRecompenses)
        {
            List<ObjectId> filmIds = new List<ObjectId>();

            foreach (Recompense reservation in pRecompenses)
            {
                if (!filmIds.Contains(reservation.FilmId))
                {
                    filmIds.Add(reservation.FilmId);
                }
            }

            List<Film> films = _dalFilm.ObtenirPlusieurs(x => x.Id, filmIds);

            foreach (Recompense reservation in pRecompenses)
            {
                reservation.Film = films.Find(x => x.Id == reservation.FilmId);
            }

            return pRecompenses;
        }

        public void InsererPlusieursRecompenses(List<Recompense> pRecompenses)
        {
            MongoDbContext.InsererPlusieursDocuments(Db, pRecompenses);
        }

        #endregion
    }
}