#region MÉTADONNÉES

// Nom du fichier : ExceptionBD.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using System;
using System.Diagnostics;

#endregion

namespace MonCine.Data.Classes.BD
{
    public class ExceptionBD : Exception
    {
        #region CONSTRUCTEURS

        public ExceptionBD(string pErreurMsg) : base(
            $"La base de données est présentement inaccessible. Erreur : {pErreurMsg}"
        )
        {
            ErreurLog.Journaliser(pErreurMsg, TraceEventType.Warning, 1);
        }

        #endregion
    }
}