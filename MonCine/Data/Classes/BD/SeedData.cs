#region MÉTADONNÉES

// Nom du fichier : SeedData.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MonCine.Data.Classes.DAL;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

#endregion

namespace MonCine.Data.Classes.BD
{
    /// <summary>
    /// Classe permettant l'auto-génération des données
    /// </summary>
    public class SeedData
    {
        #region CONSTANTES ET ATTRIBUTS STATIQUES

        private static readonly Random _rand = new Random();

        #endregion

        #region MÉTHODES

        public static void GenererDonneesDeBD(IMongoClient pClient, IMongoDatabase pDb)
        {
            DALAdministrateur dalAdministrateur = new DALAdministrateur(pClient, pDb);
            SeedData.GenererAdministrateur(dalAdministrateur);

            DALCategorie dalCategorie = new DALCategorie(pClient, pDb);
            SeedData.GenererCategories(dalCategorie);
            List<Categorie> categories = dalCategorie.ObtenirTout();

            DALActeur dalActeur = new DALActeur(pClient, pDb);
            SeedData.GenererActeurs(dalActeur);
            List<Acteur> acteurs = dalActeur.ObtenirTout();

            DALRealisateur dalRealisateur = new DALRealisateur(pClient, pDb);
            SeedData.GenererRealisateurs(dalRealisateur);
            List<Realisateur> realisateurs = dalRealisateur.ObtenirTout();

            DALSalle dalSalle = new DALSalle();
            SeedData.GenererSalles(dalSalle);

            DALFilm dalFilm = new DALFilm(dalCategorie, dalActeur, dalRealisateur, pClient, pDb);
            SeedData.GenererFilms(dalFilm, categories, acteurs, realisateurs, dalSalle.ObtenirTout());

            DALReservation dalReservation = new DALReservation(dalFilm, pClient, pDb);
            DALAbonne dalAbonne = new DALAbonne(dalCategorie, dalActeur, dalRealisateur, dalReservation, pClient, pDb);
            SeedData.GenererAbonnes(dalAbonne, categories, acteurs, realisateurs);

            SeedData.GenererNotes(dalFilm, dalFilm.ObtenirTout(), dalAbonne.ObtenirTout());

            SeedData.GenererReservations(dalReservation, dalFilm.ObtenirTout(), dalAbonne.ObtenirTout());

            SeedData.GenererRecompenses(new DALRecompense(dalFilm, pClient, pDb), dalFilm.ObtenirTout(),
                dalAbonne.ObtenirTout());
        }

        private static void GenererAdministrateur(DALAdministrateur pDalAdministrateur)
        {
            try
            {
                Administrateur administrateur = pDalAdministrateur.ObtenirUn();
                if (administrateur == null)
                {
                    pDalAdministrateur.InsererUn(
                        new Administrateur(
                            new ObjectId(),
                            "Administrateur",
                            "admin@email.com",
                            "admin"
                        )
                    );
                }
            }
            catch (IndexOutOfRangeException)
            {
                throw;
            }
            catch (ExceptionBD)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : GenererAdministrateur - Exception : {e.Message}");
            }
        }

