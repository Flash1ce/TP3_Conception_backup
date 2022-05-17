#region MÉTADONNÉES

// Nom du fichier : IObtenir.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using MonCine.Data.Interfaces.Obtenir;

#endregion

namespace MonCine.Data.Interfaces
{
    public interface IObtenir<TDocument> : IObtenirUn<TDocument>, IObtenirPlusieurs<TDocument>, IObtenirTout<TDocument>
    {
    }
}