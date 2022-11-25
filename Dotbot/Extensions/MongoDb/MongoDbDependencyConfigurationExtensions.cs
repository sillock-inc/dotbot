using System.Diagnostics;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Dotbot.Extensions.MongoDb;

public static class MongoDbDependencyConfigurationExtensions
{
    public static IServiceCollection AddMongoDbCollection<T, TM>(this IServiceCollection services, string collectionName, TM classMapExtension)
        where T : class
        where TM : IClassMapExtension
    {
        IMongoCollection<T> MongoDbCollectionFactory(IServiceProvider provider)
        {
            try
            {
                if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
                {
                    classMapExtension.Register();
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
            var database = provider.GetRequiredService<IMongoDatabase>();

            return database.GetCollection<T>(collectionName);
        }

        services.TryAddSingleton(MongoDbCollectionFactory);

        return services;
    }
}