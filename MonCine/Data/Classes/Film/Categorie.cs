#region MÉTADONNÉES

// Nom du fichier : Categorie.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Classes
{
    /// <summary>
    /// Classe représentant une catégorie d'un film.
    /// </summary>
    public class Categorie
    {
        #region PROPRIÉTÉS ET INDEXEURS

        /// <summary>
        /// Obtient ou défini l'identifiant de la catégorie
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Obtient ou défini le nom de la catégorie
        /// </summary>
        public string Nom { get; set; }

        #endregion

        #region CONSTRUCTEURS

        /// <summary>
        /// Constructeur permettant la création d'une catégorie.
        /// </summary>
        /// <param name="pId">Identifiant de la catégorie</param>
        /// <param name="pNom">Nom de la catégorie</param>
        public Categorie(ObjectId pId, string pNom)
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
            if (obj != null && obj is Categorie categorie)
            {
                return Id == categorie.Id;
            }
            return false;
        }

        #endregion
    }
}