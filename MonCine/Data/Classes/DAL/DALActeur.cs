#region MÉTADONNÉES

// Nom du fichier : DALActeur.cs
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
    public class DALActeur : DAL, IGerer<Acteur>
    {
        #region CONSTRUCTEURS

        public DALActeur(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
        }

        #endregion

        #region MÉTHODES

        public Acteur ObtenirUn(ObjectId pActeurId)
        {
            return ObtenirPlusieurs(x => x.Id == pActeurId)[0];
        }

        public List<Acteur> ObtenirPlusieurs(List<ObjectId> pActeursId)
        {
            return ObtenirPlusieurs(x => pActeursId.Contains(x.Id));
        }

        public List<Acteur> ObtenirPlusieurs(Func<Acteur, bool> pPredicate)
        {
            return MongoDbContext.ObtenirDocumentsFiltres(Db, pPredicate);
        }

        public List<Acteur> ObtenirTout()
        {
            return MongoDbContext.ObtenirCollectionListe<Acteur>(Db);
        }

        public bool InsererPlusieurs(List<Acteur> pActeurs)
        {
            return MongoDbContext.InsererPlusieursDocuments(Db, pActeurs);
        }

        #endregion
    }
}