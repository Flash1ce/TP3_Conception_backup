#region MÉTADONNÉES

// Nom du fichier : Realisateur.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Classes
{
    public class Realisateur
    {
        #region PROPRIÉTÉS ET INDEXEURS

        public ObjectId Id { get; set; }
        public string Nom { get; set; }

        #endregion

        #region CONSTRUCTEURS

        public Realisateur(ObjectId pId, string pNom)
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
            if (obj is Realisateur realisateur)
            {
                return Id == realisateur.Id;
            }

            return false;
        }

        #endregion
    }
}