#region MÉTADONNÉES

// Nom du fichier : ReservationTests.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading;
using MonCine.Data.Classes;
using MonCine.Data.Classes.BD;
using MonCine.Data.Classes.DAL;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using Xunit;

#endregion

namespace MonCineTests
{
    public class ReservationTests
    {
        #region CONSTANTES ET ATTRIBUTS STATIQUES

        private static readonly Random _rand = new Random();

        #endregion

        #region ATTRIBUTS

        private readonly Mock<IMongoClient> _mongoClientFilm;
        private readonly Mock<IMongoDatabase> _mongodbFilm;
        private readonly Mock<IMongoCollection<Film>> _filmCollection;
        private readonly Mock<IAsyncCursor<Film>> _filmCurseur;
        private readonly List<Film> _films;
        private readonly Mock<IMongoClient> _mongoClientCategorie;
        private readonly Mock<IMongoDatabase> _mongodbCategorie;
        private readonly Mock<IMongoCollection<Categorie>> _categorieCollection;
        private readonly Mock<IAsyncCursor<Categorie>> _categorieCurseur;
        private readonly List<Categorie> _categories;
        private readonly Mock<IMongoClient> _mongoClientActeur;
        private readonly Mock<IMongoDatabase> _mongodbActeur;
        private readonly Mock<IMongoCollection<Acteur>> _acteurCollection;
        private readonly Mock<IAsyncCursor<Acteur>> _acteurCurseur;
        private readonly List<Acteur> _acteurs;
        private readonly Mock<IMongoClient> _mongoClientRealisateur;
        private readonly Mock<IMongoDatabase> _mongodbRealisateur;
        private readonly Mock<IMongoCollection<Realisateur>> _realisateurCollection;
        private readonly Mock<IAsyncCursor<Realisateur>> _realisateurCurseur;
        private readonly List<Realisateur> _realisateurs;
        private readonly Mock<IMongoClient> _mongoClientAbonne;
        private readonly Mock<IMongoDatabase> _mongodbAbonne;
        private readonly Mock<IMongoCollection<Abonne>> _abonneCollection;
        private readonly Mock<IAsyncCursor<Abonne>> _abonneCurseur;
        private readonly List<Abonne> _abonnes;
        private readonly Mock<IMongoClient> _mongoClientReservation;
        private readonly Mock<IMongoDatabase> _mongodbReservation;
        private readonly Mock<IMongoCollection<Reservation>> _reservationCollection;
        private readonly Mock<IAsyncCursor<Reservation>> _reservationCurseur;
        private readonly List<Reservation> _reservations;

        #endregion

        #region CONSTRUCTEURS

        public ReservationTests()
        {
            _mongoClientFilm = new Mock<IMongoClient>();
            _mongodbFilm = new Mock<IMongoDatabase>();
            _filmCollection = new Mock<IMongoCollection<Film>>();
            _filmCurseur = new Mock<IAsyncCursor<Film>>();

            _mongoClientCategorie = new Mock<IMongoClient>();
            _mongodbCategorie = new Mock<IMongoDatabase>();
            _categorieCollection = new Mock<IMongoCollection<Categorie>>();
            _categorieCurseur = new Mock<IAsyncCursor<Categorie>>();

            _mongoClientActeur = new Mock<IMongoClient>();
            _mongodbActeur = new Mock<IMongoDatabase>();
            _acteurCollection = new Mock<IMongoCollection<Acteur>>();
            _acteurCurseur = new Mock<IAsyncCursor<Acteur>>();

            _mongoClientRealisateur = new Mock<IMongoClient>();
            _mongodbRealisateur = new Mock<IMongoDatabase>();
            _realisateurCollection = new Mock<IMongoCollection<Realisateur>>();
            _realisateurCurseur = new Mock<IAsyncCursor<Realisateur>>();

            _mongoClientAbonne = new Mock<IMongoClient>();
            _mongodbAbonne = new Mock<IMongoDatabase>();
            _abonneCollection = new Mock<IMongoCollection<Abonne>>();
            _abonneCurseur = new Mock<IAsyncCursor<Abonne>>();

            _mongoClientReservation = new Mock<IMongoClient>();
            _mongodbReservation = new Mock<IMongoDatabase>();
            _reservationCollection = new Mock<IMongoCollection<Reservation>>();
            _reservationCurseur = new Mock<IAsyncCursor<Reservation>>();

            _categories = new List<Categorie>
            {
                new Categorie(ObjectId.GenerateNewId(), "Horreur"),
                new Categorie(ObjectId.GenerateNewId(), "Fantastique"),
                new Categorie(ObjectId.GenerateNewId(), "Comédie"),
                new Categorie(ObjectId.GenerateNewId(), "Action"),
                new Categorie(ObjectId.GenerateNewId(), "Romance")
            };
            _acteurs = new List<Acteur>
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
            _realisateurs = new List<Realisateur>()
            {
                new Realisateur(ObjectId.GenerateNewId(), "James Cameron"),
                new Realisateur(ObjectId.GenerateNewId(), "Steven Spielberg"),
                new Realisateur(ObjectId.GenerateNewId(), "Tim Burton"),
                new Realisateur(ObjectId.GenerateNewId(), "Gary Ross"),
                new Realisateur(ObjectId.GenerateNewId(), "Michael Bay")
            };
            _abonnes = GenerAbonnes();
            _films = GenererFilms();
            _reservations = GenererReservations();
        }

