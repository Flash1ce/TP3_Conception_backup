#region MÉTADONNÉES

// Nom du fichier : Film.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;

#endregion

namespace MonCine.Data.Classes
{
    public class Film
    {
        #region CONSTANTES ET ATTRIBUTS STATIQUES

        public const int NB_MAX_EST_AFFICHE_PAR_ANNEE = 2;

        #endregion

        #region PROPRIÉTÉS ET INDEXEURS

        public ObjectId Id { get; set; }
        public string Nom { get; set; }
        public DateTime DateSortie { get; set; }
        public List<Projection> Projections { get; set; }
        public List<DateTime> DatesFinsAffiche { get; set; }

        [BsonIgnore]
        public bool EstAffiche
        {
            get
            {
                if (Projections != null && Projections.Count > 0)
                {
                    Projection derniereProjection = Projections[Projections.Count - 1];
                    bool estNonAffiche = derniereProjection.EstActive && derniereProjection.DateFin < DateTime.Now;
                    bool auMoinsUneProjectionActive = Projections.Find(x => x.EstActive) != null;
                    if (estNonAffiche)
                    {
                        RetirerAffiche();
                    }
                    else if (auMoinsUneProjectionActive)
                    {
                        return true;
                    }
                }

                return false;
            }
        }

        public List<Note> Notes { get; set; }

        [BsonIgnore]
        public double NoteMoy
        {
            get
            {
                if (Notes == null)
                {
                    throw new NullReferenceException(
                        "Il est impossible d'obtenir la note moyenne du film puisque la liste des notes est nulle."
                    );
                }

                int nbNotes = Notes.Count;
                return nbNotes > 0 ? Notes.Sum(pX => pX.NoteFilm) / nbNotes : 0;
            }
        }

        public ObjectId CategorieId { get; set; }
        public List<ObjectId> ActeursId { get; set; }
        public List<ObjectId> RealisateursId { get; set; }
        [BsonIgnore] public Categorie Categorie { get; set; }
        [BsonIgnore] public List<Acteur> Acteurs { get; set; }
        [BsonIgnore] public List<Realisateur> Realisateurs { get; set; }

        #endregion

        #region CONSTRUCTEURS

        public Film(ObjectId pId, string pNom, DateTime pDateSortie, List<Projection> pProjections, List<Note> pNotes,
            ObjectId pCategorieId, List<ObjectId> pActeursId, List<ObjectId> pRealisateursId)
        {
            Id = pId;
            Nom = pNom;
            DateSortie = pDateSortie;
            Projections = pProjections;
            Notes = pNotes;
            CategorieId = pCategorieId;
            ActeursId = pActeursId;
            RealisateursId = pRealisateursId;
            DatesFinsAffiche = new List<DateTime>();
        }

        #endregion

        #region MÉTHODES

        public bool RetirerAffiche()
        {
            if (Projections.Count > 0)
            {
                foreach (Projection projection in Projections)
                {
                    if (Projections[Projections.Count - 1] == projection)
                    {
                        DesactiverDerniereProjection();
                    }
                    else if (projection.EstActive)
                    {
                        projection.EstActive = false;
                    }
                }
                return true;
            }
            return false;
        }

        private void DesactiverDerniereProjection()
        {
            Projection derniereProjection = Projections[Projections.Count - 1];
            if (derniereProjection.EstActive)
            {
                derniereProjection.EstActive = false;
                DatesFinsAffiche.Add(derniereProjection.DateFin < DateTime.Now
                    ? derniereProjection.DateFin
                    : DateTime.Now);
            }
        }

        public bool AjouterProjection(DateTime pDateDebut, DateTime pDateFin, Salle pSalle)
        {
            if (DateSortie > pDateDebut)
            {
                throw new ArgumentOutOfRangeException("pDateDebut",
                    "La date de début de la projection doit être supérieure à la date de sortie internationnale du film.");
            }
            if (pDateDebut <= DateTime.Now)
            {
                throw new ArgumentOutOfRangeException("pDateDebut",
                    "La date de début de la projection doit être supérieure à la date actuelle.");
            }
            if (Projections.Count > 0)
            {
                Projection derniereProjection = Projections[Projections.Count - 1];
                bool estMemeAnnee = pDateDebut.Year == derniereProjection.DateFin.Year;
                if (derniereProjection.DateFin > pDateDebut)
                {
                    throw new ArgumentOutOfRangeException("pDateDebut",
                        "La date de début de la projection à ajouter doit être supérieure à la date de fin de la dernière projection du film.");
                }
                else if (estMemeAnnee && Film.NB_MAX_EST_AFFICHE_PAR_ANNEE < DatesFinsAffiche.Count)
                {
                    int iteration = ObtenirNbProjectionsPourAnneeCourante(pDateDebut.Year);
                   
                    if (iteration == Film.NB_MAX_EST_AFFICHE_PAR_ANNEE)
                    {
                        throw new InvalidOperationException(
                            $"Il est impossible d'ajouter une autre projection puisque celle-ci a déjà été projetée {Film.NB_MAX_EST_AFFICHE_PAR_ANNEE}");
                    }
                }
            }
            Projections.Add(new Projection(pDateDebut, pDateFin, pSalle));
            return true;
        }

        public int ObtenirNbProjectionsPourAnneeCourante(int annneeCourante)
        {
            if (Projections.Count > 0)
            {
                Projection derniereProjection = Projections[Projections.Count - 1];
                bool estMemeAnnee = annneeCourante == derniereProjection.DateFin.Year;
                int iteration = 0;
                if (estMemeAnnee)
                {
                    iteration++;
                    while (iteration < Film.NB_MAX_EST_AFFICHE_PAR_ANNEE - 1 && DatesFinsAffiche[DatesFinsAffiche.Count - 1 - iteration].Year ==
                         DatesFinsAffiche[DatesFinsAffiche.Count - 2 - iteration].Year)
                    {
                        iteration++;
                    }
                }
                return iteration;
            }
            return -1;
        }

        #region Overrides of Object

        public override string ToString()
        {
            return Nom;
        }

        #endregion

        #endregion
    }
}