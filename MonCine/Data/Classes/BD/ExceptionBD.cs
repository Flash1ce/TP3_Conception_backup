#region MÉTADONNÉES

// Nom du fichier : ExceptionBD.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using System;
using System.Diagnostics;

#endregion

namespace MonCine.Data.Classes.BD
{
    /// <summary>
    /// Classe représentant une exception personnalisée lorsqu'il y a une erreur en lien avec une base de donnée.
    /// </summary>
    public class ExceptionBD : Exception
    {
        #region CONSTRUCTEURS

        /// <summary>
        /// Constructeur d'une exception personnalisée pour une exception liée à la base de donnée.
        /// </summary>
        /// <param name="pErreurMsg">Message de l'erreur lancée</param>
        public ExceptionBD(string pErreurMsg) : base(
            $"La base de données est présentement inaccessible. Erreur : {pErreurMsg}"
        )
        {
            ErreurLog.Journaliser(pErreurMsg, TraceEventType.Warning, 1);
        }

        #endregion
    }
}