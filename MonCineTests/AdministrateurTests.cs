using MonCine.Data.Classes;
using MonCine.Data.Interfaces;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MonCineTests
{
    internal class AdministrateurTests
    {
        private List<Administrateur> GenerationAdministrateur()
        {
            List<Administrateur> administrateurs = new List<Administrateur>
            {
                    new Administrateur(new ObjectId(), "Zendaya","Zendaya@hotmail.com","Uganda123"),
                    new Administrateur(new ObjectId(), "Keanu Reeves","Keanu@hotmail.com", "YESSERMILLER1"),
                    new Administrateur(new ObjectId(), "Ahmed Toumi", "Ahmed@hotmail.com", "Hallah"),
            };

            List<ObjectId> administrateursId = new List<ObjectId>();
            administrateurs
                .GetRange(0, 3)
                .ForEach(x => administrateursId.Add(x.Id));

            return administrateurs;
        }

    }
}
