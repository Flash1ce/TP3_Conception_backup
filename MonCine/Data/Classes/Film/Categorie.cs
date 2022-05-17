#region MÉTADONNÉES

// Nom du fichier : Categorie.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Classes
{
    public class Categorie
    {
        #region PROPRIÉTÉS ET INDEXEURS

        public ObjectId Id { get; set; }
        public string Nom { get; set; }

        #endregion

        #region CONSTRUCTEURS

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