using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Dotbot.Discord.Models;

public class DiscordCommand
{
    public enum CommandType
    {
        STRING,
        FILE
    }
    
    public string? Id { get; set; }
    
    public string ServiceId { get; set; }
    public string Key { get; set; }

    public string? Content { get; set; }

    public string? FileName { get; set; }
    
    public CommandType Type { get; set; }
    
}