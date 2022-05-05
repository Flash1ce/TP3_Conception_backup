#region MÉTADONNÉES

// Nom du fichier : Reservation.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using System;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

#endregion

namespace MonCine.Data.Classes
{
    public class Reservation
    {
        #region ATTRIBUTS

        private Film _film;
        private int _nbPlaces;

        #endregion

        #region PROPRIÉTÉS ET INDEXEURS

        public ObjectId Id { get; set; }

        public ObjectId FilmId { get; set; }

        [BsonIgnore]
        public Film Film
        {
            get { return _film; }
            set
            {
                if (value is null)
                {
                    throw new ArgumentNullException("Le film de la réservation ne peut être nul.", nameof(value));
                }

                _film = value;
                FilmId = _film.Id;
            }
        }

        public int IndexProjectionFilm { get; set; }

        public ObjectId AbonneId { get; set; }

        public int NbPlaces
        {
            get { return _nbPlaces; }
            set
            {
                if (value < 1)
                {
                    throw new ArgumentOutOfRangeException(
                        nameof(value),
                        "Le nombre de places de la réservation doit être d'au moins 1."
                    );
                }

                _nbPlaces = value;

                if (Film != null)
                {
                    Projection projection = Film.Projections[IndexProjectionFilm];
                    if (projection.NbPlacesRestantes - _nbPlaces < 0)
                    {
                        throw new InvalidOperationException(
                            "Il ne reste plus de places pour créer une réservation avec le nombre de places et la projection sélectionnés.");
                    }
                    else if (projection.NbPlacesRestantes - _nbPlaces == 0) // Lorsqu'il n'y a plus de place disponible pour une réservation
                        projection.EstActive = false;

                    projection.NbPlacesRestantes -= _nbPlaces;
                }
            }
        }

        #endregion

        #region CONSTRUCTEURS

        public Reservation(ObjectId pId, Film pFilm, int pIndexProjectionFilm, ObjectId pAbonneId, int pNbPlaces)
        {
            Id = pId;
            Film = pFilm;
            IndexProjectionFilm = pIndexProjectionFilm;
            AbonneId = pAbonneId;
            NbPlaces = pNbPlaces;
        }

        #endregion
    }
}