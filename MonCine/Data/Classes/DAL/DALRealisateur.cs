#region MÉTADONNÉES

// Nom du fichier : DALRealisateur.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MonCine.Data.Classes.BD;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace MonCine.Data.Classes.DAL
{
    public class DALRealisateur : DAL
    {
        #region CONSTRUCTEURS

        public DALRealisateur(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
        }

        #endregion

        #region MÉTHODES

        public List<Realisateur> ObtenirTout()
        {
            return MongoDbContext.ObtenirCollectionListe<Realisateur>(Db);
        }

        public List<Realisateur> ObtenirPlusieurs<TField>(Expression<Func<Realisateur, TField>> pField,
            List<TField> pObjects)
        {
            return MongoDbContext.ObtenirDocumentsFiltres(Db, pField, pObjects);
        }

        public void InsererPlusieurs(List<Realisateur> pRealisateurs)
        {
            MongoDbContext.InsererPlusieursDocuments(Db, pRealisateurs);
        }

        #endregion
    }
}