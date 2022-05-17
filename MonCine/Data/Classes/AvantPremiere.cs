#region MÉTADONNÉES

// Nom du fichier : AvantPremiere.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Classes
{
    public class AvantPremiere : Recompense
    {
        #region CONSTRUCTEURS

        public AvantPremiere(ObjectId pId, ObjectId pFilmId, ObjectId pAbonneId) : base(pId, pFilmId, pAbonneId)
        {
        }

        #endregion
    }
}