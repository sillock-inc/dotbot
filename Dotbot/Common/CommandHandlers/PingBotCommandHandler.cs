namespace Dotbot.Common.CommandHandlers;

public class PingBotCommandHandler: IBotCommandHandler
{
    public bool Match(string? s) => s == "ping";
    public async Task<bool> HandleAsync(string content, IServiceContext context)
    {
        await context.ReplyAsync("pong");
        return true;
    }
}