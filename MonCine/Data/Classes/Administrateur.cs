#region MÉTADONNÉES

// Nom du fichier : Administrateur.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Classes
{
    public class Administrateur : Utilisateur
    {
        #region CONSTRUCTEURS

        public Administrateur(ObjectId pId, string pNom, string pCourriel, string pMdp) : base(pId, pNom, pCourriel,
            pMdp)
        {
        }

        #endregion

        #region MÉTHODES

        public override string ToString()
        {
            return Nom;
        }

        #endregion
    }
}