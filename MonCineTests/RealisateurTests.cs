using MonCine.Data.Classes;
using MonCine.Data.Interfaces;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MonCineTests
{
    public class RealisateurTests
    {
        private List<Realisateur> GenerationRealisateurs()
        {
            List<Realisateur> realisateurs = new List<Realisateur>()
                {
                    new Realisateur(new ObjectId(), "James Cameron"),
                    new Realisateur(new ObjectId(), "Steven Spielberg"),
                    new Realisateur(new ObjectId(), "Tim Burton"),
                    new Realisateur(new ObjectId(), "Gary Ross"),
                    new Realisateur(new ObjectId(), "Michael Bay")
                };
            List<ObjectId> realisateursId = new List<ObjectId>();
            realisateurs
                .GetRange(0, 5)
                .ForEach(x => realisateursId.Add(x.Id));

            return realisateurs;
        }
        [Fact]
        public void ObtenirTousRetourneLesRealisateurs()
        {
            List<Realisateur> realisateurs = GenerationRealisateurs();
            var realisateurMock = new Mock<ICRUD<Realisateur>>();
            realisateurMock.Setup(x => x.ObtenirTout()).Returns(realisateurs);
            var realisateursMock = realisateurMock.Object.ObtenirTout();
            Assert.Equal(realisateursMock, realisateurs);
        }

        [Fact]
        public void ObtenirPlusieursRetourneRealisateursSelonFiltre()
        {
            List<Realisateur> realisateurs = GenerationRealisateurs();
            var realisateurMock = new Mock<ICRUD<Realisateur>>();
            realisateurMock.Setup(x => x.ObtenirPlusieurs(x => x.Nom, new List<string> { "James Cameron" })).Returns(new List<Realisateur> { realisateurs[0] });
            var realisateursMock = realisateurMock.Object.ObtenirPlusieurs(x => x.Nom, new List<string> { "James Cameron" });
            Assert.Equal(realisateursMock, new List<Realisateur> { realisateurs[0] });
        }

        [Fact]
        public void InsererPlusieursRealisateursRetourneTrue()
        {
            List<Realisateur> realisateurs = GenerationRealisateurs();
            var realisateurMock = new Mock<ICRUD<Realisateur>>();
            realisateurMock.Setup(x => x.InsererPlusieurs(realisateurs)).Returns(true);
            var realisateursMock = realisateurMock.Object.InsererPlusieurs(realisateurs);
            Assert.True(realisateursMock);
        }
    }
}
