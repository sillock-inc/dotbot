using FluentResults;
using static FluentResults.Result;

namespace Dotbot.Common.CommandHandlers;

public class PingBotCommandHandler: IBotCommandHandler
{
    public CommandType CommandType => CommandType.Ping;
    public async Task<Result> HandleAsync(string content, IServiceContext context)
    {
        await context.ReplyAsync("pong");
        return Ok();
    }
}