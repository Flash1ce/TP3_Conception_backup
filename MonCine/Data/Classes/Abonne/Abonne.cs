#region MÉTADONNÉES

// Nom du fichier : Abonne.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

#endregion

namespace MonCine.Data.Classes
{
    public class Abonne : Utilisateur
    {
        #region ATTRIBUTS

        private Preference _preference;

        #endregion

        #region PROPRIÉTÉS ET INDEXEURS

        public DateTime DateAdhesion { get; set; }

        public Preference Preference
        {
            get { return _preference; }
            set
            {
                if (value == null)
                {
                    throw new NullReferenceException(
                        "Impossible d'attribuer objet nul à préférence."
                    );
                }

                _preference = value;
            }
        }

        [BsonIgnore] public int NbSeances { get; set; }

        #endregion

        #region CONSTRUCTEURS

        public Abonne(ObjectId pId, string pNom, string pCourriel, string pMdp, Preference pPreference) :
            base(pId, pNom, pCourriel, pMdp)
        {
            DateAdhesion = DateTime.Now;
            Preference = pPreference;
        }

        #endregion

        #region MÉTHODES

        #region Overrides of Object

        public override string ToString()
        {
            return Preference.ToString();
        }

        #endregion

        #endregion
    }
}