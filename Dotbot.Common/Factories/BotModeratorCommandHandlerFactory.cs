using Dotbot.Common.CommandHandlers;
using Dotbot.Common.CommandHandlers.Moderator;

namespace Dotbot.Common.Factories;

public class BotModeratorCommandHandlerFactory : IBotModeratorCommandHandlerFactory
{
    private readonly IList<IBotModeratorCommandHandler> _handlers;

    public BotModeratorCommandHandlerFactory(IEnumerable<IBotModeratorCommandHandler> handlers)
    {
        _handlers = handlers.ToList();
    }

    public IBotModeratorCommandHandler GetCommand(string str)
    {
        var commandType = ModeratorCommandType.FromDisplayName(str);
        return _handlers.First(x => commandType.Equals(x.CommandType));
    }
}