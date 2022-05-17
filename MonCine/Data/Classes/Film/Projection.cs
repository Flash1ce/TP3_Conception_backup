#region MÉTADONNÉES

// Nom du fichier : Projection.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

#endregion

namespace MonCine.Data.Classes
{
    public class Projection
    {
        #region ATTRIBUTS

        private DateTime _dateFin;
        private int _nbPlacesRestantes;
        private Salle _salle;

        #endregion

        #region PROPRIÉTÉS ET INDEXEURS

        public DateTime DateDebut { get; set; }

        public DateTime DateFin
        {
            get { return _dateFin; }
            set
            {
                if (DateDebut >= value)
                {
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "La date de fin de la projection doit être supérieure à la date de début de la projection.");
                }

                _dateFin = value;
            }
        }


        public ObjectId SalleId { get; set; }

        [BsonIgnore]
        public Salle Salle
        {
            get { return _salle; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Le film de la réservation ne peut être nul.", nameof(value));
                }

                _salle = value;
                SalleId = _salle.Id;
            }
        }

        public int NbPlacesRestantes
        {
            get { return _nbPlacesRestantes; }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException(nameof(value),
                        "Il est impossible d'affectuer une valeur négative au nombre de places restantes de la projection.");
                }

                _nbPlacesRestantes = value;
            }
        }

        public bool EstActive { get; set; }

        #endregion

        #region CONSTRUCTEURS

        public Projection(DateTime pDateDebut, DateTime pDateFin, Salle pSalle)
        {
            DateDebut = pDateDebut;
            DateFin = pDateFin;
            Salle = pSalle;
            NbPlacesRestantes = pSalle.NbPlacesMax;
            EstActive = true;
        }

        #endregion

        #region MÉTHODES

        #region Overrides of Object

        public override string ToString()
        {
            return
                "Début de la projection: " + DateDebut.ToString("d MMMM yyy") +
                "\rFin de la projection: " + DateFin.ToString("d MMMM yyy") +
                "\rNb. place restantes: " + NbPlacesRestantes +
                $"\r{(EstActive ? "Projection active" : "Projection désactivée")}\r";
        }

        #endregion

        #endregion
    }
}