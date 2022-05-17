#region MÉTADONNÉES

// Nom du fichier : DALAdministrateur.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MonCine.Data.Classes.BD;
using MonCine.Data.Interfaces;
using MonCine.Data.Interfaces.Obtenir;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;

#endregion

namespace MonCine.Data.Classes.DAL
{
    public interface IGererAdministrateur : IInsererUn<Administrateur>
    {
        public Administrateur ObtenirUn();
    }

    public class DALAdministrateur : DAL, IGererAdministrateur
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

        #endregion

        public bool InsererUn(Administrateur pAdministrateur)
        {
            return MongoDbContext.InsererUnDocument(Db, pAdministrateur);
        }
    }
}