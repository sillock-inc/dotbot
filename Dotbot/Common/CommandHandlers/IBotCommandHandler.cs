using FluentResults;

namespace Dotbot.Common.CommandHandlers;

public interface IBotCommandHandler
{
    CommandType CommandType { get; }
    Task<Result> HandleAsync(string content, IServiceContext context);
}