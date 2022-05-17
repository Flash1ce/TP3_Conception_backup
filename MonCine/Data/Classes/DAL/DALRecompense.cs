#region MÉTADONNÉES

// Nom du fichier : DALRecompense.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System.Collections.Generic;
using MonCine.Data.Classes.BD;
using MonCine.Data.Interfaces;
using MonCine.Data.Interfaces.Obtenir;
using MongoDB.Bson;
using MongoDB.Driver;

#endregion

namespace MonCine.Data.Classes.DAL;

public class DALRecompense : DAL, IObtenirTout<Recompense>, IObtenirDocumentsComplexes<Recompense>,
    IInsererPlusieur<Recompense>
{
    #region ATTRIBUTS

    /// <summary>
    /// Couche d'accès aux données pour les films
    /// </summary>
    private readonly DALFilm _dalFilm;

    #endregion

    #region CONSTRUCTEURS

    public DALRecompense(DALFilm pDalFilm, IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient,
        pDb)
    {
        _dalFilm = pDalFilm;
    }

    #endregion

    public bool InsererPlusieurs(List<Recompense> pRecompenses)
    {
        return MongoDbContext.InsererPlusieursDocuments(Db, pRecompenses);
    }

    public List<Recompense> ObtenirTout()
    {
        List<Recompense> recompenses = new List<Recompense>();
        recompenses.AddRange(ObtenirDocumentsComplexes(
            new List<Recompense>(
                MongoDbContext.ObtenirCollection<Recompense>(Db)
                    .Aggregate()
                    .OfType<TicketGratuit>()
                    .ToList())
        ));
        return recompenses;
    }

    public List<Recompense> ObtenirDocumentsComplexes(List<Recompense> pRecompenses)
    {
        List<ObjectId> filmIds = new List<ObjectId>();

        foreach (Recompense reservation in pRecompenses)
        {
            if (!filmIds.Contains(reservation.FilmId))
            {
                filmIds.Add(reservation.FilmId);
            }
        }

        List<Film> films = _dalFilm.ObtenirPlusieurs(filmIds);

        foreach (Recompense reservation in pRecompenses)
        {
            reservation.Film = films.Find(x => x.Id == reservation.FilmId);
        }

        return pRecompenses;
    }
}
