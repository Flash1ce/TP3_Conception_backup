#region MÉTADONNÉES

// Nom du fichier : IInserer.cs
// Date de modification : 2022-05-17

#endregion

namespace MonCine.Data.Interfaces
{
    public interface IInserer<TDocument> : IInsererUn<TDocument>, IInsererPlusieur<TDocument>
    {
    }
}