using Microsoft.Extensions.DependencyInjection.Extensions;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace Dotbot.Extensions.MongoDb;

public static class MongoDbDependencyConfigurationExtensions
{
    public static IServiceCollection AddMongoDbCollection<T, TM>(this IServiceCollection services, TM classMapExtension)
        where T : class
        where TM : IClassMapExtension
    {
        IMongoCollection<T> MongoDbCollectionFactory(IServiceProvider provider)
        {
            if (!BsonClassMap.IsClassMapRegistered(typeof(T)))
            {
                BsonClassMap.RegisterClassMap(classMapExtension.Register());
            }
            
            var database = provider.GetRequiredService<IMongoDatabase>();

            return database.GetCollection<T>(typeof(T).Name);
        }

        services.TryAddSingleton(MongoDbCollectionFactory);

        return services;
    }
}