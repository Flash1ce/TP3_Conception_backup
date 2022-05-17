#region MÉTADONNÉES

// Nom du fichier : IInsererPlusieur.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System.Collections.Generic;

#endregion

namespace MonCine.Data.Interfaces
{
    public interface IInsererPlusieur<TDocument>
    {
        #region MÉTHODES

        public bool InsererPlusieurs(List<TDocument> pDocuments);

        #endregion
    }
}