        private static void GenererCategories(DALCategorie pDalCategorie)
        {
            try
            {
                List<Categorie> categories = pDalCategorie.ObtenirTout();
                if (categories.Count == 0)
                {
                    pDalCategorie.InsererPlusieurs(
                        new List<Categorie>
                        {
                            new Categorie(new ObjectId(), "Horreur"),
                            new Categorie(new ObjectId(), "Fantastique"),
                            new Categorie(new ObjectId(), "Comédie"),
                            new Categorie(new ObjectId(), "Action"),
                            new Categorie(new ObjectId(), "Romance")
                        }
                    );
                }
            }
            catch (ExceptionBD)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : GenererCategories - Exception : {e.Message}");
            }
        }

        private static void GenererActeurs(DALActeur pDalActeur)
        {
            try
            {
                List<Acteur> acteurs = pDalActeur.ObtenirTout();
                if (acteurs.Count == 0)
                {
                    pDalActeur.InsererPlusieurs(
                        new List<Acteur>
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
                        }
                    );
                }
            }
            catch (ExceptionBD)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : GenererAbonnes - Exception : {e.Message}");
            }
        }

        private static void GenererRealisateurs(DALRealisateur pDalRealisateur)
        {
            try
            {
                List<Realisateur> realisateurs = pDalRealisateur.ObtenirTout();
                if (realisateurs.Count == 0)
                {
                    pDalRealisateur.InsererPlusieurs(
                        new List<Realisateur>()
                        {
                            new Realisateur(new ObjectId(), "James Cameron"),
                            new Realisateur(new ObjectId(), "Steven Spielberg"),
                            new Realisateur(new ObjectId(), "Tim Burton"),
                            new Realisateur(new ObjectId(), "Gary Ross"),
                            new Realisateur(new ObjectId(), "Michael Bay")
                        }
                    );
                }
            }
            catch (ExceptionBD)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : GenererRealisateurs - Exception : {e.Message}");
            }
        }

        private static void GenererSalles(DALSalle pDalSalle)
        {
            try
            {
                List<Salle> salles = pDalSalle.ObtenirTout();
                if (salles.Count == 0)
                {
                    int nbSalles = SeedData._rand.Next(10, 20);
                    for (int i = 0; i < nbSalles; i++)
                    {
                        salles.Add(new Salle(
                            new ObjectId(),
                            $"Salle {i + 1}",
                            SeedData._rand.Next(20, 40)
                        ));
                    }
                    pDalSalle.InsererPlusieurs(salles);
                }
            }
            catch (ExceptionBD)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : GenererRealisateurs - Exception : {e.Message}");
            }
        }

        private static void GenererFilms(DALFilm pDalFilm, List<Categorie> pCategories, List<Acteur> pActeurs,
            List<Realisateur> pRealisateurs, List<Salle> pSalles)
        {
            try
            {
                List<Film> films = pDalFilm.ObtenirTout();
                if (!films.Any())
                {
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
                    foreach (string nom in nomsFilm)
                    {
                        films.Add(SeedData.GenererFilm(nom, pCategories, pActeurs, pRealisateurs));
                    }
                    for (int i = 0; i < films.Count; i += SeedData._rand.Next(1, 3))
                    {
                        SeedData.GenererProjections(films[i], pSalles);
                    }
                    pDalFilm.InsererPlusieurs(films);
                }
            }
            catch (ArgumentOutOfRangeException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (ExceptionBD)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : GenererFilms - Exception : {e.Message}");
            }
        }

        private static Film GenererFilm(string pNom, List<Categorie> pCategories, List<Acteur> pActeurs,
            List<Realisateur> pRealisateurs)
        {
            DateTime dateSortie = DateTime.Now;
            dateSortie = dateSortie.AddYears(-1 * SeedData._rand.Next(30));
            List<ObjectId> acteursId = new List<ObjectId>();
            pActeurs
                .GetRange(0, SeedData._rand.Next(1, SeedData._rand.Next(2, pActeurs.Count)))
                .ForEach(x => acteursId.Add(x.Id));
            List<ObjectId> realisateursId = new List<ObjectId>();
            pRealisateurs
                .GetRange(0, SeedData._rand.Next(1, SeedData._rand.Next(2, pRealisateurs.Count)))
                .ForEach(x => realisateursId.Add(x.Id));
            Film film = new Film
            (
                new ObjectId(),
                pNom,
                dateSortie,
                new List<Projection>(),
                new List<Note>(),
                pCategories[SeedData._rand.Next(pCategories.Count - 1)].Id,
                acteursId,
                realisateursId
            );

            return film;
        }

        private static void GenererProjections(Film pFilm, List<Salle> pSalles)
        {
            DateTime dateFin = DateTime.Now;
            int nbProjections = SeedData._rand.Next(0, 20);
            for (int i = 0; i < nbProjections; i++)
            {
                int heureSuppDebut = SeedData._rand.Next(1, 23);
                DateTime dateDebut = dateFin.AddHours(heureSuppDebut);
                dateFin = dateDebut.AddHours(SeedData._rand.Next(1, 23));
                pFilm.AjouterProjection(dateDebut, dateFin, pSalles[SeedData._rand.Next(0, pSalles.Count - 1)]);
            }
        }

        private static void GenererAbonnes(DALAbonne pDalAbonne, List<Categorie> pCategories,
            List<Acteur> pActeurs, List<Realisateur> pRealisateurs)
        {
            try
            {
                List<Abonne> abonnes = pDalAbonne.ObtenirTout();
                if (!abonnes.Any())
                {
                    int nbAbonnesGeneres = SeedData._rand.Next(6, 30);
                    for (int i = 0; i < nbAbonnesGeneres; i++)
                    {
                        abonnes.Add(SeedData.GenererAbonne(i + 1, pCategories, pActeurs, pRealisateurs));
                    }
                    pDalAbonne.InsererPlusieurs(abonnes);
                }
            }
            catch (ExceptionBD)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : ObtenirAbonnes - Exception : {e.Message}");
            }
        }

        private static Abonne GenererAbonne(int numero, List<Categorie> pCategories,
            List<Acteur> pActeurs, List<Realisateur> pRealisateurs)
        {
            List<ObjectId> categoriesId = new List<ObjectId>();
            pCategories
                .GetRange(0, SeedData._rand.Next(0, Preference.NB_MAX_CATEGORIES_PREF))
                .ForEach(x => categoriesId.Add(x.Id));
            List<ObjectId> acteursId = new List<ObjectId>();
            pActeurs
                .GetRange(0, SeedData._rand.Next(0, Preference.NB_MAX_ACTEURS_PREF))
                .ForEach(x => acteursId.Add(x.Id));
            List<ObjectId> realisateursId = new List<ObjectId>();
            pRealisateurs
                .GetRange(0, SeedData._rand.Next(0, Preference.NB_MAX_REALISATEURS_PREF))
                .ForEach(x => realisateursId.Add(x.Id));

            return new Abonne
            (
                new ObjectId(),
                $"Utilisateur {numero}",
                $"utilisateur_{numero}@email.com",
                $"user{numero}",
                new Preference(
                    categoriesId,
                    acteursId,
                    realisateursId
                )
            );
        }

        private static void GenererNotes(DALFilm pDalFilm, List<Film> pFilms, List<Abonne> pAbonnes)
        {
            try
            {
                foreach (Film film in pFilms)
                {
                    if (film.Notes.Count == 0 && pAbonnes.Count > 0)
                    {
                        int nbNotes = SeedData._rand.Next(1, 11);
                        List<int> indexAbonnesOntNote = new List<int>();
                        for (int i = 0; i < nbNotes; i++)
                        {
                            int index = SeedData._rand.Next(0, pAbonnes.Count - 1);
                            if (!indexAbonnesOntNote.Contains(index))
                            {
                                film.Notes.Add(new Note(pAbonnes[index].Id, SeedData._rand.Next(10)));
                                indexAbonnesOntNote.Add(index);
                            }
                        }

                        pDalFilm.MAJUn(
                            x => x.Id == film.Id,
                            new List<(Expression<Func<Film, object>> field, object value)>
                            {
                                (x => x.Notes, film.Notes)
                            }
                        );
                    }
                }
            }
            catch (ExceptionBD)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : ObtenirAbonnes - Exception : {e.Message}");
            }
        }

        private static void GenererReservations(DALReservation pDalReservation, List<Film> pFilms,
            List<Abonne> pAbonnes)
        {
            try
            {
                List<Reservation> reservations = pDalReservation.ObtenirTout();
                if (!reservations.Any())
                {
                    int nbReservations = SeedData._rand.Next(30, 60);
                    for (int i = 0; i < nbReservations; i++)
                    {
                        Film film = pFilms[SeedData._rand.Next(0, pFilms.Count - 1)];
                        if (film.Projections.Count > 0)
                        {
                            int indexProjection = SeedData._rand.Next(0, film.Projections.Count - 1);
                            if (film.Projections[indexProjection].EstActive)
                            {
                                int nbPlaces = SeedData._rand.Next(1, 10);
                                if (film.Projections[indexProjection].NbPlacesRestantes - nbPlaces > -1)
                                {
                                    pDalReservation.InsererUne(new Reservation(
                                        new ObjectId(),
                                        film,
                                        indexProjection,
                                        pAbonnes[SeedData._rand.Next(0, pAbonnes.Count - 1)].Id,
                                        nbPlaces
                                    ));
                                }
                            }
                        }
                    }
                }
            }
            catch (ExceptionBD)
            {
                throw;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : GenererReservations - Exception : {e.Message}");
            }
        }

        private static void GenererRecompenses(DALRecompense pDalRecompense, List<Film> pFilms, List<Abonne> pAbonnes)
        {
            try
            {
                List<Recompense> recompenses = pDalRecompense.ObtenirRecompenses();

                if (!recompenses.Any())
                {
                    recompenses.AddRange(SeedData.GenererTicketGratuits(pFilms, pAbonnes));
                    if (recompenses.Count > 0)
                    {
                        pDalRecompense.InsererPlusieursRecompenses(recompenses);
                    }
                }
            }
            catch (ExceptionBD e)
            {
                throw e;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : GenererRecompenses - Exception : {e.Message}");
            }
        }

        private static List<TicketGratuit> GenererTicketGratuits(List<Film> pFilms, List<Abonne> pAbonnes)
        {
            List<TicketGratuit> ticketGratuits = new List<TicketGratuit>();
            int nbTicketGratuitsGeneres = SeedData._rand.Next(2, pFilms.Count);
            for (int i = 0; i < nbTicketGratuitsGeneres; i++)
            {
                ticketGratuits.Add(new TicketGratuit(
                    new ObjectId(),
                    pFilms[i].Id,
                    pAbonnes[SeedData._rand.Next(0, pAbonnes.Count - 1)].Id)
                );
            }

            return ticketGratuits;
        }

        #endregion
    }
}