        #endregion

        #region MÉTHODES

        private void InitializeMongoDb()
        {
            _mongoClientFilm.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(_mongodbFilm.Object);
            _mongodbFilm.Setup(x => x.GetCollection<Film>($"{typeof(Film).Name}s", default))
                .Returns(_filmCollection.Object);

            _mongoClientCategorie.Setup(x => x.GetDatabase(It.IsAny<string>(), default))
                .Returns(_mongodbCategorie.Object);
            _mongodbCategorie.Setup(x => x.GetCollection<Categorie>($"{typeof(Categorie).Name}s", default))
                .Returns(_categorieCollection.Object);

            _mongoClientActeur.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(_mongodbActeur.Object);
            _mongodbActeur.Setup(x => x.GetCollection<Acteur>($"{typeof(Acteur).Name}s", default))
                .Returns(_acteurCollection.Object);

            _mongoClientRealisateur.Setup(x => x.GetDatabase(It.IsAny<string>(), default))
                .Returns(_mongodbRealisateur.Object);
            _mongodbRealisateur.Setup(x => x.GetCollection<Realisateur>($"{typeof(Realisateur).Name}s", default))
                .Returns(_realisateurCollection.Object);

            _mongoClientAbonne.Setup(x => x.GetDatabase(It.IsAny<string>(), default)).Returns(_mongodbAbonne.Object);
            _mongodbAbonne.Setup(x => x.GetCollection<Abonne>($"{typeof(Abonne).Name}s", default))
                .Returns(_abonneCollection.Object);

            _mongoClientReservation.Setup(x => x.GetDatabase(It.IsAny<string>(), default))
                .Returns(_mongodbReservation.Object);
            _mongodbReservation.Setup(x => x.GetCollection<Reservation>($"{typeof(Reservation).Name}s", default))
                .Returns(_reservationCollection.Object);
        }

        private void InitializeMongoCollection()
        {
            _filmCurseur.Setup(x => x.Current).Returns(_films);
            _filmCurseur.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            _filmCollection
                .Setup(
                    x => x.FindSync(Builders<Film>.Filter.Empty,
                        It.IsAny<FindOptions<Film>>(), default)
                )
                .Returns(_filmCurseur.Object);

            _categorieCurseur.Setup(x => x.Current).Returns(_categories);
            _categorieCurseur.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true)
                .Returns(false);
            _categorieCollection
                .Setup(
                    x => x.FindSync(Builders<Categorie>.Filter.Empty,
                        It.IsAny<FindOptions<Categorie>>(), default)
                )
                .Returns(_categorieCurseur.Object);

            _acteurCurseur.Setup(x => x.Current).Returns(_acteurs);
            _acteurCurseur.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            _acteurCollection
                .Setup(
                    x => x.FindSync(Builders<Acteur>.Filter.Empty,
                        It.IsAny<FindOptions<Acteur>>(), default)
                )
                .Returns(_acteurCurseur.Object);

