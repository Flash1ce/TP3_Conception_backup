#region MÉTADONNÉES

// Nom du fichier : RealisateurTests.cs
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
    public class RealisateurTests
    {
        #region MÉTHODES

        private List<Realisateur> GenerationRealisateurs()
        {
            List<Realisateur> realisateurs = new List<Realisateur>()
            {
                new Realisateur(ObjectId.GenerateNewId(), "James Cameron"),
                new Realisateur(ObjectId.GenerateNewId(), "Steven Spielberg"),
                new Realisateur(ObjectId.GenerateNewId(), "Tim Burton"),
                new Realisateur(ObjectId.GenerateNewId(), "Gary Ross"),
                new Realisateur(ObjectId.GenerateNewId(), "Michael Bay")
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
            //List<Realisateur> realisateurs = GenerationRealisateurs();
            //var realisateurMock = new Mock<ICRUD<Realisateur>>();
            //realisateurMock.Setup(x => x.ObtenirTout()).Returns(realisateurs);
            //var realisateursMock = realisateurMock.Object.ObtenirTout();
            //Assert.Equal(realisateursMock, realisateurs);
        }

        [Fact]
        public void ObtenirPlusieursRetourneRealisateursSelonFiltre()
        {
            //List<Realisateur> realisateurs = GenerationRealisateurs();
            //var realisateurMock = new Mock<ICRUD<Realisateur>>();
            //realisateurMock.Setup(x => x.ObtenirPlusieurs(x => x.Nom, new List<string> { "James Cameron" })).Returns(new List<Realisateur> { realisateurs[0] });
            //var realisateursMock = realisateurMock.Object.ObtenirPlusieurs(x => x.Nom, new List<string> { "James Cameron" });
            //Assert.Equal(realisateursMock, new List<Realisateur> { realisateurs[0] });
        }

        [Fact]
        public void InsererPlusieursRealisateursRetourneTrue()
        {
            //List<Realisateur> realisateurs = GenerationRealisateurs();
            //var realisateurMock = new Mock<ICRUD<Realisateur>>();
            //realisateurMock.Setup(x => x.InsererPlusieurs(realisateurs)).Returns(true);
            //var realisateursMock = realisateurMock.Object.InsererPlusieurs(realisateurs);
            //Assert.True(realisateursMock);
        }

        #endregion
    }
}