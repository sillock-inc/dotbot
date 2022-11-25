using Dotbot.Common.Models;
using Dotbot.Common.Services;

namespace Dotbot.Common.CommandHandlers;

public class DefaultBotCommandHandler : IBotCommandHandler
{

    private readonly IBotCommandService _botCommandService;

    public DefaultBotCommandHandler(IBotCommandService botCommandService)
    {
        _botCommandService = botCommandService;
    }

    public bool Match(string? s) => s == null;

    public async Task<bool> HandleAsync(string content, IServiceContext context)
    {
        var messageSplit = content.Split(' ');

        var key = messageSplit[0];
        var command = await _botCommandService.GetCommand(await context.GetServerId(), key);

        if (command != null)
        {
            switch (command.Type)
            {
                case BotCommand.CommandType.STRING:
                    await HandleString(command, context);
                    break;
                case BotCommand.CommandType.FILE:
                    await HandleFile(command, context);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
        else
        {
            await context.SendMessageAsync($"No command {key} found");
        }

        return true;
    }

    private async Task HandleFile(BotCommand command, IServiceContext context)
    {
        var fileStream = await _botCommandService.GetCommandFileStream(command);
        if (fileStream == null)
        {
            await context.SendMessageAsync($"Cannot find file content for {command.Key}");
            return;
        }

        await context.SendFileAsync(command.FileName, fileStream);
    }

    private async Task HandleString(BotCommand command, IServiceContext context)
    {
        await context.SendMessageAsync(command.Content);
    }
    
}