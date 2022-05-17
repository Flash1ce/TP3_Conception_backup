#region MÉTADONNÉES

// Nom du fichier : IMAJUn.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace MonCine.Data.Interfaces
{
    public interface IMAJUn<TDocument>
    {
        #region MÉTHODES

        public bool MAJUn<TField>(Expression<Func<TDocument, bool>> pFiltre,
            List<(Expression<Func<TDocument, TField>> field, TField value)> pMajDefinitions);

        #endregion
    }
}