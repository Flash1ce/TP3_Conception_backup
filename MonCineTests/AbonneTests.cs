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
                    new Categorie(new ObjectId(), "Horreur"),
                    new Categorie(new ObjectId(), "Fantastique"),
                    new Categorie(new ObjectId(), "Comédie"),
                    new Categorie(new ObjectId(), "Action"),
                    new Categorie(new ObjectId(), "Romance")
                };

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

            List<Realisateur> realisateurs = new List<Realisateur>()
                {
                    new Realisateur(new ObjectId(), "James Cameron"),
                    new Realisateur(new ObjectId(), "Steven Spielberg"),
                    new Realisateur(new ObjectId(), "Tim Burton"),
                    new Realisateur(new ObjectId(), "Gary Ross"),
                    new Realisateur(new ObjectId(), "Michael Bay")
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