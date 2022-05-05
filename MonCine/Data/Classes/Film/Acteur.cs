#region MÉTADONNÉES

// Nom du fichier : Acteur.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Classes
{
    /// <summary>
    /// Classe représentant un acteur d'un film.
    /// </summary>
    public class Acteur
    {
        #region PROPRIÉTÉS ET INDEXEURS

        /// <summary>
        /// Obtient ou défini l'identifiant de l'acteur
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Obtient ou défini le nom de l'acteur
        /// </summary>
        public string Nom { get; set; }

        #endregion

        #region CONSTRUCTEURS

        /// <summary>
        /// Constructeur permettant la création d'un acteur.
        /// </summary>
        /// <param name="pId">Identifiant de l'acteur</param>
        /// <param name="pNom">Nom de l'acteur</param>
        public Acteur(ObjectId pId, string pNom)
        {
            Id = pId;
            Nom = pNom;
        }

        #endregion

        #region MÉTHODES

        public override string ToString()
        {
            return Nom;
        }

        public override bool Equals(object? obj)
        {
            if (obj is Acteur acteur)
            {
                return Id == acteur.Id;
            }
            return false;
        }

        #endregion
    }
}