#region MÉTADONNÉES

// Nom du fichier : DALRecompense.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using MonCine.Data.Classes.BD;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;

#endregion

namespace MonCine.Data.Classes.DAL
{
    /// <summary>
    /// Classe représentant une couche d'accès aux données pour les objets de type <see cref="Recompense"/>
    /// </summary>
    public class DALRecompense : DAL
    {
        #region ATTRIBUTS

        /// <summary>
        /// Couche d'accès aux données pour les films
        /// </summary>
        private readonly DALFilm _dalFilm;

        #endregion

        #region CONSTRUCTEURS

        /// <summary>
        /// Permet la création de la couche d'accès aux données pour les objets de type <see cref="Recompense"/>
        /// </summary>
        /// <param name="pDalFilm">Couche d'accès aux données pour les films</param>
        /// <param name="pClient">L'interface client vers MongoDB</param>
        /// <param name="pDb">Base de données MongoDB utilisée</param>
        public DALRecompense(DALFilm pDalFilm, IMongoClient pClient = null, IMongoDatabase pDb = null) : base(pClient,
            pDb)
        {
            _dalFilm = pDalFilm;
        }

        #endregion

        #region MÉTHODES

        /// <summary>
        /// Permet d'obtenir la liste des récompenses contenue dans la base de données de la cinémathèque.
        /// </summary>
        /// <returns>La liste des récompenses contenue dans la base de données de la cinémathèque.</returns>
        public List<Recompense> ObtenirRecompenses()
        {
            // Création d'une liste de récompenses pour pouvoir ajouter d'autres récompenses
            // dont le type de récompense n'est pas des tickets gratuits
            List<Recompense> recompenses = new List<Recompense>();
            recompenses.AddRange(ObtenirObjetsDansRecompenses(
                new List<Recompense>(
                    MongoDbContext.ObtenirCollection<Recompense>(Db)
                        .Aggregate()
                        .OfType<TicketGratuit>()
                        .ToList())
            ));
            return recompenses;
        }

        /// <summary>
        /// Permet d'obtenir les objets pour les attributs d'un <see cref="Recompense"/> faisant référence à une autre
        /// collection dans la base de données de la cinémathèque pour toute la liste de abonnés spécifiée en paramètre.
        /// </summary>
        /// <param name="pRecompenses">Liste des récompense</param>
        /// <returns>
        /// La liste des récompenses dont les attributs faisant référence à une autre collection
        /// dans la base de données de la cinémathèque sont à présent définis par des objets non nul.
        /// </returns>
        private List<Recompense> ObtenirObjetsDansRecompenses(List<Recompense> pRecompenses)
        {
            List<ObjectId> filmIds = new List<ObjectId>();

            foreach (Recompense reservation in pRecompenses)
            {
                if (!filmIds.Contains(reservation.FilmId))
                {
                    filmIds.Add(reservation.FilmId);
                }
            }

            List<Film> films = _dalFilm.ObtenirPlusieurs(x => x.Id, filmIds);

            foreach (Recompense reservation in pRecompenses)
            {
                reservation.Film = films.Find(x => x.Id == reservation.FilmId);
            }

            return pRecompenses;
        }

        /// <summary>
        /// Permet d'insérer la liste des récompenses reçue en paramètre dans la base de données de la cinémathèque.
        /// </summary>
        /// <param name="pRecompenses">Liste des récompenses à insérer dans la base de données</param>
        public void InsererPlusieursRecompenses(List<Recompense> pRecompenses)
        {
            MongoDbContext.InsererPlusieursDocuments(Db, pRecompenses);
        }

        #endregion
    }
}