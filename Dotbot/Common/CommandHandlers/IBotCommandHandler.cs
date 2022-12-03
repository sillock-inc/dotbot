using FluentResults;

namespace Dotbot.Common.CommandHandlers;

public interface IBotCommandHandler
{
    bool Match(string? s);

    Task<Result> HandleAsync(string content, IServiceContext context);
}