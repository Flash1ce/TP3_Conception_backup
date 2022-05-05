using MonCine.Data.Classes;
using MonCine.Data.Interfaces;
using MongoDB.Bson;
using Moq;
using System.Collections.Generic;
using Xunit;

namespace MonCineTests
{
    public class AbonneTests
    {
        private List<Abonne> GenerationAbonnes()
        {
            List<Abonne> abonnes = new List<Abonne>();

            List<Categorie> categories = new List<Categorie>
                {
                    new(new ObjectId(), "Horreur"),
                    new(new ObjectId(), "Fantastique"),
                    new(new ObjectId(), "Comédie"),
                    new(new ObjectId(), "Action"),
                    new(new ObjectId(), "Romance")
                };

            List<Acteur> acteurs = new List<Acteur>
                {
                    new(new ObjectId(), "Zendaya"),
                    new(new ObjectId(), "Keanu Reeves"),
                    new(new ObjectId(), "Ahmed Toumi"),
                    new(new ObjectId(), "Marvin Laeib"),
                    new(new ObjectId(), "Le Grand Gwenaël"),
                    new(new ObjectId(), "Antoine Le Merveilleux"),
                    new(new ObjectId(), "Timoté"),
                    new(new ObjectId(), "Ptite petate"),
                    new(new ObjectId(), "Mélina Chaud")
                };

            List<Realisateur> realisateurs = new List<Realisateur>()
                {
                    new(new ObjectId(), "James Cameron"),
                    new(new ObjectId(), "Steven Spielberg"),
                    new(new ObjectId(), "Tim Burton"),
                    new(new ObjectId(), "Gary Ross"),
                    new(new ObjectId(), "Michael Bay")
                };

            List<ObjectId> categoriesId = new List<ObjectId>();
            categories
                .GetRange(0, 3)
                .ForEach(x => categoriesId.Add(x.Id));

            List<ObjectId> acteursId = new List<ObjectId>();
            acteurs
                .GetRange(0, 5)
                .ForEach(x => acteursId.Add(x.Id));

            List<ObjectId> realisateursId = new List<ObjectId>();
            realisateurs
                .GetRange(0, 5)
                .ForEach(x => realisateursId.Add(x.Id));

            abonnes.Add(new Abonne
            (
                new ObjectId(),
                $"Utilisateur {1}",
                $"utilisateur{1}@email.com",
                $"user{1}",
                new Preference(
                    categoriesId,
                    acteursId,
                    realisateursId
                )
            ));
            abonnes.Add(new Abonne
              (
                  new ObjectId(),
                  $"Utilisateur {2}",
                  $"utilisateur{2}@email.com",
                  $"user{2}",
                  new Preference(
                      categoriesId,
                      acteursId,
                      realisateursId
                  )
              )); abonnes.Add(new Abonne
               (
                   new ObjectId(),
                   $"Utilisateur {3}",
                   $"utilisateur{3}@email.com",
                   $"user{3}",
                   new Preference(
                       categoriesId,
                       acteursId,
                       realisateursId
                   )
               )); abonnes.Add(new Abonne
               (
                   new ObjectId(),
                   $"Utilisateur {4}",
                   $"utilisateur{4}@email.com",
                   $"user{4}",
                   new Preference(
                       categoriesId,
                       acteursId,
                       realisateursId
                   )
               ));
            abonnes.Add(new Abonne
              (
                  new ObjectId(),
                  $"Utilisateur {5}",
                  $"utilisateur{5}@email.com",
                  $"user{5}",
                  new Preference(
                      categoriesId,
                      acteursId,
                      realisateursId
                  )
              ));
            abonnes.Add(new Abonne
              (
                  new ObjectId(),
                  $"Utilisateur {6}",
                  $"utilisateur{6}@email.com",
                  $"user{6}",
                  new Preference(
                      categoriesId,
                      acteursId,
                      realisateursId
                  )
              ));
            return abonnes;
        }
        [Fact]
        public void ObtenirToutSiTousLesAbonnesSontRetournes()
        {
            List<Abonne> abonnes = GenerationAbonnes();
            var abonneMock = new Mock<ICRUD<Abonne>>();

            abonneMock.Setup(x => x.ObtenirTout()).Returns(abonnes);
            var abonnesMock = abonneMock.Object.ObtenirTout();
            Assert.Equal(abonnesMock, abonnes);
        }
        [Fact]
        public void ObtenirPlusieursRetourneAbonnesSelonFiltre()
        {
            List<Abonne> abonnes = GenerationAbonnes();
            var abonneMock = new Mock<ICRUD<Abonne>>();
            abonneMock.Setup(x => x.ObtenirPlusieurs(x => x.Nom, new List<string> { "Utilisateur 6" })).Returns(new List<Abonne> { abonnes[5] });
            var abonnesMock = abonneMock.Object.ObtenirPlusieurs(x => x.Nom, new List<string> { "Utilisateur 6" });
            Assert.Equal(abonnesMock, new List<Abonne> { abonnes[5] });
        }
    }
}