using Dotbot.Common.Models;
using Dotbot.Common.Services;
using FluentResults;
using static FluentResults.Result;

namespace Dotbot.Common.CommandHandlers;

public class DefaultBotCommandHandler : IBotCommandHandler
{

    private readonly IBotCommandService _botCommandService;

    public DefaultBotCommandHandler(IBotCommandService botCommandService)
    {
        _botCommandService = botCommandService;
    }

    public bool Match(string? s) => s == null;

    public async Task<Result> HandleAsync(string content, IServiceContext context)
    {
        var messageSplit = content.Split(' ');

        var key = messageSplit[0];
        var command = await _botCommandService.GetCommand(await context.GetServerId(), key);

        if (command.IsSuccess)
        {
            return command.Value.Type switch
            {
                BotCommand.CommandType.STRING => await HandleString(command.Value, context),
                BotCommand.CommandType.FILE => await HandleFile(command.Value, context),
                _ => throw new ArgumentOutOfRangeException()
            };
        }

        await context.SendMessageAsync($"No command {key} found");

        return Fail($"No command {key} found");
    }

    private async Task<Result> HandleFile(BotCommand command, IServiceContext context)
    {
        var fileStream = await _botCommandService.GetCommandFileStream(command);
        if (fileStream.IsFailed)
        {
            await context.SendMessageAsync($"Cannot find file content for {command.Key}");
            return Fail($"Cannot find file content for {command.Key}");
        }

        await context.SendFileAsync(command.FileName, fileStream.Value);
        return Ok();
    }

    private static async Task<Result> HandleString(BotCommand command, IServiceContext context)
    {
        await context.SendMessageAsync(command.Content);
        return Ok();
    }
    
}