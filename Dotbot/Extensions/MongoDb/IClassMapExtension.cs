using MongoDB.Bson.Serialization;

namespace Dotbot.Extensions.MongoDb;

public interface IClassMapExtension
{
    public BsonClassMap Register();
}