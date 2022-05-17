#region MÉTADONNÉES

// Nom du fichier : IObtenirPlusieurs.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System;
using System.Collections.Generic;
using MongoDB.Bson;

#endregion

namespace MonCine.Data.Interfaces.Obtenir
{
    public interface IObtenirPlusieurs<TDocument>
    {
        #region MÉTHODES

        public List<TDocument> ObtenirPlusieurs(List<ObjectId> pDocumentsId);
        public List<TDocument> ObtenirPlusieurs(Func<TDocument, bool> pPredicate);

        #endregion
    }
}