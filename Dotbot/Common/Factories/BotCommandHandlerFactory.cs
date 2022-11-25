using Dotbot.Common.CommandHandlers;

namespace Dotbot.Common.Factories;

public class BotCommandHandlerFactory : IBotCommandHandlerFactory
{
    private readonly IBotCommandHandler _defaultBotCommandHandler;
    private readonly IList<IBotCommandHandler> _handlers;

    public BotCommandHandlerFactory(IEnumerable<IBotCommandHandler> handlers)
    {
        _handlers = handlers.ToList();
        _defaultBotCommandHandler = _handlers.First(x => x.Match(null));
    }

    public IBotCommandHandler GetCommand(string str)
    {
        var cmd = _handlers.FirstOrDefault(x => x.Match(str));
        return cmd ?? _defaultBotCommandHandler;
    }
}