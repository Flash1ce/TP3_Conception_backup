#region MÉTADONNÉES

// Nom du fichier : IObtenirUn.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using MongoDB.Bson;

#endregion

namespace MonCine.Data.Interfaces.Obtenir
{
    public interface IObtenirUn<TDocument>
    {
        #region MÉTHODES

        public TDocument ObtenirUn(ObjectId pDocumentId);

        #endregion
    }
}