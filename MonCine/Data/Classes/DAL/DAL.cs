#region MÉTADONNÉES

// Nom du fichier : DAL.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using System;
using MonCine.Data.Classes.BD;
using MongoDB.Driver;

#endregion

namespace MonCine.Data.Classes.DAL
{
    /// <summary>
    /// Classe représentant une couche d'accès aux données pour le type de document spécifié.
    /// </summary>
    public abstract class DAL
    {
        #region ATTRIBUTS

        /// <summary>
        /// L'interface client vers MongoDB
        /// </summary>
        protected internal IMongoClient MongoDbClient;

        /// <summary>
        /// Base de données MongoDB utilisée
        /// </summary>
        protected internal IMongoDatabase Db;

        #endregion

        #region CONSTRUCTEURS

        /// <summary>
        /// Constructeur permettant la création d'une couche d'accès aux données.
        /// </summary>
        /// <param name="pClient">L'interface client vers MongoDB</param>
        /// <param name="pDb">Base de données MongoDB utilisée</param>
        protected DAL(IMongoClient pClient = null, IMongoDatabase pDb = null)
        {
            MongoDbClient = pClient ?? OuvrirConnexion();
            Db = pDb ?? ObtenirBd();
        }

        #endregion

        #region MÉTHODES

        /// <summary>
        /// Permet d'ouvrir une connexion vers MongoDB.
        /// </summary>
        /// <returns>La connexion vers MongoDB.</returns>
        /// <exception cref="ExceptionBD">Lancée lorsqu'une erreur liée à la base de données se produit.</exception>
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

        /// <summary>
        /// Permet d'obtenir la base de données MongoDB utillisée.
        /// </summary>
        /// <returns>La base de données MongoDB utilisée</returns>
        /// <exception cref="ExceptionBD">Lancée lorsqu'une erreur liée à la base de données se produit.</exception>
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