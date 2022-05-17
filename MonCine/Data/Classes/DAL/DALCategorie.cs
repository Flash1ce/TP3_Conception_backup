#region MÉTADONNÉES

// Nom du fichier : DALCategorie.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System;
using System.Collections.Generic;
using MonCine.Data.Classes.BD;
using MonCine.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

#endregion

namespace MonCine.Data.Classes.DAL
{
    public class DALCategorie : DAL, IGerer<Categorie>
    {
        #region CONSTRUCTEURS

        public DALCategorie(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
        }

        #endregion

        public Categorie ObtenirUn(ObjectId pCategorieId)
        {
            return ObtenirPlusieurs(x => x.Id == pCategorieId)[0];
        }

        public List<Categorie> ObtenirPlusieurs(List<ObjectId> pCategoriesId)
        {
            return ObtenirPlusieurs(x => pCategoriesId.Contains(x.Id));
        }

        public List<Categorie> ObtenirPlusieurs(Func<Categorie, bool> pPredicate)
        {
            return MongoDbContext.ObtenirDocumentsFiltres(Db, pPredicate);
        }

        public List<Categorie> ObtenirTout()
        {
            return MongoDbContext.ObtenirCollectionListe<Categorie>(Db);
        }

        public bool InsererPlusieurs(List<Categorie> pCategories)
        {
            return MongoDbContext.InsererPlusieursDocuments(Db, pCategories);
        }
    }
}