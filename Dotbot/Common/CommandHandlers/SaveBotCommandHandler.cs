using Dotbot.Common.Services;

namespace Dotbot.Common.CommandHandlers;

public class SaveBotCommandHandler: IBotCommandHandler
{
    private readonly IBotCommandService _botCommandService;

    public SaveBotCommandHandler(IBotCommandService botCommandService)
    {
        _botCommandService = botCommandService;
    }

    public bool Match(string? s) => s == "save";

    public async Task<bool> HandleAsync(string content, IServiceContext context)
    {
        var split = content.Split(' ');
        var key = split[1];
        if (await context.HasAttachments())
        {
            var attachments = (await context.GetAttachments()).First();

            var httpClient = new HttpClient();
            var fileStream = await httpClient.GetStreamAsync(attachments.Url);

            await _botCommandService.SaveCommand(await context.GetServerId(), key, attachments.Filename, fileStream,
                true);
        }
        else
        {
            if (split.Length < 3)
            {
                await context.SendMessageAsync("No content given");
                return false;
            }

            var commandContent = string.Join(" ", split[2..]);
            await _botCommandService.SaveCommand(await context.GetServerId(), key, commandContent, true);
        }

        await context.SendMessageAsync($"Saved command as {key}");
        
        return true;
    }
}