            _realisateurCurseur.Setup(x => x.Current).Returns(_realisateurs);
            _realisateurCurseur.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true)
                .Returns(false);
            _realisateurCollection
                .Setup(
                    x => x.FindSync(Builders<Realisateur>.Filter.Empty,
                        It.IsAny<FindOptions<Realisateur>>(), default)
                )
                .Returns(_realisateurCurseur.Object);

            _abonneCurseur.Setup(x => x.Current).Returns(_abonnes);
            _abonneCurseur.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true).Returns(false);
            _abonneCollection
                .Setup(
                    x => x.FindSync(Builders<Abonne>.Filter.Empty,
                        It.IsAny<FindOptions<Abonne>>(), default)
                )
                .Returns(_abonneCurseur.Object);

            _reservationCurseur.Setup(x => x.Current).Returns(_reservations);
            _reservationCurseur.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true)
                .Returns(false);
            _reservationCollection
                .Setup(
                    x => x.FindSync(Builders<Reservation>.Filter.Empty,
                        It.IsAny<FindOptions<Reservation>>(), default)
                )
                .Returns(_reservationCurseur.Object);
            InitializeMongoDb();
        }

        private List<Film> GenererFilms()
        {
            List<Film> films = new List<Film>();
            List<Salle> salles = new List<Salle>
            {
                new Salle(ObjectId.GenerateNewId(), "Salle 1", 30),
                new Salle(ObjectId.GenerateNewId(), "Salle 2", 25),
                new Salle(ObjectId.GenerateNewId(), "Salle 3", 27),
                new Salle(ObjectId.GenerateNewId(), "Salle 4", 28),
                new Salle(ObjectId.GenerateNewId(), "Salle 5", 10)
            };

            List<ObjectId> categoriesId = new List<ObjectId>();
            _categories
                .GetRange(0, 5)
                .ForEach(x => categoriesId.Add(x.Id));
            int startIndex = ReservationTests._rand.Next(0, _acteurs.Count);
            List<ObjectId> acteursId = new List<ObjectId>();
            _acteurs
                .GetRange(startIndex, ReservationTests._rand.Next(0, _acteurs.Count - startIndex))
                .ForEach(x => acteursId.Add(x.Id));
            startIndex = ReservationTests._rand.Next(0, _realisateurs.Count);
            List<ObjectId> realisateursId = new List<ObjectId>();
            _realisateurs
                .GetRange(startIndex, ReservationTests._rand.Next(0, _realisateurs.Count - startIndex))
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
                    ObjectId.GenerateNewId(),
                    nom,
                    DateTime.Now,
                    new List<Projection>(),
                    GenererNotes(),
                    categoriesId[ReservationTests._rand.Next(0, categoriesId.Count - 1)],
                    acteursId,
                    realisateursId
                );
                films.Add(film);
            }

            // Génération de projection pour certains films choisis aléatoirement
            for (int i = 0; i < films.Count; i += ReservationTests._rand.Next(1, 3))
            {
                ReservationTests.GenererProjections(films[i], salles);
            }

            return films;
        }

        private List<Abonne> GenerAbonnes()
        {
            List<Abonne> abonnes = new List<Abonne>();

            List<ObjectId> categoriesId = new List<ObjectId>();
            _categories
                .GetRange(0, 3)
                .ForEach(x => categoriesId.Add(x.Id));

            List<ObjectId> acteursId = new List<ObjectId>();
            _acteurs
                .GetRange(0, 5)
                .ForEach(x => acteursId.Add(x.Id));

            List<ObjectId> realisateursId = new List<ObjectId>();
            _realisateurs
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

        private List<Note> GenererNotes()
        {
            int nbNotes = ReservationTests._rand.Next(1, 11);
            List<int> indexAbonnesOntNote = new List<int>();
            List<Note> notes = new List<Note>();
            for (int i = 0; i < nbNotes; i++)
            {
                int index = ReservationTests._rand.Next(0, _abonnes.Count - 1);
                if (!indexAbonnesOntNote.Contains(index))
                {
                    notes.Add(new Note(_abonnes[index].Id, ReservationTests._rand.Next(10)));
                    indexAbonnesOntNote.Add(index);
                }
            }

            return notes;
        }

        private static void GenererProjections(Film pFilm, List<Salle> pSalles)
        {
            DateTime dateFin = DateTime.Now;
            int nbProjections = ReservationTests._rand.Next(0, 20);
            for (int i = 0; i < nbProjections; i++)
            {
                int heureSuppDebut = ReservationTests._rand.Next(1, 23);

                DateTime dateDebut = dateFin.AddHours(heureSuppDebut);
                dateFin = dateDebut.AddHours(ReservationTests._rand.Next(1, 23));

                pFilm.AjouterProjection(dateDebut, dateFin, pSalles[ReservationTests._rand.Next(0, pSalles.Count - 1)]);
            }
        }

        private List<Reservation> GenererReservations()
        {
            List<Reservation> reservations = new List<Reservation>();
            int nbReservations = _rand.Next(30, 60);
            for (int i = 0; i < nbReservations; i++)
            {
                Film film = _films[_rand.Next(0, _films.Count - 1)];
                if (film.Projections.Count > 0)
                {
                    int indexProjection = _rand.Next(0, film.Projections.Count - 1);
                    if (film.Projections[indexProjection].EstActive)
                    {
                        int nbPlaces = _rand.Next(1, 10);
                        if (film.Projections[indexProjection].NbPlacesRestantes - nbPlaces > -1)
                        {
                            reservations.Add(new Reservation(
                                ObjectId.GenerateNewId(),
                                film,
                                indexProjection,
                                _abonnes[_rand.Next(0, _abonnes.Count - 1)].Id,
                                nbPlaces
                            ));
                        }
                    }
                }
            }

            return reservations;
        }

        [Fact]
        public void ObtenirToutRetourneToutesLesReservations()
        {
            // Création des faux objets
            InitializeMongoCollection();

            // Arrange
            DALCategorie dalCategorie = new DALCategorie(_mongoClientCategorie.Object);
            DALActeur dalActeur = new DALActeur(_mongoClientActeur.Object);
            DALRealisateur dalRealisateur = new DALRealisateur(_mongoClientRealisateur.Object);
            DALAbonne dalAbonne = new DALAbonne(pClient: _mongoClientAbonne.Object, pDalCategorie: dalCategorie,
                pDalActeur: dalActeur, pDalRealisateur: dalRealisateur);
            DALFilm dalFilm = new DALFilm(pClient: _mongoClientFilm.Object, pDalCategorie: dalCategorie,
                pDalActeur: dalActeur, pDalRealisateur: dalRealisateur, pDalAbonne: dalAbonne,
                pClientReservation: _mongoClientReservation.Object);
            DALReservation dalReservation = new DALReservation(dalFilm, _mongoClientReservation.Object);

            // Act et Assert
            Assert.Equal(_reservations, dalReservation.ObtenirTout());
        }

        [Fact]
        public void ObtenirPlusieursRetourneReservationsSelonFiltre()
        {
            // Création des faux objets
            InitializeMongoCollection();

            // Arrange
            DALCategorie dalCategorie = new DALCategorie(_mongoClientCategorie.Object);
            DALActeur dalActeur = new DALActeur(_mongoClientActeur.Object);
            DALRealisateur dalRealisateur = new DALRealisateur(_mongoClientRealisateur.Object);
            DALAbonne dalAbonne = new DALAbonne(pClient: _mongoClientAbonne.Object, pDalCategorie: dalCategorie,
                pDalActeur: dalActeur, pDalRealisateur: dalRealisateur);
            DALFilm dalFilm = new DALFilm(pClient: _mongoClientFilm.Object, pDalCategorie: dalCategorie,
                pDalActeur: dalActeur, pDalRealisateur: dalRealisateur, pDalAbonne: dalAbonne,
                pClientReservation: _mongoClientReservation.Object);
            DALReservation dalReservation = new DALReservation(dalFilm, _mongoClientReservation.Object);

            // Act et Assert
            Assert.Equal(_reservations.Where(x => x.AbonneId == _abonnes[3].Id),
                dalReservation.ObtenirPlusieurs(x => x.AbonneId == _abonnes[3].Id));
        }

        [Fact]
        public void InsererUnRetourneVrai()
        {
            //    // Création des faux objets
            //    InitializeMongoCollection();

            //    // Arrange
            //    DALCategorie dalCategorie = new DALCategorie(_mongoClientCategorie.Object);
            //    DALActeur dalActeur = new DALActeur(_mongoClientActeur.Object);
            //    DALRealisateur dalRealisateur = new DALRealisateur(_mongoClientRealisateur.Object);
            //    DALAbonne dalAbonne = new DALAbonne(pClient: _mongoClientAbonne.Object, pDalCategorie: dalCategorie,
            //        pDalActeur: dalActeur, pDalRealisateur: dalRealisateur);
            //    DALFilm dalFilm = new DALFilm(pClient: _mongoClientFilm.Object, pDalCategorie: dalCategorie,
            //        pDalActeur: dalActeur, pDalRealisateur: dalRealisateur, pDalAbonne: dalAbonne,
            //        pClientReservation: _mongoClientReservation.Object);
            //    DALReservation dalReservation = new DALReservation(dalFilm, _mongoClientReservation.Object);
            //    // Vérifier que insererUn retourne True
            //    // Act
            //    Reservation r = new Reservation(new ObjectId(), _films[0], 0, _abonnes[0].Id, 1);

            //    // Assert
            //    Assert.True(dalReservation.InsererUn(r));
        }

        [Fact]
        public void InsererUnAjouteReservation()
        {
            //    // Création des faux objets
            //    InitializeMongoCollection();

            //    // Arrange
            //    DALCategorie dalCategorie = new DALCategorie(_mongoClientCategorie.Object);
            //    DALActeur dalActeur = new DALActeur(_mongoClientActeur.Object);
            //    DALRealisateur dalRealisateur = new DALRealisateur(_mongoClientRealisateur.Object);
            //    DALAbonne dalAbonne = new DALAbonne(pClient: _mongoClientAbonne.Object, pDalCategorie: dalCategorie,
            //        pDalActeur: dalActeur, pDalRealisateur: dalRealisateur);
            //    DALFilm dalFilm = new DALFilm(pClient: _mongoClientFilm.Object, pDalCategorie: dalCategorie,
            //        pDalActeur: dalActeur, pDalRealisateur: dalRealisateur, pDalAbonne: dalAbonne,
            //        pClientReservation: _mongoClientReservation.Object);
            //    DALReservation dalReservation = new DALReservation(dalFilm, _mongoClientReservation.Object);

            //    // Vérifier que InsererUn a ajouté la réservation et utilisé obtenir 1
            //    // Act
            //    Reservation r = new Reservation(new ObjectId(), _films[0], 0, _abonnes[0].Id, 1);
            //    dalReservation.InsererUn(r);
            //    Reservation resultat = dalReservation.ObtenirUn(r.Id);
            //    dalFilm.MAJProjections(_films[0]);

            //    // Assert
            //    Assert.Equal(r, resultat);
        }

        [Fact]
        public void InsererUnDiminueNbPlacesRestantesDeProjectionDuFilm()
        {
            //    // Création des faux objets
            //    InitializeMongoCollection();

            //    // Arrange
            //    DALCategorie dalCategorie = new DALCategorie(_mongoClientCategorie.Object);
            //    DALActeur dalActeur = new DALActeur(_mongoClientActeur.Object);
            //    DALRealisateur dalRealisateur = new DALRealisateur(_mongoClientRealisateur.Object);
            //    DALAbonne dalAbonne = new DALAbonne(pClient: _mongoClientAbonne.Object, pDalCategorie: dalCategorie,
            //        pDalActeur: dalActeur, pDalRealisateur: dalRealisateur);
            //    DALFilm dalFilm = new DALFilm(pClient: _mongoClientFilm.Object, pDalCategorie: dalCategorie,
            //        pDalActeur: dalActeur, pDalRealisateur: dalRealisateur, pDalAbonne: dalAbonne,
            //        pClientReservation: _mongoClientReservation.Object);
            //    DALReservation dalReservation = new DALReservation(dalFilm, _mongoClientReservation.Object);

            //    // Vérifier nombre de place de la projection a diminué lors de la réservation
            //    // Act
            //    Reservation r = new Reservation(new ObjectId(), _films[0], 0, _abonnes[0].Id, 1);
            //    dalReservation.InsererUn(r);
            //    Reservation resultat = dalReservation.ObtenirUn(r.Id);
            //    dalFilm.MAJProjections(_films[0]);

            //    // Assert
            //    Assert.Equal(r.NbPlaces - 1, resultat.NbPlaces);
        }

        #endregion
    }
}