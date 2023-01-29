using Dotbot.Common.CommandHandlers.Moderator;

namespace Dotbot.Common.Factories;

public interface IBotModeratorCommandHandlerFactory
{
    IBotModeratorCommandHandler GetCommand(string str);
}