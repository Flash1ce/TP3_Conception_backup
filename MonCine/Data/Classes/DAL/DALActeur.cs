#region MÉTADONNÉES

// Nom du fichier : DALActeur.cs
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
    public class DALActeur : DAL
    {
        #region CONSTRUCTEURS

        public DALActeur(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
        }

        #endregion

        #region MÉTHODES

        public List<Acteur> ObtenirTout()
        {
            return MongoDbContext.ObtenirCollectionListe<Acteur>(Db);
        }

        public List<Acteur> ObtenirPlusieurs<TField>(Expression<Func<Acteur, TField>> pField,
            List<TField> pObjects)
        {
            return MongoDbContext.ObtenirDocumentsFiltres(Db, pField, pObjects);
        }

        public void InsererPlusieurs(List<Acteur> pActeurs)
        {
            MongoDbContext.InsererPlusieursDocuments(Db, pActeurs);
        }

        #endregion
    }
}