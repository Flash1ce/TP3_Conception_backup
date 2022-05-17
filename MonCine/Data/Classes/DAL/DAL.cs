#region MÉTADONNÉES

// Nom du fichier : DAL.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MonCine.Data.Classes.BD;
using MongoDB.Driver;
using System;

#endregion

namespace MonCine.Data.Classes.DAL
{
    public abstract class DAL
    {
        #region ATTRIBUTS

        protected internal IMongoClient MongoDbClient;

        protected internal IMongoDatabase Db;

        #endregion

        #region CONSTRUCTEURS

        protected DAL(IMongoClient pClient = null, IMongoDatabase pDb = null)
        {
            MongoDbClient = pClient ?? OuvrirConnexion();
            Db = pDb ?? ObtenirBd();
        }

        #endregion

        #region MÉTHODES

        private IMongoClient OuvrirConnexion()
        {
            try
            {
                return new MongoClient("mongodb://localhost:27017/");
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : OuvrirConnexion - Exception : {e.Message}");
            }
        }

        private IMongoDatabase ObtenirBd()
        {
            try
            {
                return MongoDbClient.GetDatabase("TP2DB");
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : ObtenirBD - Exception : {e.Message}");
            }
        }

        #endregion
    }
}