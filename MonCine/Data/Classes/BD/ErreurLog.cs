#region MÉTADONNÉES

// Nom du fichier : ErreurLog.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using System;
using System.Diagnostics;

#endregion

namespace MonCine.Data.Classes.BD
{
    public static class ErreurLog
    {
        #region CONSTANTES ET ATTRIBUTS STATIQUES

        private static readonly TraceSource traceur = new TraceSource("TraceSourceApp");

        #endregion

        #region MÉTHODES

        public static void Journaliser(string pMsg, TraceEventType pType, int pId)
        {
            ErreurLog.traceur.TraceEvent(pType, pId, $"{DateTime.Now} : {pMsg}");
            ErreurLog.traceur.Flush();
        }

        #endregion
    }
}