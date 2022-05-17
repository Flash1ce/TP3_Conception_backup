#region MÉTADONNÉES

// Nom du fichier : Utilisateur.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Classes
{
    public abstract class Utilisateur
    {
        #region PROPRIÉTÉS ET INDEXEURS

        public ObjectId Id { get; set; }
        public string Nom { get; set; }
        public string Courriel { get; set; }
        public string Mdp { get; set; }

        #endregion

        #region CONSTRUCTEURS

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