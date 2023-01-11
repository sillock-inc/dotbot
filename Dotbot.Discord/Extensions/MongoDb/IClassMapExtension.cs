using MongoDB.Bson.Serialization;

namespace Dotbot.Discord.Extensions.MongoDb;

public interface IClassMapExtension
{
    public BsonClassMap Register();
}