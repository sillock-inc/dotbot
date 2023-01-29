using FluentResults;
using static FluentResults.Result;

namespace Dotbot.Common.CommandHandlers.Moderator;

public class DefaultModeratorBotCommandHandler: IBotModeratorCommandHandler
{
    public ModeratorCommandType CommandType  => ModeratorCommandType.Default;
    public async Task<Result> HandleAsync(string content, IServiceContext context)
    {
        var messageSplit = content.Split(' ');
        var key = messageSplit[0];
        
        await context.SendMessageAsync($"No command {key} found");
        return Fail($"No command {key} found");
    }
}