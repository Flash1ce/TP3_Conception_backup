#region MÉTADONNÉES

// Nom du fichier : IObtenirDocumentsComplexes.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System.Collections.Generic;

#endregion

namespace MonCine.Data.Interfaces
{
    public interface IObtenirDocumentsComplexes<TDocument>
    {
        #region MÉTHODES

        public List<TDocument> ObtenirDocumentsComplexes(List<TDocument> pDocuments);

        #endregion
    }
}