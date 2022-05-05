using MonCine.Data.Classes.BD;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace MonCine.Data.Interfaces
{
    public interface ICRUD<TDocument>
    {
        public List<TDocument> ObtenirTout();

        public List<TDocument> ObtenirPlusieurs<TField>(Expression<Func<TDocument, TField>> pField,
            List<TField> pObjects);

        public List<TDocument> ObtenirObjetsDansLst(List<TDocument> pDocuments);

        public bool InsererPlusieurs(List<TDocument> pDocuments);

        public bool MAJUn<TField>(Expression<Func<TDocument, bool>> pFiltre,
            List<(Expression<Func<TDocument, TField>> field, TField value)> pMajDefinitions);
    }
}
