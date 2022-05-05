#region MÉTADONNÉES

// Nom du fichier : DALSalle.cs
// Date de création : 2022-04-21
// Date de modification : 2022-04-21

#endregion

#region USING

using MonCine.Data.Classes.BD;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

#endregion

namespace MonCine.Data.Classes.DAL
{
    /// <summary>
    /// Classe représentant une couche d'accès aux données pour les objets de type <see cref="Salle"/>
    /// </summary>
    public class DALSalle : DAL
    {
        #region CONSTRUCTEURS

        /// <summary>
        /// Permet la création de la couche d'accès aux données pour les objets de type <see cref="Salle"/>
        /// </summary>
        /// <param name="pClient">L'interface client vers MongoDB</param>
        /// <param name="pDb">Base de données MongoDB utilisée</param>
        public DALSalle(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
        }

        #endregion

        #region MÉTHODES

        /// <summary>
        /// Permet d'obtenir la liste des salles contenue dans la base de données de la cinémathèque.
        /// </summary>
        /// <returns>La liste des salles contenue dans la base de données de la cinémathèque.</returns>
        public List<Salle> ObtenirTout()
        {
            return MongoDbContext.ObtenirCollectionListe<Salle>(Db);
        }

        /// <summary>
        /// Permet d'insérer la liste des salles reçue en paramètre dans la base de données de la cinémathèque.
        /// </summary>
        /// <param name="pSalles">Liste des salles à insérer dans la base de données</param>
        public void InsererPlusieurs(List<Salle> pSalles)
        {
            MongoDbContext.InsererPlusieursDocuments(Db, pSalles);
        }

        #endregion
    }
}