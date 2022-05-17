#region MÉTADONNÉES

// Nom du fichier : ICRUD.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace MonCine.Data.Interfaces
{
    public interface ICRUD<TDocument>
    {
        #region MÉTHODES

        public List<TDocument> ObtenirTout();

        public List<TDocument> ObtenirPlusieurs<TField>(Expression<Func<TDocument, TField>> pField,
            List<TField> pObjects);

        public List<TDocument> ObtenirObjetsDansLst(List<TDocument> pDocuments);

        public bool InsererPlusieurs(List<TDocument> pDocuments);

        public bool MAJUn<TField>(Expression<Func<TDocument, bool>> pFiltre,
            List<(Expression<Func<TDocument, TField>> field, TField value)> pMajDefinitions);

        #endregion
    }
}