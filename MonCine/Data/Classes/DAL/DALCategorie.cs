#region MÉTADONNÉES

// Nom du fichier : DALCategorie.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MonCine.Data.Classes.BD;
using MonCine.Data.Interfaces;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace MonCine.Data.Classes.DAL
{
    public class DALCategorie : DAL, ICRUD<Categorie>
    {
        #region CONSTRUCTEURS

        public DALCategorie(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
        }

        #endregion

        public List<Categorie> ObtenirTout()
        {
            return MongoDbContext.ObtenirCollectionListe<Categorie>(Db);
        }

        public List<Categorie> ObtenirPlusieurs<TField>(Expression<Func<Categorie, TField>> pField,
            List<TField> pObjects)
        {
            return MongoDbContext.ObtenirDocumentsFiltres(Db, pField, pObjects);
        }

        public List<Categorie> ObtenirObjetsDansLst(List<Categorie> pCategories)
        {
            throw new NotImplementedException();
        }

        public bool InsererPlusieurs(List<Categorie> pCategories)
        {
            return MongoDbContext.InsererPlusieursDocuments(Db, pCategories);
        }

        public bool MAJUn<TField>(Expression<Func<Categorie, bool>> pFiltre,
            List<(Expression<Func<Categorie, TField>> field, TField value)> pMajDefinitions)
        {
            throw new NotImplementedException();
        }
    }
}