using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dotbot.Discord.Models;

[BsonIgnoreExtraElements]
public class ChatServer
{
    //TODO: Need to write a migration at some point to rename some of these fields 
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("guildId")] 
    public string ServiceId { get; set; }
    
    [BsonElement("userWordCounts")]
    public Dictionary<string, int> UserWordCounts { get; set; } = new();
    
    [BsonElement("privilegedUsers")]
    public List<string> ModeratorIds { get; set; } = new();
    
    [BsonElement("memeChannels")]
    public List<string> MemeChannelIds { get; set; } = new();
    
    [BsonElement("deafenedChannels")]
    public List<string> DeafenedChannelIds { get; set; } = new();

    [BsonElement("xkcdChannelId")]
    public string? XkcdChannelId { get; set; }

    [BsonElement("volume")]
    public int Volume { get; set; } = 50;
    
    public ChatServer(string serviceId)
    {
        ServiceId = serviceId;
    }
}