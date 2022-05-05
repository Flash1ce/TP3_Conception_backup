using MonCine.Data.Classes;
using MonCine.Data.Interfaces;
using MongoDB.Bson;
using Moq;
using System;
using System.Collections.Generic;
using Xunit;

namespace MonCineTests
{
    public class CategorieTests
    {
        private List<Categorie> GenerationCategories()
        {
            List<Categorie> categories = new List<Categorie>
                {
                    new(new ObjectId(), "Horreur"),
                    new(new ObjectId(), "Fantastique"),
                    new(new ObjectId(), "Comédie"),
                    new(new ObjectId(), "Action"),
                    new(new ObjectId(), "Romance")
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
            List<Categorie> categories = GenerationCategories();
            var categorieMock = new Mock<ICRUD<Categorie>>();
            categorieMock.Setup(x => x.ObtenirTout()).Returns(categories);
            var categoriesMock = categorieMock.Object.ObtenirTout();
            Assert.Equal(categoriesMock, categories);
        }

        [Fact]
        public void ObtenirPlusieursRetourneCategoriesSelonFiltre()
        {
            List<Categorie> categories = GenerationCategories();
            var categorieMock = new Mock<ICRUD<Categorie>>();
            categorieMock.Setup(x => x.ObtenirPlusieurs(x => x.Nom, new List<string> { "Horreur" })).Returns(new List<Categorie> { categories[0] });
            var categoriesMock = categorieMock.Object.ObtenirPlusieurs(x => x.Nom, new List<string> { "Horreur" });
            Assert.Equal(categoriesMock, new List<Categorie> { categories[0] });
        }

        [Fact]
        public void InsererPlusieursCategoriesRetourneTrue()
        {
            List<Categorie> categories = GenerationCategories();
            var categorieMock = new Mock<ICRUD<Categorie>>();
            categorieMock.Setup(x => x.InsererPlusieurs(categories)).Returns(true);
            var categoriesMock = categorieMock.Object.InsererPlusieurs(categories);
            Assert.True(categoriesMock);
        }
    }
}