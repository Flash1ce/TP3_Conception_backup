#region MÉTADONNÉES

// Nom du fichier : Preference.cs
// Date de création : 2022-04-20
// Date de modification : 2022-04-21

#endregion

#region USING

using System;
using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

#endregion

namespace MonCine.Data.Classes
{
    public class Preference
    {
        #region CONSTANTES ET ATTRIBUTS STATIQUES

        /// <summary>
        /// Nombre maximum de catégories pouvant être sélectionnées
        /// </summary>
        public const int NB_MAX_CATEGORIES_PREF = 3;

        /// <summary>
        /// Nombre maximum d'acteurs pouvant être sélectionnés
        /// </summary>
        public const int NB_MAX_ACTEURS_PREF = 5;

        /// <summary>
        /// Nombre maximum de réalisateurs pouvant être sélectionnés
        /// </summary>
        public const int NB_MAX_REALISATEURS_PREF = 5;

        #endregion

        #region ATTRIBUTS

        private List<ObjectId> _categoriesId;
        private List<ObjectId> _acteursId;
        private List<ObjectId> _realisateursId;

        #endregion

        #region PROPRIÉTÉS ET INDEXEURS
        public List<ObjectId> CategoriesId
        {
            get { return _categoriesId; }
            set
            {
                if (value.Count > Preference.NB_MAX_CATEGORIES_PREF)
                {
                    throw new IndexOutOfRangeException(
                        $"Le nombre de catégories préférées est supérieur à {Preference.NB_MAX_CATEGORIES_PREF}");
                }

                _categoriesId = value;
            }
        }

        public List<ObjectId> ActeursId
        {
            get { return _acteursId; }
            set
            {
                if (value.Count > Preference.NB_MAX_ACTEURS_PREF)
                {
                    throw new IndexOutOfRangeException(
                        $"Le nombre d'acteurs préférés est supérieur à {Preference.NB_MAX_ACTEURS_PREF}");
                }

                _acteursId = value;
            }
        }

        public List<ObjectId> RealisateursId
        {
            get { return _realisateursId; }
            set
            {
                if (value.Count > Preference.NB_MAX_REALISATEURS_PREF)
                {
                    throw new IndexOutOfRangeException(
                        $"Le nombre de réalisateurs préférés est supérieur à {Preference.NB_MAX_REALISATEURS_PREF}");
                }

                _realisateursId = value;
            }
        }

        [BsonIgnore]
        public List<Categorie> Categories { get; set; }

        [BsonIgnore]
        public List<Acteur> Acteurs { get; set; }

        [BsonIgnore]
        public List<Realisateur> Realisateurs { get; set; }

        #endregion

        #region CONSTRUCTEURS

        public Preference(List<ObjectId> pCategoriesId, List<ObjectId> pActeursId, List<ObjectId> pRealisateursId)
        {
            CategoriesId = pCategoriesId;
            ActeursId = pActeursId;
            RealisateursId = pRealisateursId;
        }

        #endregion

        #region MÉTHODES

        #region Overrides of Object

        public override string ToString()
        {
            string sautDeLigne = "\n";
            string aucun = "Aucune";
            string strFormate = sautDeLigne + "Catégories: ";
            int nbIterations = 0;

            if (Categories.Count > 0)
            {
                foreach (Categorie categorie in Categories)
                {
                    if (Categories.IndexOf(categorie) == Categories.Count - 1)
                    {
                        strFormate += categorie.Nom;
                    }
                    else
                    {
                        strFormate += categorie.Nom + ", ";
                    }
                    nbIterations++;
                }
            }
            else
            {
                strFormate += aucun;
            }

            strFormate += sautDeLigne + "Acteurs: ";

            if (Acteurs.Count > 0)
            {
                foreach (Acteur acteur in Acteurs)
                {
                    if (Acteurs.IndexOf(acteur) == Acteurs.Count - 1)
                    {
                        strFormate += acteur.Nom;
                    }
                    else
                    {
                        strFormate += acteur.Nom + ", ";
                    }
                }
            }
            else
            {
                strFormate += aucun;
            }

            strFormate += sautDeLigne + "Réalisateurs: ";

            if (Realisateurs.Count > 0)
            {
                foreach (Realisateur realisateur in Realisateurs)
                {
                    if (Realisateurs.IndexOf(realisateur) == Realisateurs.Count - 1)
                    {
                        strFormate += realisateur.Nom;
                    }
                    else
                    {
                        strFormate += realisateur.Nom + ", ";
                    }
                }
            }
            else
            {
                strFormate += aucun;
            }
            return strFormate;
        }

        #endregion

        #endregion
    }
}