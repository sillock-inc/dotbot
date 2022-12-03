using Dotbot.Infrastructure.Repositories;
using Dotbot.Infrastructure.Services;
using FluentResults;
using static FluentResults.Result;

namespace Dotbot.Common.CommandHandlers;

public class SaveBotCommandHandler: IBotCommandHandler
{
    private readonly IBotCommandRepository _botCommandRepository;
    private readonly HttpClient _httpClient;
    private readonly IFileService _fileService;

    public SaveBotCommandHandler(IBotCommandRepository botCommandRepository, HttpClient httpClient, IFileService fileService)
    {
        _botCommandRepository = botCommandRepository;
        _httpClient = httpClient;
        _fileService = fileService;
    }

    public bool Match(string? s) => s == "save";

    public async Task<Result> HandleAsync(string content, IServiceContext context)
    {
        var split = content.Split(' ');
        var key = split[1];
        if (await context.HasAttachments())
        {
            var attachments = (await context.GetAttachments()).First();
            
            var fileStream = await _httpClient.GetStreamAsync(attachments.Url);

            var serverId = await context.GetServerId();
            
            var fileUploadResult = await _fileService.SaveFile($"{serverId}:{attachments.Filename}:{key}", fileStream);
            if (fileUploadResult.IsFailed) return Fail(fileUploadResult.Errors);
            
            var result = await _botCommandRepository.SaveCommand(serverId, key, attachments.Filename, fileStream,
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
            var result = await _botCommandRepository.SaveCommand(await context.GetServerId(), key, commandContent, true);
            if (result.IsFailed)
            {
                await context.SendMessageAsync($"Failed to save command:");
            };
        }

        await context.SendMessageAsync($"Saved command as {key}");
        
        return Ok();
    }
}