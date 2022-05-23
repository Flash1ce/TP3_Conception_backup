#region MÉTADONNÉES

// Nom du fichier : Salle.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Classes
{
    public class Salle
    {
        #region PROPRIÉTÉS ET INDEXEURS

        public ObjectId Id { get; set; }
        public string Nom { get; set; }
        public int NbPlacesMax { get; set; }

        #endregion

        #region CONSTRUCTEURS

        public Salle(ObjectId pId, string pNom, int pNbPlacesMax)
        {
            Id = pId;
            Nom = pNom;
            NbPlacesMax = pNbPlacesMax;
        }

        #endregion

        #region MÉTHODES

        public override string ToString()
        {
            return $"{Nom} - Nb. Places : {NbPlacesMax}";
        }

        #endregion
    }
}