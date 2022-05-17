#region MÉTADONNÉES

// Nom du fichier : ActeurTests.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System.Collections.Generic;
using MonCine.Data.Classes;
using MongoDB.Bson;
using Xunit;

#endregion

namespace MonCineTests
{
    public class ActeurTests
    {
        #region MÉTHODES

        private List<Acteur> GenerationActeurs()
        {
            List<Acteur> acteurs = new List<Acteur>
            {
                new Acteur(ObjectId.GenerateNewId(), "Zendaya"),
                new Acteur(ObjectId.GenerateNewId(), "Keanu Reeves"),
                new Acteur(ObjectId.GenerateNewId(), "Ahmed Toumi"),
                new Acteur(ObjectId.GenerateNewId(), "Marvin Laeib"),
                new Acteur(ObjectId.GenerateNewId(), "Le Grand Gwenaël"),
                new Acteur(ObjectId.GenerateNewId(), "Antoine Le Merveilleux"),
                new Acteur(ObjectId.GenerateNewId(), "Timoté"),
                new Acteur(ObjectId.GenerateNewId(), "Ptite petate"),
                new Acteur(ObjectId.GenerateNewId(), "Mélina Chaud")
            };

            List<ObjectId> acteursId = new List<ObjectId>();
            acteurs
                .GetRange(0, 5)
                .ForEach(x => acteursId.Add(x.Id));

            return acteurs;
        }

        [Fact]
        public void ObtenirTousRetourneLesActeurs()
        {
            //List<Acteur> acteurs = GenerationActeurs();
            //var acteurMock = new Mock<ICRUD<Acteur>>();
            //acteurMock.Setup(x => x.ObtenirTout()).Returns(acteurs);
            //var acteursMock = acteurMock.Object.ObtenirTout();
            //Assert.Equal(acteursMock, acteurs);
        }

        [Fact]
        public void ObtenirPlusieursRetourneActeursSelonFiltre()
        {
            //List<Acteur> acteurs = GenerationActeurs();
            //var acteurMock = new Mock<ICRUD<Acteur>>();
            //acteurMock.Setup(x => x.ObtenirPlusieurs(x => x.Nom, new List<string> { "Zendaya" })).Returns(new List<Acteur> { acteurs[0] });
            //var acteursMock = acteurMock.Object.ObtenirPlusieurs(x => x.Nom, new List<string> { "Zendaya" });
            //Assert.Equal(acteursMock, new List<Acteur> { acteurs[0] });
        }

        [Fact]
        public void InsererPlusieursActeursRetourneTrue()
        {
            //List<Acteur> acteurs = GenerationActeurs();
            //var acteurMock = new Mock<ICRUD<Acteur>>();
            //acteurMock.Setup(x => x.InsererPlusieurs(acteurs)).Returns(true);
            //var acteursMock = acteurMock.Object.InsererPlusieurs(acteurs);
            //Assert.True(acteursMock);
        }

        #endregion
    }
}