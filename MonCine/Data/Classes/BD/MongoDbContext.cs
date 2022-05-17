#region MÉTADONNÉES

// Nom du fichier : MongoDbContext.cs
// Date de modification : 2022-05-12

#endregion

#region USING

using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

#endregion

namespace MonCine.Data.Classes.BD
{
    internal class MongoDbContext
    {
        #region MÉTHODES

        public static IMongoCollection<TDocument> ObtenirCollection<TDocument>(IMongoDatabase pBd) =>
            pBd.GetCollection<TDocument>($"{typeof(TDocument).Name}s");

        public static List<TDocument> ObtenirCollectionListe<TDocument>(IMongoDatabase pBd) =>
            MongoDbContext.ObtenirCollection<TDocument>(pBd)
                .FindSync<TDocument>(Builders<TDocument>.Filter.Empty).ToList();

        public static List<TDocument> ObtenirDocumentsFiltres<TDocument, TField>(IMongoDatabase pBd,
            Expression<Func<TDocument, TField>> pField,
            List<TField> pObjects)
        {
            try
            {
                return MongoDbContext.ObtenirCollection<TDocument>(pBd)
                    .Find(Builders<TDocument>.Filter.In(pField, pObjects)).ToList();
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : ObtenirDocumentsFiltres - Exception : {e.Message}");
            }
        }

        public static void InsererUnDocument<TDocument>(IMongoDatabase pBd, TDocument pDocument)
        {
            try
            {
                MongoDbContext.ObtenirCollection<TDocument>(pBd).InsertOne(pDocument);
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : InsererUnDocument - Exception : {e.Message}");
            }
        }

        public static bool InsererPlusieursDocuments<TDocument>(IMongoDatabase pBd, List<TDocument> pDocuments)
        {
            try
            {
                MongoDbContext.ObtenirCollection<TDocument>(pBd).InsertMany(pDocuments);
                return true;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : InsererPlusieursDocuments - Exception : {e.Message}");
            }
        }

        public static bool MAJUnDocument<TDocument, TField>(IMongoDatabase pBd,
            Expression<Func<TDocument, bool>> pFiltre,
            List<(Expression<Func<TDocument, TField>> field, TField value)> pMajDefinitions)
        {
            try
            {
                var builder = Builders<TDocument>.Update;
                UpdateDefinition<TDocument> majDefinition = null;

                foreach ((Expression<Func<TDocument, TField>> field, TField value) in pMajDefinitions)
                {
                    majDefinition = majDefinition == null
                        ? builder.Set(field, value)
                        : majDefinition.Set(field, value);
                }

                return MongoDbContext.ObtenirCollection<TDocument>(pBd).UpdateOne(pFiltre, majDefinition)
                    .IsAcknowledged;
            }
            catch (Exception e)
            {
                throw new ExceptionBD($"Méthode : MAJUnDocument - MongoDbContext.cs - Exception : {e.Message}");
            }
        }

        #endregion
    }
}