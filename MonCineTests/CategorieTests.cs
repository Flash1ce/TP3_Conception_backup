#region MÉTADONNÉES

// Nom du fichier : CategorieTests.cs
// Date de modification : 2022-05-17

#endregion

#region USING

using System.Collections.Generic;
using MonCine.Data.Classes;
using MonCine.Data.Classes.DAL;
using MongoDB.Bson;
using MongoDB.Driver;
using Moq;
using System.Threading;
using Xunit;

#endregion

namespace MonCineTests
{
    public class CategorieTests
    {
        private readonly Mock<IMongoClient> _mongoClientCategorie;
        private readonly Mock<IMongoDatabase> _mongodbCategorie;
        private readonly Mock<IMongoCollection<Categorie>> _categorieCollection;
        private readonly Mock<IAsyncCursor<Categorie>> _categorieCurseur;
        private readonly List<Categorie> _categories;

        public CategorieTests()
        {
            _mongoClientCategorie = new Mock<IMongoClient>();
            _mongodbCategorie = new Mock<IMongoDatabase>();
            _categorieCollection = new Mock<IMongoCollection<Categorie>>();
            _categorieCurseur = new Mock<IAsyncCursor<Categorie>>();

            _categories = new List<Categorie>
            {
                new Categorie(ObjectId.GenerateNewId(), "Horreur"),
                new Categorie(ObjectId.GenerateNewId(), "Fantastique"),
                new Categorie(ObjectId.GenerateNewId(), "Comédie"),
                new Categorie(ObjectId.GenerateNewId(), "Action"),
                new Categorie(ObjectId.GenerateNewId(), "Romance")
            };
        }


        private void InitializeMongoDb()
        {
            _mongoClientCategorie.Setup(x => x.GetDatabase(It.IsAny<string>(), default))
                .Returns(_mongodbCategorie.Object);
            _mongodbCategorie.Setup(x => x.GetCollection<Categorie>($"{typeof(Categorie).Name}s", default))
                .Returns(_categorieCollection.Object);
        }

        private void InitializeMongoCollection()
        {
            _categorieCurseur.Setup(x => x.Current).Returns(_categories);
            _categorieCurseur.SetupSequence(x => x.MoveNext(It.IsAny<CancellationToken>())).Returns(true)
                .Returns(false);
            _categorieCollection
                .Setup(
                    x => x.FindSync(Builders<Categorie>.Filter.Empty,
                        It.IsAny<FindOptions<Categorie>>(), default)
                )
                .Returns(_categorieCurseur.Object);
            
            InitializeMongoDb();
        }

        #region MÉTHODES

        [Fact]
        public void ObtenirToutLesCategories()
        {
            // Création des faux objets
            InitializeMongoCollection();

            // Arrange
            DALCategorie dalCategorie = new DALCategorie(_mongoClientCategorie.Object);

            // Act et Assert
            Assert.Equal(_categories, dalCategorie.ObtenirTout());
        }

        [Fact]
        public void ObtenirPlusieursRetourneCategoriesSelonFiltre()
        {
            // Création des faux objets
            InitializeMongoCollection();

            // Arrange
            DALCategorie dalCategorie = new DALCategorie(_mongoClientCategorie.Object);

            // Act et Assert
            Assert.Equal(_categories[0], dalCategorie.ObtenirUn(_categories[0].Id));
        }

        [Fact]
        public void InsererPlusieursCategoriesRetourneTrue()
        {
            // Création des faux objets
            InitializeMongoCollection();

            // Arrange
            DALCategorie dalCategorie = new DALCategorie(_mongoClientCategorie.Object);

            // Act et Assert
            Assert.True(dalCategorie.InsererPlusieurs(new List<Categorie> { new Categorie(ObjectId.GenerateNewId(), "A") }));
        }

        #endregion
    }
}