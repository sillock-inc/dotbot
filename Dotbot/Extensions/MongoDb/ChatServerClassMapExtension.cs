using Dotbot.Common.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;
using MongoDB.Bson.Serialization.Serializers;

namespace Dotbot.Extensions.MongoDb;

public class ChatServerClassMapExtension : IClassMapExtension
{
    public BsonClassMap Register()
    {
        return BsonClassMap.RegisterClassMap<ChatServer>(cm =>
        {
            cm.AutoMap();
            cm.SetIgnoreExtraElements(true);
            
            cm.MapIdProperty(m => m.Id)
                .SetIdGenerator(StringObjectIdGenerator.Instance)
                .SetSerializer(new StringSerializer(BsonType.ObjectId));
            cm.MapMember(m => m.ServiceId)
                .SetElementName("guildId");
            cm.MapMember(m => m.UserWordCounts)
                .SetElementName("userWordCounts");
            cm.MapMember(m => m.ModeratorIds)
                .SetElementName("privilegedUsers");
            cm.MapMember(m => m.MemeChannelIds)
                .SetElementName("memeChannels");
            cm.MapMember(m => m.DeafenedChannelIds)
                .SetElementName("deafenedChannels");
            cm.MapMember(m => m.XkcdChannelId)
                .SetElementName("xkcdChannelId");
            cm.MapMember(m => m.Volume)
                .SetElementName("volume");
        });
    }
}