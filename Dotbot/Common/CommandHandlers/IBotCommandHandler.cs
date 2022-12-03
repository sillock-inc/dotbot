using FluentResults;

namespace Dotbot.Common.CommandHandlers;

public interface IBotCommandHandler
{
    bool Match(string? s);
    CommandType CommandType { get; }
    Task<Result> HandleAsync(string content, IServiceContext context);
}