using Dotbot.Common.SeedWork;

namespace Dotbot.Common.CommandHandlers.Moderator;

public class ModeratorCommandType: Enumeration
{
    //TODO: I don't think this is the best way to do multiple command types and should revisted, maybe adding something onto contexts or something 
    public static ModeratorCommandType Default = new(0, nameof(Default));
    public static ModeratorCommandType AddModerator = new(1, nameof(AddModerator));
    public static ModeratorCommandType RemoveModerator = new(2, nameof(RemoveModerator));
    public static ModeratorCommandType SetXkcdChannel = new(3, nameof(SetXkcdChannel));

    public ModeratorCommandType(int id, string name) : base(id, name)
    {
        
    }
    
    public static ModeratorCommandType FromDisplayName(string displayName)
    {
        var matchingItem = GetAll<ModeratorCommandType>().FirstOrDefault(item => item.Name.Equals(displayName, StringComparison.InvariantCultureIgnoreCase));
        return matchingItem ?? Default;
    }
}
