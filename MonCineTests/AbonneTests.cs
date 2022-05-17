#region MÉTADONNÉES

// Nom du fichier : AbonneTests.cs
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
    public class AbonneTests
    {
        #region MÉTHODES

        private List<Abonne> GenerationAbonnes()
        {
            List<Abonne> abonnes = new List<Abonne>();

            List<Categorie> categories = new List<Categorie>
            {
                new Categorie(ObjectId.GenerateNewId(), "Horreur"),
                new Categorie(ObjectId.GenerateNewId(), "Fantastique"),
                new Categorie(ObjectId.GenerateNewId(), "Comédie"),
                new Categorie(ObjectId.GenerateNewId(), "Action"),
                new Categorie(ObjectId.GenerateNewId(), "Romance")
            };

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

            List<Realisateur> realisateurs = new List<Realisateur>()
            {
                new Realisateur(ObjectId.GenerateNewId(), "James Cameron"),
                new Realisateur(ObjectId.GenerateNewId(), "Steven Spielberg"),
                new Realisateur(ObjectId.GenerateNewId(), "Tim Burton"),
                new Realisateur(ObjectId.GenerateNewId(), "Gary Ross"),
                new Realisateur(ObjectId.GenerateNewId(), "Michael Bay")
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
                ObjectId.GenerateNewId(),
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
                ObjectId.GenerateNewId(),
                $"Utilisateur {2}",
                $"utilisateur{2}@email.com",
                $"user{2}",
                new Preference(
                    categoriesId,
                    acteursId,
                    realisateursId
                )
            ));
            abonnes.Add(new Abonne
            (
                ObjectId.GenerateNewId(),
                $"Utilisateur {3}",
                $"utilisateur{3}@email.com",
                $"user{3}",
                new Preference(
                    categoriesId,
                    acteursId,
                    realisateursId
                )
            ));
            abonnes.Add(new Abonne
            (
                ObjectId.GenerateNewId(),
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
                ObjectId.GenerateNewId(),
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
                ObjectId.GenerateNewId(),
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
            //List<Abonne> abonnes = GenerationAbonnes();
            //var abonneMock = new Mock<ICRUD<Abonne>>();

            //abonneMock.Setup(x => x.ObtenirTout()).Returns(abonnes);
            //var abonnesMock = abonneMock.Object.ObtenirTout();
            //Assert.Equal(abonnesMock, abonnes);
        }

        [Fact]
        public void ObtenirPlusieursRetourneAbonnesSelonFiltre()
        {
            //List<Abonne> abonnes = GenerationAbonnes();
            //var abonneMock = new Mock<ICRUD<Abonne>>();
            //abonneMock.Setup(x => x.ObtenirPlusieurs(x => x.Nom, new List<string> { "Utilisateur 6" })).Returns(new List<Abonne> { abonnes[5] });
            //var abonnesMock = abonneMock.Object.ObtenirPlusieurs(x => x.Nom, new List<string> { "Utilisateur 6" });
            //Assert.Equal(abonnesMock, new List<Abonne> { abonnes[5] });
        }

        #endregion
    }
}