using Discord;
using Dotbot.Infrastructure.Entities;
using Dotbot.Infrastructure.Repositories;
using FluentResults;

namespace Dotbot.Common.CommandHandlers;

public class SavedCommandHandler: IBotCommandHandler
{
    private const int MaxPageSize = 20;
    private readonly IBotCommandRepository _botCommandRepository;

    public SavedCommandHandler(IBotCommandRepository botCommandRepository)
    {
        _botCommandRepository = botCommandRepository;
    }

    public CommandType CommandType => CommandType.Saved;
    public async Task<Result> HandleAsync(string content, IServiceContext context)
    {
        var split = content.Split(' ');
        var page = 1;

        if (split.Length > 1 && !int.TryParse(split[1], out page)) page = 1;

        var countDbResult = await _botCommandRepository.GetCommandCount();

        var embedBuilder = new EmbedBuilder();

        if (countDbResult.IsSuccess && countDbResult.Value != 0 && page >= 0)
        {
            var commandCount = countDbResult.Value;

            var pages = Math.Ceiling((decimal)commandCount / MaxPageSize);

            if (page <= pages)
            {
                embedBuilder.Title = "Saved commands";
                embedBuilder.Description = $"Page {page} of {pages} pages ({commandCount} saved commands)";
                embedBuilder.Color = new Color(0x9d03fc);

                var commands = await _botCommandRepository.GetCommands(page - 1, MaxPageSize);

                if (commands.IsSuccess)
                {
                    embedBuilder.Fields.AddRange(commands.Value.Select(x => new EmbedFieldBuilder
                    {
                        Name = x.Key,
                        Value = x.Type == BotCommand.CommandType.FILE ? x.FileName : x.Content
                    }).ToList());
                }
            }
            else
            {
                embedBuilder.Title = "Error";
                embedBuilder.Color = Color.Red;
                embedBuilder.Description = "Invalid page";  
            }
        }
        else
        {
            embedBuilder.Title = "Error";
            embedBuilder.Color = Color.Red;
            embedBuilder.Description = page < 0 ? "Invalid page" : "No commands found";  
        }
        
        await context.SendEmbedAsync(embedBuilder.Build());

        return Result.Ok();
    }
}