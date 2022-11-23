using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dotbot.Discord.Models;

[BsonIgnoreExtraElements]
public class DiscordCommand
{
    public enum CommandType
    {
        STRING,
        FILE
    }
    
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    
    [BsonElement("guildId")] 
    public string ServiceId { get; set; }
    
    [BsonElement("key")] 
    public string Key { get; set; }
    
    [BsonElement("content")] 
    public string? Content { get; set; }
    
    [BsonElement("fileName")] 
    public string? FileName { get; set; }
    
    [BsonElement("type")] 
    public CommandType Type { get; set; }
    
}