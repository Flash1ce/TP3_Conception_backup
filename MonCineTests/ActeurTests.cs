using MonCine.Data.Classes;
using MonCine.Data.Interfaces;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MonCineTests
{
    public class ActeurTests
    {
        private List<Acteur> GenerationActeurs()
        {
            List<Acteur> acteurs = new List<Acteur>
            {
                    new Acteur(new ObjectId(), "Zendaya"),
                    new Acteur(new ObjectId(), "Keanu Reeves"),
                    new Acteur(new ObjectId(), "Ahmed Toumi"),
                    new Acteur(new ObjectId(), "Marvin Laeib"),
                    new Acteur(new ObjectId(), "Le Grand Gwenaël"),
                    new Acteur(new ObjectId(), "Antoine Le Merveilleux"),
                    new Acteur(new ObjectId(), "Timoté"),
                    new Acteur(new ObjectId(), "Ptite petate"),
                    new Acteur(new ObjectId(), "Mélina Chaud")
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
            List<Acteur> acteurs = GenerationActeurs();
            var acteurMock = new Mock<ICRUD<Acteur>>();
            acteurMock.Setup(x => x.ObtenirTout()).Returns(acteurs);
            var acteursMock = acteurMock.Object.ObtenirTout();
            Assert.Equal(acteursMock, acteurs);
        }

        [Fact]
        public void ObtenirPlusieursRetourneActeursSelonFiltre()
        {
            List<Acteur> acteurs = GenerationActeurs();
            var acteurMock = new Mock<ICRUD<Acteur>>();
            acteurMock.Setup(x => x.ObtenirPlusieurs(x => x.Nom, new List<string> { "Zendaya" })).Returns(new List<Acteur> { acteurs[0] });
            var acteursMock = acteurMock.Object.ObtenirPlusieurs(x => x.Nom, new List<string> { "Zendaya" });
            Assert.Equal(acteursMock, new List<Acteur> { acteurs[0] });
        }

        [Fact]
        public void InsererPlusieursActeursRetourneTrue()
        {
            List<Acteur> acteurs = GenerationActeurs();
            var acteurMock = new Mock<ICRUD<Acteur>>();
            acteurMock.Setup(x => x.InsererPlusieurs(acteurs)).Returns(true);
            var acteursMock = acteurMock.Object.InsererPlusieurs(acteurs);
            Assert.True(acteursMock);
        }
    }
}
