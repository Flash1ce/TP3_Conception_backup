#region MÉTADONNÉES

// Nom du fichier : CategorieTests.cs
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
    public class CategorieTests
    {
        #region MÉTHODES

        private List<Categorie> GenerationCategories()
        {
            List<Categorie> categories = new List<Categorie>
            {
                new Categorie(ObjectId.GenerateNewId(), "Horreur"),
                new Categorie(ObjectId.GenerateNewId(), "Fantastique"),
                new Categorie(ObjectId.GenerateNewId(), "Comédie"),
                new Categorie(ObjectId.GenerateNewId(), "Action"),
                new Categorie(ObjectId.GenerateNewId(), "Romance")
            };

            List<ObjectId> categoriesId = new List<ObjectId>();
            categories
                .GetRange(0, 3)
                .ForEach(x => categoriesId.Add(x.Id));

            return categories;
        }

        [Fact]
        public void ObtenirToutLesCategories()
        {
            //List<Categorie> categories = GenerationCategories();
            //var categorieMock = new Mock<ICRUD<Categorie>>();
            //categorieMock.Setup(x => x.ObtenirTout()).Returns(categories);
            //var categoriesMock = categorieMock.Object.ObtenirTout();
            //Assert.Equal(categoriesMock, categories);
        }

        [Fact]
        public void ObtenirPlusieursRetourneCategoriesSelonFiltre()
        {
            //List<Categorie> categories = GenerationCategories();
            //var categorieMock = new Mock<ICRUD<Categorie>>();
            //categorieMock.Setup(x => x.ObtenirPlusieurs(x => x.Nom, new List<string> { "Horreur" })).Returns(new List<Categorie> { categories[0] });
            //var categoriesMock = categorieMock.Object.ObtenirPlusieurs(x => x.Nom, new List<string> { "Horreur" });
            //Assert.Equal(categoriesMock, new List<Categorie> { categories[0] });
        }

        [Fact]
        public void InsererPlusieursCategoriesRetourneTrue()
        {
            //List<Categorie> categories = GenerationCategories();
            //var categorieMock = new Mock<ICRUD<Categorie>>();
            //categorieMock.Setup(x => x.InsererPlusieurs(categories)).Returns(true);
            //var categoriesMock = categorieMock.Object.InsererPlusieurs(categories);
            //Assert.True(categoriesMock);
        }

        #endregion
    }
}