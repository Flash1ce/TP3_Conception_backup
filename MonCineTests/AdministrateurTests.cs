#region MÉTADONNÉES

// Nom du fichier : AdministrateurTests.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System.Collections.Generic;
using MonCine.Data.Classes;
using MongoDB.Bson;

#endregion

namespace MonCineTests
{
    internal class AdministrateurTests
    {
        #region MÉTHODES

        private List<Administrateur> GenerationAdministrateur()
        {
            List<Administrateur> administrateurs = new List<Administrateur>
            {
                new Administrateur(ObjectId.GenerateNewId(), "Zendaya", "Zendaya@hotmail.com", "Uganda123"),
                new Administrateur(ObjectId.GenerateNewId(), "Keanu Reeves", "Keanu@hotmail.com", "YESSERMILLER1"),
                new Administrateur(ObjectId.GenerateNewId(), "Ahmed Toumi", "Ahmed@hotmail.com", "Hallah"),
            };

            List<ObjectId> administrateursId = new List<ObjectId>();
            administrateurs
                .GetRange(0, 3)
                .ForEach(x => administrateursId.Add(x.Id));

            return administrateurs;
        }

        #endregion
    }
}