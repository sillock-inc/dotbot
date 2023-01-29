using FluentResults;

namespace Dotbot.Common.CommandHandlers.Moderator;

public interface IBotModeratorCommandHandler
{
    ModeratorCommandType CommandType { get; }
    Task<Result> HandleAsync(string content, IServiceContext context);
}