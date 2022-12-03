using Dotbot.SeedWork;

namespace Dotbot.Common.CommandHandlers;

public class CommandType : Enumeration
{
    public static CommandType Default = new(0, nameof(Default));
    public static CommandType Save = new(1, nameof(Save));
    public static CommandType Ping = new(2, nameof(Ping));
    public CommandType(int id, string name) : base(id, name)
    {
        
    }
    
    public static CommandType FromDisplayName(string displayName)
    {
        var matchingItem = GetAll<CommandType>().FirstOrDefault(item => item.Name.Equals(displayName, StringComparison.InvariantCultureIgnoreCase));
        return matchingItem ?? Default;
    }
}