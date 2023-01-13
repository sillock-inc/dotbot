using Dotbot.Common.SeedWork;

namespace Dotbot.Common.CommandHandlers;

public class CommandType : Enumeration
{
    public static CommandType Default = new(0, nameof(Default));
    public static CommandType Save = new(1, nameof(Save));
    public static CommandType Ping = new(2, nameof(Ping));
    public static CommandType Saved = new(3, nameof(Saved));
    public static CommandType Avatar = new(4, nameof(Avatar));
    public static CommandType Xkcd = new(5, nameof(Xkcd));
    public CommandType(int id, string name) : base(id, name)
    {
        
    }
    
    public static CommandType FromDisplayName(string displayName)
    {
        var matchingItem = GetAll<CommandType>().FirstOrDefault(item => item.Name.Equals(displayName, StringComparison.InvariantCultureIgnoreCase));
        return matchingItem ?? Default;
    }
}