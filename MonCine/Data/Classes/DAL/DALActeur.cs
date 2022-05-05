#region MÉTADONNÉES

// Nom du fichier : DALActeur.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MonCine.Data.Classes.BD;
using MongoDB.Driver;

#endregion

namespace MonCine.Data.Classes.DAL
{
    /// <summary>
    /// Classe représentant une couche d'accès aux données pour les objets de type <see cref="Acteur"/>
    /// </summary>
    public class DALActeur : DAL
    {
        #region CONSTRUCTEURS

        /// <summary>
        /// Permet la création de la couche d'accès aux données pour les objets de type <see cref="Acteur"/>
        /// </summary>
        /// <param name="pClient">L'interface client vers MongoDB</param>
        /// <param name="pDb">Base de données MongoDB utilisée</param>
        public DALActeur(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
        }

        #endregion

        #region MÉTHODES

        /// <summary>
        /// Permet d'obtenir la liste des acteurs contenue dans la base de données de la cinémathèque.
        /// </summary>
        /// <returns>La liste des acteurs contenue dans la base de données de la cinémathèque.</returns>
        public List<Acteur> ObtenirActeurs()
        {
            return MongoDbContext.ObtenirCollectionListe<Acteur>(Db);
        }

        /// <summary>
        /// Permet de filtrer les acteurs contenus dans la base de données de la cinémathèque selon le champs et les valeurs spécifiés en paramètre.
        /// </summary>
        /// <typeparam name="TField">Type du champs sur lequel le filtrage sera effectué</typeparam>
        /// <param name="pField">Champs sur lequel le filtrage sera effectué</param>
        /// <param name="pObjects">Liste des valeurs à filtrer</param>
        /// <returns>La liste des acteurs filtrée selon le champs et les valeurs spécifiés en paramètre.</returns>
        public List<Acteur> ObtenirActeursFiltres<TField>(Expression<Func<Acteur, TField>> pField,
            List<TField> pObjects)
        {
            return MongoDbContext.ObtenirDocumentsFiltres(Db, pField, pObjects);
        }

        /// <summary>
        /// Permet d'insérer la liste des acteurs reçue en paramètre dans la baes de données de la cinémathèque.
        /// </summary>
        /// <param name="pActeurs">Liste des catégories dans insérer dans la base de données</param>
        public void InsererPlusieursActeurs(List<Acteur> pActeurs)
        {
            MongoDbContext.InsererPlusieursDocuments(Db, pActeurs);
        }

        #endregion
    }
}