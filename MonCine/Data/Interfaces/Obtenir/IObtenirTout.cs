#region MÉTADONNÉES

// Nom du fichier : IObtenirTout.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System.Collections.Generic;

#endregion

namespace MonCine.Data.Interfaces.Obtenir
{
    public interface IObtenirTout<TDocument>
    {
        #region MÉTHODES

        public List<TDocument> ObtenirTout();

        #endregion
    }
}