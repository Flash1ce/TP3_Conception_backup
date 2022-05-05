#region MÉTADONNÉES

// Nom du fichier : Administrateur.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Classes
{
    /// <summary>
    /// Classe représentant un administrateur de la cinémathèque.
    /// </summary>
    public class Administrateur : Utilisateur
    {
        #region CONSTRUCTEURS

        /// <summary>
        /// Constructeur permettant la création d'un administrateur.
        /// </summary>
        /// <param name="pId">Identifiant de l'administrateur</param>
        /// <param name="pNom">Nom de l'administrateur</param>
        /// <param name="pCourriel">Courriel de l'administrateur</param>
        /// <param name="pMdp">Mot de passe de l'administrateur</param>
        public Administrateur(ObjectId pId, string pNom, string pCourriel, string pMdp) : base(pId, pNom, pCourriel, pMdp)
        {
        }

        #endregion
    }
}