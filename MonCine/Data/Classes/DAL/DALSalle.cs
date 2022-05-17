#region MÉTADONNÉES

// Nom du fichier : DALSalle.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MonCine.Data.Classes.BD;
using MonCine.Data.Interfaces;
using MonCine.Data.Interfaces.Obtenir;
using MongoDB.Driver;
using System.Collections.Generic;

#endregion

namespace MonCine.Data.Classes.DAL
{
    public class DALSalle : DAL, IObtenirTout<Salle>, IInsererPlusieur<Salle>
    {
        #region CONSTRUCTEURS

        public DALSalle(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
        }

        #endregion

        #region MÉTHODES

        public List<Salle> ObtenirTout()
        {
            return MongoDbContext.ObtenirCollectionListe<Salle>(Db);
        }

        public bool InsererPlusieurs(List<Salle> pSalles)
        {
            return MongoDbContext.InsererPlusieursDocuments(Db, pSalles);
        }

        #endregion
    }
}