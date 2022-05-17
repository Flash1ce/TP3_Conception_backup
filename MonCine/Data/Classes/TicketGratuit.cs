#region MÉTADONNÉES

// Nom du fichier : TicketGratuit.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Classes
{
    public class TicketGratuit : Recompense
    {
        #region CONSTRUCTEURS

        public TicketGratuit(ObjectId pId, ObjectId pFilmId, ObjectId pAbonneId) : base(pId, pFilmId, pAbonneId)
        {
        }

        #endregion
    }
}