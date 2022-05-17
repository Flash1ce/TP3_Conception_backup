#region MÉTADONNÉES

// Nom du fichier : DALSalle.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MonCine.Data.Classes.BD;
using MongoDB.Driver;
using System.Collections.Generic;

#endregion

namespace MonCine.Data.Classes.DAL
{
    public class DALSalle : DAL
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

        public void InsererPlusieurs(List<Salle> pSalles)
        {
            MongoDbContext.InsererPlusieursDocuments(Db, pSalles);
        }

        #endregion
    }
}