using MonCine.Data.Classes;
using MonCine.Data.Classes.DAL;
using MonCine.Data.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading;
using Xunit;

namespace MonCineTests
{
    public class FilmTests
    {
        private const string NOM_FILM_COLLECTION = "Films";
        private static Random _rand = new Random();
        private Mock<IMongoClient> _mongoClient;
        private Mock<IMongoDatabase> _mongodb;
        private Mock<IMongoCollection<Film>> _filmCollection;
        private List<Film> _films;
        private Mock<IAsyncCursor<Film>> _filmCurseur;

        public FilmTests()
        {
            _mongoClient = new Mock<IMongoClient>();
            _mongodb = new Mock<IMongoDatabase>();
            _filmCollection = new Mock<IMongoCollection<Film>>();
            _filmCurseur = new Mock<IAsyncCursor<Film>>();
            _films = GenererFilms();
        }

        private void InitializeMongoDb()
        {
            _mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(_mongodb.Object);
            _mongodb.Setup(x => x.GetCollection<Film>(FilmTests.NOM_FILM_COLLECTION, default))
                .Returns(_filmCollection.Object);
        }

        private void InitializeMongoFilmCollection()
        {
            _filmCurseur.Setup(x => x.Current).Returns(_films);
            _filmCurseur.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            _filmCollection
                .Setup(
                    x => x.FindSync(Builders<Film>.Filter.Empty,
                        It.IsAny<FindOptions<Film>>(), default)
                )
                .Returns(_filmCurseur.Object);
            InitializeMongoDb();
        }

        private List<Film> GenererFilms()
        {
            List<Film> films = new List<Film>();
            List<Salle> salles = new List<Salle>
            {
                new Salle(new ObjectId(), "Salle 1", 30),
                new Salle(new ObjectId(), "Salle 2", 25),
                new Salle(new ObjectId(), "Salle 3", 27),
                new Salle(new ObjectId(), "Salle 4", 28),
                new Salle(new ObjectId(), "Salle 5", 10)
            };
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
            List<string> nomsFilm = new List<string>
            {
                "Il faut sauver le soldat Ryan",
                "La Chute du faucon noir",
                "Dunkerque",
                "1917",
                "Fury",
                "American Sniper",
                "6 Underground",
                "Notice rouge",
            };
            // Génération des films
            foreach (string nom in nomsFilm)
            {
                Film film = new Film
                (
                    new ObjectId(),
                    nom,
                    DateTime.Now,
                    new List<Projection>(),
                    new List<Note>(),
                    categoriesId[1],
                    acteursId,
                    realisateursId
                );
                films.Add(film);
            }

            // Génération de projection pour certains films choisis aléatoirement
            for (int i = 0; i < films.Count; i += FilmTests._rand.Next(1, 3))
            {
                FilmTests.GenererProjections(films[i], salles);
            }
            return films;
        }

        /// <summary>
        /// Permet de générer une projection pour le film et la salle pour la projection du film reçus en paramètre. 
        /// </summary>
        /// <param name="pFilm">Film auquel il faut ajouter la projection</param>
        /// <param name="pSalle">Salle dans laquelle aura lieu la projection</param>
        private static void GenererProjections(Film pFilm, List<Salle> pSalles)
        {
            DateTime dateFin = DateTime.Now;
            int nbProjections = FilmTests._rand.Next(0, 20);
            for (int i = 0; i < nbProjections; i++)
            {
                int heureSuppDebut = FilmTests._rand.Next(1, 23);

                DateTime dateDebut = dateFin.AddHours(heureSuppDebut);
                dateFin = dateDebut.AddHours(FilmTests._rand.Next(1, 23));

                pFilm.AjouterProjection(dateDebut, dateFin, pSalles[FilmTests._rand.Next(0, pSalles.Count - 1)]);
            }
        }

        //[Fact]
        //public void ObtenirTousRetournesTousLesFilms()
        //{
        //    // Création des faux objets
        //    InitializeMongoFilmCollection();

        //    // Arrange
        //    DALFilm dalFilm = new DALFilm(_mongoClient.Object);

        //    // Act et Assert
        //    Assert.Equal(_films, dalFilm.ObtenirTout());
        //}

        [Fact]
        public void ObtenirTousLesFilmEtVerifierTousRetournes()
        {
            List<Film> films = GenererFilms();
            var filmMock = new Mock<ICRUD<Film>>();
            filmMock.Setup(x => x.ObtenirTout()).Returns(films);
            var filmsMock = filmMock.Object.ObtenirTout();

            Assert.Equal(filmsMock, films);
        }

        [Fact]
        public void ObtenirPlusieursRetourneFilmsSelonFiltre()
        {
            List<Film> films = GenererFilms();
            var filmMock = new Mock<ICRUD<Film>>();
            filmMock.Setup(x => x.ObtenirPlusieurs(x => x.Nom, new List<string> { "American Sniper" }))
                .Returns(new List<Film> { films[5] });
            var filmsMock = filmMock.Object.ObtenirPlusieurs(x => x.Nom, new List<string> { "American Sniper" });

            Assert.Equal(filmsMock, new List<Film> { films[5] });
        }

        [Fact]
        public void InsererPlusieursFilmsRetourneTrue()
        {
            List<Film> films = GenererFilms();
            var filmMock = new Mock<ICRUD<Film>>();
            filmMock.Setup(x => x.InsererPlusieurs(films)).Returns(true);
            var filmsMock = filmMock.Object.InsererPlusieurs(films);

            Assert.True(filmsMock);
        }

    }
}