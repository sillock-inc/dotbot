namespace Dotbot.Common.CommandHandlers;
public interface IBotCommandHandler
{
    bool Match(string? s);

    Task<bool> HandleAsync(string content, IServiceContext context);
}