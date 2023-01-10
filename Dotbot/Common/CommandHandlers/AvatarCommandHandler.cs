using Discord;
using Dotbot.Common.Models;
using FluentResults;
using static FluentResults.Result;

namespace Dotbot.Common.CommandHandlers;

public class AvatarCommandHandler : IBotCommandHandler
{
    public CommandType CommandType => CommandType.Avatar;

    public async Task<Result> HandleAsync(string content, IServiceContext context)
    {
        var mentionedIds = await context.GetUserMentionsAsync();

        foreach (var user in mentionedIds)
        {
            await SendAvatarEmbed(user, context);
        }

        return Ok();
    }

    private static async Task SendAvatarEmbed(User user, IServiceContext context)
    {
        await context.SendEmbedAsync(new EmbedBuilder
        {
            Title = user.Username, 
            ImageUrl = user.EffectiveAvatarUrl
        }.Build());
    }
}