#region MÉTADONNÉES

// Nom du fichier : DALRealisateur.cs
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
    public class DALRealisateur : DAL, IGerer<Realisateur>
    {
        #region CONSTRUCTEURS

        public DALRealisateur(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
        }

        #endregion

        #region MÉTHODES

        public Realisateur ObtenirUn(ObjectId pRealisateurId)
        {
            return ObtenirPlusieurs(x => x.Id == pRealisateurId)[0];
        }

        public List<Realisateur> ObtenirPlusieurs(List<ObjectId> pRealisateursId)
        {
            return ObtenirPlusieurs(x => pRealisateursId.Contains(x.Id));
        }

        public List<Realisateur> ObtenirPlusieurs(Func<Realisateur, bool> pPredicate)
        {
            return MongoDbContext.ObtenirDocumentsFiltres(Db, pPredicate);
        }

        public List<Realisateur> ObtenirTout()
        {
            return MongoDbContext.ObtenirCollectionListe<Realisateur>(Db);
        }

        public bool InsererPlusieurs(List<Realisateur> pRealisateurs)
        {
            return MongoDbContext.InsererPlusieursDocuments(Db, pRealisateurs);
        }
      
        #endregion
    }
}