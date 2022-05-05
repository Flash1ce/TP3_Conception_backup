using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace MonCine.Data.Classes
{
    public class Salle
    {
        public ObjectId Id { get; set; }
        public string Nom { get; set; }
        public int NbPlacesMax { get; set; }

        public Salle(ObjectId pId, string pNom, int pNbPlacesMax)
        {
            Id = pId;
            Nom = pNom;
            NbPlacesMax = pNbPlacesMax;
        }

        public override string ToString()
        {
            return $"{Nom} - Nb. Places : {NbPlacesMax}";
        }
    }
}
