using Dotbot.Common.Services;
using FluentResults;
using static FluentResults.Result;

namespace Dotbot.Common.CommandHandlers;

public class SaveBotCommandHandler: IBotCommandHandler
{
    private readonly IBotCommandService _botCommandService;

    public SaveBotCommandHandler(IBotCommandService botCommandService)
    {
        _botCommandService = botCommandService;
    }

    public bool Match(string? s) => s == "save";

    public async Task<Result> HandleAsync(string content, IServiceContext context)
    {
        var split = content.Split(' ');
        var key = split[1];
        if (await context.HasAttachments())
        {
            var attachments = (await context.GetAttachments()).First();

            var httpClient = new HttpClient();
            var fileStream = await httpClient.GetStreamAsync(attachments.Url);

            var result = await _botCommandService.SaveCommand(await context.GetServerId(), key, attachments.Filename, fileStream,
                true);
            if (result.IsFailed)
            {
                await context.SendMessageAsync("Failed to save command");
            }
        }
        else
        {
            if (split.Length < 3)
            {
                await context.SendMessageAsync("No content given");
                return Fail("No content given");
            }

            var commandContent = string.Join(" ", split[2..]);
            var result = await _botCommandService.SaveCommand(await context.GetServerId(), key, commandContent, true);
            if (result.IsFailed)
            {
                await context.SendMessageAsync($"Failed to save command:");
            };
        }

        await context.SendMessageAsync($"Saved command as {key}");
        
        return Ok();
    }
}