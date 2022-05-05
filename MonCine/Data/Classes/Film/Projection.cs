#region MÉTADONNÉES

// Nom du fichier : Projection.cs
// Date de création : 2022-04-23
// Date de modification : 2022-04-23

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

        /// <summary>
        /// Date de fin de la projection
        /// </summary>
        private DateTime _dateFin;

        /// <summary>
        /// Nombre de places restantes de la projection
        /// </summary>
        private int _nbPlacesRestantes;

        private Salle _salle;

        #endregion

        #region PROPRIÉTÉS ET INDEXEURS

        /// <summary>
        /// Obtient ou défini la date de début de la projection
        /// </summary>
        public DateTime DateDebut { get; set; }

        /// <summary>
        /// Obtient ou défini la date de fin de la projection
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Lancée lorsque valeur est supérieure à la date de début de la projection.</exception>
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

        /// <summary>
        /// Obtient ou défini l'identifiant de la salle pour la projection du film
        /// </summary>
        public ObjectId SalleId { get; set; }

        /// <summary>
        /// Obtient ou défini la salle pour la projection du film
        /// </summary>
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

        /// <summary>
        /// Obtient ou défini le nombre de places restantes de la projection
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Lancée lorsque la valeur est négative.</exception>
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

        /// <summary>
        /// Obtient ou défini si la projection est active
        /// </summary>
        public bool EstActive { get; set; }

        #endregion

        #region CONSTRUCTEURS

        /// <summary>
        /// Constructeur permettant la création d'une projection étant active.
        /// </summary>
        /// <param name="pDateDebut">Date de début de la projection</param>
        /// <param name="pDateFin">Date de fin de la projection</param>
        /// <param name="pNbPlacesMax">Nombre de places maximum de la projection</param>
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