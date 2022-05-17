#region MÉTADONNÉES

// Nom du fichier : IGerer.cs
// Date de modification : 2022-05-17

#endregion

namespace MonCine.Data.Interfaces
{
    public interface IGerer<TDocument> : IObtenir<TDocument>, IInsererPlusieur<TDocument>
    {
    }
}