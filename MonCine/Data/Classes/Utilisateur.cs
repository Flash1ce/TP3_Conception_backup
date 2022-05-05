#region MÉTADONNÉES

// Nom du fichier : Utilisateur.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Classes
{
    /// <summary>
    /// Classe représentant un utilisateur de la cinémathèque
    /// </summary>
    public abstract class Utilisateur
    {
        #region PROPRIÉTÉS ET INDEXEURS

        /// <summary>
        /// Obtient ou défini l'identifiant de l'utilisateur
        /// </summary>
        public ObjectId Id { get; set; }

        /// <summary>
        /// Obtient ou défini le nom de l'utilisateur
        /// </summary>
        public string Nom { get; set; }

        /// <summary>
        /// Obtient ou défini le courriel de l'utilisateur
        /// </summary>
        public string Courriel { get; set; }

        /// <summary>
        /// Obtient ou défini le mot de passe de l'utilisateur
        /// </summary>
        public string Mdp { get; set; }

        #endregion

        #region CONSTRUCTEURS

        /// <summary>
        /// Constructeur permettant la création d'un utilisateur.
        /// </summary>
        /// <param name="pId">Identifiant de l'utilisateur</param>
        /// <param name="pNom">Nom de l'utilisateur</param>
        /// <param name="pCourriel">Courriel de l'utilisateur</param>
        /// <param name="pMdp">Mot de passe de l'utilisateur</param>
        protected Utilisateur(ObjectId pId, string pNom, string pCourriel, string pMdp)
        {
            Id = pId;
            Nom = pNom;
            Courriel = pCourriel;
            Mdp = pMdp;
        }

        #endregion
    }
}