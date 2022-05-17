#region MÉTADONNÉES

// Nom du fichier : IInsererUn.cs
// Date de modification : 2022-05-17

#endregion

namespace MonCine.Data.Interfaces
{
    public interface IInsererUn<TDocument>
    {
        #region MÉTHODES

        public bool InsererUn(TDocument pDocument);

        #endregion
    }
}