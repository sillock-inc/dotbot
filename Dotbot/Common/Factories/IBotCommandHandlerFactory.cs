using Dotbot.Common.CommandHandlers;

namespace Dotbot.Common.Factories;

public interface IBotCommandHandlerFactory
{
    IBotCommandHandler GetCommand(string str);
}