#region MÉTADONNÉES

// Nom du fichier : Recompense.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

#endregion

namespace MonCine.Data.Classes
{
    public abstract class Recompense
    {
        #region PROPRIÉTÉS ET INDEXEURS

        public ObjectId Id { get; set; }

        public ObjectId FilmId { get; set; }

        [BsonIgnore] public Film Film { get; set; }

        public ObjectId AbonneId { get; set; }

        [BsonIgnore] public Abonne Abonne { get; set; }

        #endregion

        #region CONSTRUCTEURS

        protected Recompense(ObjectId pId, ObjectId pFilmId, ObjectId pAbonneId)
        {
            Id = pId;
            FilmId = pFilmId;
            AbonneId = pAbonneId;
        }

        #endregion

        #region MÉTHODES

        #region Overrides of Object

        public override string ToString()
        {
            return $"{GetType().Name} - {Film.Nom}";
        }

        #endregion

        #endregion
    }
}