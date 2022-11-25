using Dotbot.Common.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Dotbot.Extensions.MongoDb;

public class DiscordCommandClassMapExtension : IClassMapExtension
{
    public BsonClassMap Register()
    {
        return BsonClassMap.RegisterClassMap<BotCommand>(cm =>
        {
            cm.SetIgnoreExtraElements(true);
            cm.MapIdProperty(m => m.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
            cm.MapMember(m => m.ServiceId)
                .SetElementName("guildId");
            cm.MapMember(m => m.Key)
                .SetElementName("key");
            cm.MapMember(m => m.Content)
                .SetElementName("content");
            cm.MapMember(m => m.FileName)
                .SetElementName("fileName");
            cm.MapMember(m => m.Type)
                .SetElementName("type");
        });
    }
}