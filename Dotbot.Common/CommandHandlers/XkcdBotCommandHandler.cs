using System.Drawing;
using Dotbot.Common.Models;
using Dotbot.Common.Services;
using FluentResults;

namespace Dotbot.Common.CommandHandlers;

public class XkcdBotCommandHandler: BotCommandHandler
{
    public XkcdBotCommandHandler(IXkcdService xkcdService)
    {
        _xkcdService = xkcdService;
    }
    public override CommandType CommandType => CommandType.Xkcd;
    public override Privilege PrivilegeLevel => Privilege.Base;


    private readonly IXkcdService _xkcdService;
    
    protected override async Task<Result> ExecuteAsync(string content, IServiceContext context)
    {
        var strings = content.Split(' ');
        Result<XkcdComic> comic;
        if (strings.Length > 1 && int.TryParse(strings[1], out var comicNum))
        {
            comic = await _xkcdService.GetComic(comicNum);
        }
        else
        {
            comic = await _xkcdService.GetLatestComic();
        }

        if (comic.IsFailed)
        {
            await context.SendEmbedAsync(new FormattedMessage
            {
                Color = Color.Red,
                Description = "Failed to retrieve latest comic",
                Title = "Error"
            });
            return Result.Fail("Failed to retrieve latest comic");
        }

        await context.SendEmbedAsync(new FormattedMessage
        {
            ImageUrl = comic.Value.Img,
            Title = $"XKCD: #{comic.Value.Num}",
            Color = Color.FromArgb(157,3, 252),
            Fields = new List<FormattedMessage.Field>
            {
                new()
                {
                    Name = "Title",
                    Value = comic.Value.Title,
                    Inline = true
                },
                new()
                {
                    Name = "Published",
                    Value = $"{comic.Value.Day}/{comic.Value.Month}/{comic.Value.Year}",
                    Inline = true
                },
                new()
                {
                    Name = "Alt Text",
                    Value = comic.Value.Alt,
                    Inline = true
                }
            }
        });
        return Result.Ok();
    }
}