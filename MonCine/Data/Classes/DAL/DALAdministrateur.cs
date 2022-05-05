#region MÉTADONNÉES

// Nom du fichier : DALAdministrateur.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using System;
using System.Collections.Generic;
using MonCine.Data.Classes.BD;
using MongoDB.Driver;

#endregion

namespace MonCine.Data.Classes.DAL
{
    /// <summary>
    /// Classe représentant une couche d'accès aux données pour les objets de type <see cref="Administrateur"/>.
    /// </summary>
    public class DALAdministrateur : DAL
    {
        #region CONSTRUCTEURS

        /// <summary>
        /// Constructeur permettant la création de la couche d'accès au données pour les objets de type <see cref="Administrateur"/>.
        /// </summary>
        /// <param name="pClient">L'interface client vers MongoDB</param>
        /// <param name="pDb">Base de données MongoDB utilisée</param>
        public DALAdministrateur(IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient, pDb)
        {
        }

        #endregion

        #region MÉTHODES

        /// <summary>
        /// Permet d'obtenir l'administrateur à partir de la base de données de la cinémathèque depuis la collection concernée.
        /// </summary>
        /// <returns>L'administrateur provenant de la base de données.</returns>
        /// <exception cref="IndexOutOfRangeException">Lancée lorsqu'il y a plus de 1 administrateur dans la base de données.</exception>
        public Administrateur ObtenirUn()
        {
            List<Administrateur> administrateurs = MongoDbContext.ObtenirCollectionListe<Administrateur>(Db);
            if (administrateurs.Count > 1)
            {
                throw new IndexOutOfRangeException(
                    "La base de données contient plus d'un administrateur pour la cinémathèque."
                );
            }
            return administrateurs.Count == 1 ? administrateurs[0] : null;
        }

        /// <summary>
        /// Permet d'insérer l'administrateur reçu en paramètre dans la base de données de la cinémathèque.
        /// </summary>
        /// <param name="pAdministrateur">Administrateur à insérer dans la base de données</param>
        public void InsererUn(Administrateur pAdministrateur)
        {
            MongoDbContext.InsererUnDocument(Db, pAdministrateur);
        }

        #endregion
    }
}