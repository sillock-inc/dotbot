using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dotbot.Discord.Models;

public class ChatServer
{
    public string? Id { get; set; }
    
    public string ServiceId { get; set; }
    
    public Dictionary<string, int> UserWordCounts { get; set; } = new();
    
    public List<string> ModeratorIds { get; set; } = new();
    
    public List<string> MemeChannelIds { get; set; } = new();
    
    public List<string> DeafenedChannelIds { get; set; } = new();
    
    public string? XkcdChannelId { get; set; }
    
    public int Volume { get; set; } = 50;
    
    public ChatServer(string serviceId)
    {
        ServiceId = serviceId;
    }
}